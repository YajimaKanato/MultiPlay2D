using UnityEngine;

/// <summary> ブレイクショットの処理を行うフェーズ </summary>
public class BreakShotPhase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;
    private CollideBalls _collideBalls;
    private TurnController _turnController;

    public GameState StateType => GameState.BreakShotPhase;

    public BreakShotPhase(GameStateMachine stateMachine, GameStateController gameStateController, CollideBalls collideBalls, TurnController turnController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
        _collideBalls = collideBalls;
        _turnController = turnController;
    }

    public void Enter(float limitTime)
    {

    }

    public void Update()
    {
        //ショット後、全ての球が止まったらドラフトフェーズに移行する
        if (_gameStateController.IsShotted && _gameStateController.HadAllBallsStop)
        {
            _stateMachine.ChangeState(new DraftPhase(_stateMachine, _gameStateController, _collideBalls, _turnController));
        }
    }

    public void Exit()
    {

    }
}
