using UnityEngine;

/// <summary> ゲーム開始前の準備を行うフェーズ </summary>
public class ReadyPhase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;
    private CollideBalls _collideBalls;
    private TurnController _turnController;

    public GameState StateType => GameState.ReadyPhase;

    public ReadyPhase(GameStateMachine stateMachine, GameStateController gameStateController, CollideBalls collideBalls, TurnController turnController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
        _collideBalls = collideBalls;
        _turnController = turnController;
    }

    public void Enter(float limitTime)
    {
        GameDebug.Log("ゲーム開始前のReadyPhaseから始まります");
    }

    public void Update()
    {
        //プレイヤーの準備が整ったらブレイクショットフェーズに移行する
        if (Input.GetKeyDown(KeyCode.Return))       //仮の条件
        {
            _stateMachine.ChangeState(new BreakShotPhase(_stateMachine, _gameStateController, _collideBalls, _turnController));
        }
    }

    public void Exit()
    {

    }
}
