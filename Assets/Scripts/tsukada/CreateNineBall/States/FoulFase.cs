using UnityEngine;

/// <summary> ファールが発生した場合の処理を行うフェーズ </summary>
public class FoulFase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;

    private TurnController _turnController;

    public GameState StateType => GameState.FoulFase;

    public FoulFase(GameStateMachine stateMachine, GameStateController gameStateController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
    }

    public void Enter()
    {
        _turnController.ChangeTurn();
    }

    public void Update()
    {
        //_collideBallsの_pocketBallsを参照し、含まれるボールを初期位置に戻すメソッドを呼び出す
        // ↑ 9番玉が含まれていた場合、9番玉'だけ'はフットスポットへ戻す？ ↑
        //if(手球を好きな場所へ配置した場合(配置した通知を受け取った場合))
        _stateMachine.ChangeState(new DraftFase(_stateMachine, _gameStateController));
    }

    public void Exit()
    {

    }
}
