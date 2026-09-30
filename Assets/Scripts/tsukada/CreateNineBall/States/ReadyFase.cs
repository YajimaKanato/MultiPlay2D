using UnityEngine;

/// <summary> ゲーム開始前の準備を行うフェーズ </summary>
public class ReadyFase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;

    public GameState StateType => GameState.ReadyFase;

    public ReadyFase(GameStateMachine stateMachine, GameStateController gameStateController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
    }

    public void Enter()
    {
        GameDebug.Log("ゲーム開始前のReadyFaseから始まります");
    }

    public void Update()
    {
        //プレイヤーの準備が整ったらブレイクショットフェーズに移行する
        if (Input.GetKeyDown(KeyCode.Return))       //仮の条件
        {
            _stateMachine.ChangeState(new BreakShotFase(_stateMachine, _gameStateController));
        }
    }

    public void Exit()
    {

    }
}
