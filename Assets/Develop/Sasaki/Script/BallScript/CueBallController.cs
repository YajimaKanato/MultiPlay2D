using UnityEngine;

/// <summary>
/// 手球（CueBall）の入力操作と物理挙動を管理するクラス。
/// </summary>
public class CueBallController : BallBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

    }

    protected override void BaseFixedUpdate()
    {
        base.BaseFixedUpdate();
    }

    /// <summary>
    /// 手球の移動方向と速度の値を代入　＋　基底クラスで手球の移動を更新する処理
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="addForce"></param>
    public void SetCueBallVelocity(Vector3 direction, float addForce = 1)
    {
        base.UpdateMoveVelocity(direction, addForce);
    }
}
