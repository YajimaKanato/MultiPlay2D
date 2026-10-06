using Unity.VisualScripting;
using UnityEngine;

/// <summary> ドラフトフェーズの処理を行うクラス</summary>
public class DraftPhase : IGameState
{
    private GameStateMachine _stateMachine;
    private GameStateController _gameStateController;
    private CollideBalls _collideBalls;
    private TurnController _turnController;

    float _timer = 0f;

    private float _draftPhaseTime = 30.0f;

    public GameState StateType => GameState.DraftPhase;

    public DraftPhase(GameStateMachine stateMachine, GameStateController gameStateController, CollideBalls collideBalls, TurnController turnController)
    {
        _stateMachine = stateMachine;
        _gameStateController = gameStateController;
        _collideBalls = collideBalls;
        _turnController = turnController;
    }

    public void Enter(float limitTime)
    {
        _timer = 0f;
        _draftPhaseTime = limitTime;        //制限時間を設定
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
            _gameStateController.SwitchFlagOfPocketAnyBall();
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
        if (_timer >= _draftPhaseTime)
        {
            _stateMachine.ChangeState(new ShotPhase(_stateMachine, _gameStateController, _collideBalls, _turnController));
        }

        //if(全員の行動が完了したら)
        if (Input.GetKeyDown(KeyCode.Return))    //仮の条件
        {
            _stateMachine.ChangeState(new ShotPhase(_stateMachine, _gameStateController, _collideBalls, _turnController));
        }
    }

    public void Exit()
    {

    }
}
