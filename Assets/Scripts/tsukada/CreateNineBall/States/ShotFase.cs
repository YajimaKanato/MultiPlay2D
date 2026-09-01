using UnityEngine;

/// <summary> ショットフェーズに関する処理を行うクラス </summary>
public class ShotFase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;

    [SerializeField] TurnController _turnController = null;

    private float _timer;
    private float _limitTime = 30f;

    public GameState StateType => GameState.ShotFase;

    public ShotFase(GameStateMachine stateMachine, GameStateController gameStateController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
    }

    public void Enter()
    {
        _timer = 0f;
    }

    public void Update()
    {
        //制限時間を超えたらドラフトフェーズに移行する
        if (_timer >= _limitTime)
        {
            _stateMachine.ChangeState(new DraftFase(_stateMachine, _gameStateController));
            return;
        }

        //ショット後、全ての球が止まり、ファールをしていなかったらリザルトフェーズに移行する
        if (_gameStateController.IsShotted && !_gameStateController.HadFoul && _gameStateController.IsGameClear)
        {
            _stateMachine.ChangeState(new ResultFase(_gameStateController, _turnController));
            return;
        }

        //まだショットしていない間、制限時間をカウントする
        if (!_gameStateController.IsShotted)
        {
            _timer += Time.deltaTime;
        }
    }

    public void Exit()
    {

    }
}
