using UnityEngine;

/// <summary> ショットフェーズに関する処理を行うクラス </summary>
public class ShotPhase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;
    private CollideBalls _collideBalls;
    private TurnController _turnController;

    private float _timer;
    private float _shotPhaselimitTime = 30f;

    public GameState StateType => GameState.ShotPhase;

    //コンストラクタ
    public ShotPhase(GameStateMachine stateMachine, GameStateController gameStateController, 
                        CollideBalls collideBalls, TurnController turnController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
        _collideBalls = collideBalls;
        _turnController = turnController;
    }

    public void Enter(float limitTime)
    {
        _timer = 0f;
        _shotPhaselimitTime = limitTime;        //フェーズ制限時間保有クラス(PhaseTimeLimitData)から参照し制限時間を設定
    }

    public void Update()
    {
        //制限時間を超えたらドラフトフェーズに移行する
        if (_timer >= _shotPhaselimitTime)
        {
            _stateMachine.ChangeState(new DraftPhase(_stateMachine, _gameStateController, _collideBalls, _turnController));
            return;
        }

        //ショット後、全ての球が止まり、ファールをしていなかったらリザルトフェーズに移行する
        if (_gameStateController.IsShotted && !_gameStateController.HadFoul && _gameStateController.IsGameClear)
        {
            _stateMachine.ChangeState(new ResultPhase(_gameStateController, _turnController));
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
