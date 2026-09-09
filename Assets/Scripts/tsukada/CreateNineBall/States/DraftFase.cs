using Unity.VisualScripting;
using UnityEngine;

/// <summary> ドラフトフェーズの処理を行うクラス</summary>
public class DraftFase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;
    [SerializeField] private CollideBalls _collideBalls = null;
    [SerializeField] private TurnController _turnController = null;

    float _timer = 0f;

    [Tooltip("ドラフトフェーズの制限時間")]
    [SerializeField] private float _draftFaseTime = 30.0f;

    public GameState StateType => GameState.DraftFase;

    public DraftFase(GameStateMachine stateMachine, GameStateController gameStateController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
    }

    public void Enter()
    {
        _timer = 0f;
        _collideBalls.RemoveObjectBallNum();        //ポケットしたボールを存在管理Listから削除する
        _gameStateController.SwitchFlagOfShotted();    //ショット済みフラグをリセットする

        //ポケットもファールもしなかった場合、ターンを切り替える
        if (!_gameStateController.HadPocketAnyBall)
        {
            _turnController.ChangeTurn();    
        }

        //フラグリセット
        if (_gameStateController.HadPocketAnyBall)
        {
            _gameStateController.SwitchFlagOfPocketAnyBall(); _gameStateController.SwitchFlagOfPocketAnyBall();
        }

        if (_gameStateController.HadFoul)
        {
            _gameStateController.SwitchFlagofFouled();
        }

    }

    public void Update()
    {
        _timer += Time.deltaTime;

        //ドラフトフェーズの制限時間を超えたらショットフェーズに移行する
        if (_timer >= _draftFaseTime)
        {
            _stateMachine.ChangeState(new ShotFase(_stateMachine, _gameStateController));
        }

        //if(全員の行動が完了したら)
        if (Input.GetKeyDown(KeyCode.Return))    //仮の条件
        {
            _stateMachine.ChangeState(new ShotFase(_stateMachine, _gameStateController));
        }
    }

    public void Exit()
    {

    }
}
