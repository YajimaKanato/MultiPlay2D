using UnityEngine;
/// <summary>
/// 引っ張り操作時に、触れた場所から指の引っ張り方向に向かって矢印を伸縮・表示するクラス
/// </summary>
public class PullArrowVisualizer : MonoBehaviour
{
    [Header("参照")]
    [Tooltip("矢印を表示するSpriteRenderer（未設定時は自身から取得）")]
    [SerializeField] private SpriteRenderer _arrowRenderer;

    [Tooltip("入力ハンドラー（設定すると自動でイベント購読します。UnityEventで手動設定も可）")]
    [SerializeField] private PullShotInputHandler _inputHandler;

    [Header("外観・スケール設定")]
    [Tooltip("矢印の幅（太さ）")]
    [SerializeField] private float _arrowWidth = 1.0f;

    [Tooltip("InputHandlerの最大引っ張り距離と矢印の最大長さを自動同期するか")]
    [SerializeField] private bool _syncWithInputHandler = true;

    [Tooltip("パワー最大（100%）時の矢印の最大長さ（ワールド距離）。_syncWithInputHandlerが無効またはHandler未設定時に使用")]
    [SerializeField] private float _maxArrowLength = 5.0f;

    [Tooltip("矢印の長さの追加倍率（微調整用）")]
    [SerializeField] private float _lengthMultiplier = 1.0f;

    [Tooltip("床との重なり（Zファイト）を防ぐためのY軸オフセット")]
    [SerializeField] private float _yOffset = 0.05f;

    private float _baseSpriteWidth = 1.0f;

    private void Awake()
    {
        if (_arrowRenderer == null)
        {
            _arrowRenderer = GetComponent<SpriteRenderer>();
        }

        if (_arrowRenderer != null && _arrowRenderer.sprite != null)
        {
            // スプライト本来の幅（PixelsPerUnit考慮後のワールド単位）を取得
            _baseSpriteWidth = _arrowRenderer.sprite.bounds.size.x;
            if (_baseSpriteWidth <= 0.0001f)
            {
                _baseSpriteWidth = 1.0f;
            }
        }

        // 初期状態は非表示
        HideArrow();
    }

    private void OnEnable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnDragStartEvent.AddListener(OnDragStart);
            _inputHandler.OnDragUpdateEvent.AddListener(OnDragUpdate);
            _inputHandler.OnDragEndEvent.AddListener(OnDragEnd);
        }
    }

    private void OnDisable()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnDragStartEvent.RemoveListener(OnDragStart);
            _inputHandler.OnDragUpdateEvent.RemoveListener(OnDragUpdate);
            _inputHandler.OnDragEndEvent.RemoveListener(OnDragEnd);
        }
        HideArrow();
    }

    /// <summary>
    /// ドラッグ開始時の処理
    /// </summary>
    /// <param name="startPos">タップ/クリックしたワールド座標</param>
    public void OnDragStart(Vector3 startPos)
    {
        if (_arrowRenderer == null) return;

        startPos.y += _yOffset;
        _arrowRenderer.transform.position = startPos;
        _arrowRenderer.transform.localScale = Vector3.zero;
        _arrowRenderer.gameObject.SetActive(true);
    }

    /// <summary>
    /// ドラッグ中の更新処理
    /// </summary>
    /// <param name="startPos">ドラッグ開始ワールド座標</param>
    /// <param name="dragVector">開始位置から現在位置へのベクトル（XZ平面）</param>
    /// <param name="powerRatio">パワー割合 (0.0 〜 1.0)</param>
    public void OnDragUpdate(Vector3 startPos, Vector3 dragVector, float powerRatio)
    {
        if (_arrowRenderer == null) return;

        float distance = dragVector.magnitude;
        if (distance < 0.001f)
        {
            _arrowRenderer.transform.localScale = Vector3.zero;
            return;
        }

        // 1. 位置の更新（触れた場所を基準にする）
        startPos.y += _yOffset;
        _arrowRenderer.transform.position = startPos;

        // 2. 回転の計算
        // XZ平面での指の引っ張り方向の角度（ラジアン -> 度）
        float targetAngle = Mathf.Atan2(dragVector.z, dragVector.x) * Mathf.Rad2Deg;

        float rotZ = targetAngle - 180f;
        _arrowRenderer.transform.rotation = Quaternion.Euler(90f, 0f, rotZ);

        // 3. スケール（長さ・太さ）の計算
        // 最大パワー時の目標長さ（ワールド単位）
        float maxLength = (_syncWithInputHandler && _inputHandler != null)
            ? _inputHandler.MaxDragDistance
            : _maxArrowLength;

        // パワー割合 (0.0〜1.0) に完全連動させて長さを算出
        float currentLength = maxLength * powerRatio;

        // スプライトの元幅に合わせてスケールを適用（powerRatioが1.0のときにmaxLengthになる）
        float scaleX = (currentLength / _baseSpriteWidth) * _lengthMultiplier;
        float scaleY = _arrowWidth;

        _arrowRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        if (!_arrowRenderer.gameObject.activeSelf)
        {
            _arrowRenderer.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// ドラッグ終了時の非表示処理
    /// </summary>
    public void OnDragEnd()
    {
        HideArrow();
    }

    private void HideArrow()
    {
        if (_arrowRenderer != null)
        {
            _arrowRenderer.gameObject.SetActive(false);
        }
    }
}
