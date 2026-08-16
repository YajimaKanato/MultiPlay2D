using UnityEngine;

public class CueBallController : BallBase
{
    protected override void Awake()
    {
        base.Awake();
        UpdateMoveVelocity(new Vector3(-1, 0, 0), 40);
    }

    protected override void BaseUpdate()
    {
        base.BaseUpdate();

    }

    protected override void BaseFixedUpdate()
    {
        base.BaseFixedUpdate();
    }
}
