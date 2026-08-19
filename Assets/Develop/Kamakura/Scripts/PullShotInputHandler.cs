using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
/// <summary>
/// 引っ張り操作を検知し、ショット力を計算してRigidbodyに加えるクラス
/// </summary>
public class PullShotInputHandler : MonoBehaviour
{
    [Header("引っ張り設定")]
    [Tooltip("最小引っ張り距離（これ未満はショットキャンセル）")]
    [SerializeField] private float _minDragDistance = 0.5f;
    [Tooltip("最大引っ張り距離")]
    [SerializeField] private float _maxDragDistance = 5.0f;
    [Tooltip("最大ショット力")]
    [SerializeField] private float _maxShotPower = 20f;

    [Header("参照")]
    [Tooltip("ショット対象のRigidbody（未設定時は_shootTargetから取得）")]
    [SerializeField] private Rigidbody _targetRigidbody;
    [Tooltip("操作に使用するカメラ（未設定時はCamera.mainを使用）")]
    [SerializeField] private Camera _camera;

    [Header("イベント（拡張用）")]
    [Tooltip("ショット時に発火。引数は XZ平面のショット力ベクトル")]
    [SerializeField] private UnityEvent<Vector3> _onShot;
    [Tooltip("ドラッグ開始時に発火。引数は開始ワールド座標")]
    [SerializeField] private UnityEvent<Vector3> _onDragStart;
    [Tooltip("ドラッグ更新時に発火。引数は (開始位置, ドラッグベクトル, パワー割合)")]
    [SerializeField] private UnityEvent<Vector3, Vector3, float> _onDragUpdate;
    [Tooltip("ドラッグ終了時に発火")]
    [SerializeField] private UnityEvent _onDragEnd;

    public UnityEvent<Vector3> OnShot => _onShot;
    public UnityEvent<Vector3> OnDragStartEvent => _onDragStart;
    public UnityEvent<Vector3, Vector3, float> OnDragUpdateEvent => _onDragUpdate;
    public UnityEvent OnDragEndEvent => _onDragEnd;

    public float MaxDragDistance => _maxDragDistance;
    public float MinDragDistance => _minDragDistance;

    private bool _isDragging = false;
    private Vector3 _dragStartPos;
    private int _activePointerId = -1;
    private void Awake()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }

        if (_targetRigidbody == null)
        {
            _targetRigidbody = GetComponent<Rigidbody>();
        }

    }

    private void Update()
    {
        if (_camera == null) return;

        Pointer currentPointer = Pointer.current;
        if (currentPointer == null) return;

        float targetY = _targetRigidbody != null ? _targetRigidbody.transform.position.y : 0f;


        // ドラッグ開始
        if (currentPointer.press.wasPressedThisFrame)
        {
            // UIをタップしている場合は入力を無視する
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(currentPointer.deviceId))
            {
                return;
            }
            if (TryGetWorldPosition(currentPointer.position.ReadValue(), targetY, out Vector3 worldPos))
            {
                _isDragging = true;
                _activePointerId = currentPointer.deviceId;
                _dragStartPos = worldPos;
                // ドラッグ開始位置を通知（Rigidbodyがある場合はその位置を優先）
                Vector3 originPos = _targetRigidbody != null ? _targetRigidbody.position : worldPos;
                _onDragStart?.Invoke(originPos);
            }
        }
        // ドラッグ中（矢印表示・予測線などの通知）
        else if (currentPointer.press.isPressed && _isDragging)
        {
            if (currentPointer.deviceId != _activePointerId) return;
            if (TryGetWorldPosition(currentPointer.position.ReadValue(), targetY, out Vector3 currentWorldPos))
            {
                Vector3 dragVector = currentWorldPos - _dragStartPos;
                dragVector.y = 0f;

                float clampedDistance = Mathf.Clamp(dragVector.magnitude, 0f, _maxDragDistance);
                float powerRatio = Mathf.Clamp01(clampedDistance / _maxDragDistance);

                // ドラッグ更新位置を通知（Rigidbodyがある場合はその位置を優先）
                Vector3 originPos = _targetRigidbody != null ? _targetRigidbody.position : _dragStartPos;
                _onDragUpdate?.Invoke(originPos, dragVector, powerRatio);
            }
        }
        // ドラッグ終了（ショット発射またはキャンセル）
        else if (currentPointer.press.wasReleasedThisFrame && _isDragging)
        {
            if (currentPointer.deviceId != _activePointerId) return;
            _isDragging = false;
            _activePointerId = -1;
            _onDragEnd?.Invoke();

            if (TryGetWorldPosition(currentPointer.position.ReadValue(), targetY, out Vector3 currentWorldPos))
            {
                Vector3 dragVector = currentWorldPos - _dragStartPos;
                dragVector.y = 0f;
                float dragDistance = dragVector.magnitude;

                if (dragDistance < _minDragDistance)
                {
                    GameDebug.Log("[PullShotInputHandler] ドラッグ距離不足のためキャンセル", this);
                    return;
                }

                ShootBall(dragVector);
            }
        }
    }
    private void OnDisable()
    {
        // ドラッグ中に無効化された場合、ドラッグ終了イベントを発火
        if (_isDragging)
        {
            _isDragging = false;
            _activePointerId = -1;
            _onDragEnd?.Invoke();
        }
    }

    /// <summary>
    /// スクリーン座標を、指定した高さ（Y座標）のXZ平面上のワールド座標に変換する
    /// </summary>
    private bool TryGetWorldPosition(Vector2 screenPosition, float groundY, out Vector3 worldPosition)
    {
        if (_camera == null)
        {
            worldPosition = Vector3.zero;
            return false;
        }

        Ray ray = _camera.ScreenPointToRay(screenPosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));

        if (groundPlane.Raycast(ray, out float distance))
        {
            worldPosition = ray.GetPoint(distance);
            return true;
        }

        worldPosition = Vector3.zero;
        return false;
    }


    /// <summary>
    /// シュートを実行する
    /// <param name="dragVector">ドラッグベクトル（現在位置 - 開始位置</param>
    /// </summary>
    private void ShootBall(Vector3 dragVector)
    {
        if (_targetRigidbody == null) return;

        // 1. 引っ張った方向の逆向き（ショット方向）
        Vector3 shotDirection = -dragVector.normalized;

        // 2. 距離のクランプ
        float clampedDistance = Mathf.Clamp(dragVector.magnitude, _minDragDistance, _maxDragDistance);

        // 3. パワー割合の算出 (0.0 〜 1.0)
        float powerRatio = Mathf.Clamp01(clampedDistance / _maxDragDistance);

        // 4. 最終的なショット力
        Vector3 shotForce = shotDirection * (powerRatio * _maxShotPower);

        GameDebug.Log($"[PullShotInputHandler] ショット発動: 方向: {shotDirection}, 距離: {clampedDistance:F2}, パワー割合: {powerRatio:P0}, 力: {shotForce}", this);

        // 5. 発射
        _targetRigidbody.AddForce(shotForce, ForceMode.Impulse);
        _onShot?.Invoke(shotForce);
    }

}
