using UnityEngine;

/// <summary> ブレイクショットの処理を行うフェーズ </summary>
public class BreakShotFase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;

    public GameState StateType => GameState.BreakeShotFase;

    public BreakShotFase(GameStateMachine stateMachine, GameStateController gameStateController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        //ショット後、全ての球が止まったらドラフトフェーズに移行する
        if (_gameStateController.IsShotted && _gameStateController.HadAllBallsStop)
        {
            _stateMachine.ChangeState(new DraftFase(_stateMachine, _gameStateController));
        }
    }

    public void Exit()
    {

    }
}
