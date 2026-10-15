using UnityEngine;

/// <summary>
/// 球の基底クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class BallBase : MonoBehaviour
{
    
    [Header("球の設定")]
    [Tooltip("球の半径")]
    [SerializeField] private float _ballRadius;
    [Tooltip("球の摩擦力")]
    [SerializeField] private float _friction = 0.99f;
    [Tooltip("球が停止したとみなす閾値")] 
    [SerializeField] private float _stopThresholdSpeed = 0.05f;

    [SerializeField] private Rigidbody _rb;
    [Tooltip("球が停止中か判定")]
    [SerializeField] private bool _isStopped;


    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        BaseUpdate();
    }

    private void FixedUpdate()
    {
        BaseFixedUpdate();
    }


    protected virtual void BaseUpdate()
    {
        UpdateRotate(_rb.linearVelocity);
    }

    /// <summary>
    /// 球の摩擦更新処理
    /// </summary>
    protected virtual void BaseFixedUpdate()
    {
        if (_rb.linearVelocity == Vector3.zero) return;

        //球がストップしたか判定 (閾値以下のときは0を代入)
        if (_rb.linearVelocity.magnitude < _stopThresholdSpeed)
        {
            _rb.linearVelocity = Vector3.zero;
            _isStopped = true;
            GameDebug.Log($"{gameObject.name}球が止まりました");
        }
        else
        {
            //摩擦処理
            _rb.linearVelocity *= _friction;
            _isStopped = false;
        }
    }

    /// <summary>
    ///  速度の倍率更新処理
    /// </summary>
    /// <param name="velocity">速度の倍率</param>
    public void ApplySpeedMultiplier(float speedMultiplier)
    {
        _rb.linearVelocity *= speedMultiplier;
    }

    /// <summary>
    /// 球の半径から回転を更新する処理
    /// </summary>
    /// <param name="direction">移動方向（進行方向へ回転）</param>
    private void UpdateRotate(Vector3 direction)
    {     
        // 進行方向に応じた回転軸を求める式
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        // 移動距離から回転角度を計算
        float distance = direction.magnitude * Time.deltaTime;
        float angle = (distance / _ballRadius) * Mathf.Rad2Deg;

        transform.Rotate(rotationAxis, angle, Space.World);
    }

    /// <summary>
    /// 球の大きさを更新する処理
    /// </summary>
    /// <param name="radius">ボールの半径（直径に変換して適用する）</param>
    public void UpdateBallRadius(float radius)
    {
        transform.localScale = new Vector3(radius, radius, radius) * 2;

        _ballRadius = radius; 
    }
}
