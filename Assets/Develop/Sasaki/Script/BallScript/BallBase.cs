using UnityEngine;

/// <summary>
/// 球の基底クラス
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class BallBase : MonoBehaviour
{
    private Rigidbody _rb;

    private float _ballRadius;　　
    private float _friction = 0.99f;　//転がる時の球の摩擦力

    private Vector3 _velocity;　

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
    /// 移動方向と速度を更新する処理 (ショットで使用　＋　加速装置で使用)
    /// </summary>
    public void UpdateMoveVelocity(Vector3 direction, float addForce = 1)
    {
        _velocity = direction * addForce;

        _rb.linearVelocity = _velocity;
    }

    /// <summary>
    /// 球の半径から回転を更新する処理　（進行方向へ回転）
    /// </summary>
    /// <param name="direction"></param>
    private void UpdateRotate(Vector3 direction)
    {

        if (_ballRadius != gameObject.transform.localScale.x || _ballRadius == 0)
        {
            _ballRadius = gameObject.transform.localScale.x;
        }

        // 進行方向に応じた回転軸を求める式
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        // 移動距離から回転角度を計算
        float distance = direction.magnitude * Time.deltaTime;
        float angle = (distance / _ballRadius) * Mathf.Rad2Deg;

        transform.Rotate(rotationAxis, angle, Space.World);
    }
}
