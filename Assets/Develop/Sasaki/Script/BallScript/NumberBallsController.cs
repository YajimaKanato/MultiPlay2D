using UnityEngine;

public class NumberBallsController : BallBase
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
    public void SetNumberBallVelocity(Vector3 direction, float addForce = 1)
    {
        base.UpdateMoveVelocity(direction, addForce);
    }
}
