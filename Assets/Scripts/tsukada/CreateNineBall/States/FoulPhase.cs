using UnityEngine;

/// <summary> ファールが発生した場合の処理を行うフェーズ </summary>
public class FoulPhase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;

    private TurnController _turnController;
    private CollideBalls _collideBalls;

    public GameState StateType => GameState.FoulPhase;

    public FoulPhase(GameStateMachine stateMachine, GameStateController gameStateController, 
                        CollideBalls collideBalls, TurnController turnController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
        _collideBalls = collideBalls;
        _turnController = turnController;
    }

    public void Enter(float limitTime)
    {
        _turnController.ChangeTurn();
    }

    public void Update()
    {
        //_collideBallsの_pocketBallsを参照し、含まれるボールを初期位置に戻すメソッドを呼び出す
        // ↑ 9番玉が含まれていた場合、9番玉'だけ'はフットスポットへ戻す？ ↑
        //if(手球を好きな場所へ配置した場合(配置した通知を受け取った場合))
        _stateMachine.ChangeState(new DraftPhase(_stateMachine, _gameStateController, _collideBalls, _turnController));
    }

    public void Exit()
    {

    }
}
