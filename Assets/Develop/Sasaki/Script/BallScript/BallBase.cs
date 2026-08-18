using UnityEngine;

/// <summary>
/// 球の基底クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class BallBase : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private float _ballRadius;
    [Tooltip("転がる時の球の摩擦力")]
    [SerializeField] private float _friction = 0.99f;

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
        _rb.linearVelocity *= _friction;
    }

    /// <summary>
    /// 速度ベクトルを更新する処理 (ショットで使用　＋　加速装置で使用)   
    /// 引数は（速度ベクトル）
    /// </summary>
    public void UpdateMoveVelocity(Vector3 velocity)
    {
        _rb.linearVelocity = velocity;
    }

    /// <summary>
    /// 球の半径から回転を更新する処理　（進行方向へ回転）
    /// </summary>
    /// <param name="direction"></param>
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
    ///ボールの半径を更新する処理 
    /// </summary>
    public void UpdateBallRadius()
    {
        _ballRadius = transform.localScale.x;
    }
}
