using UnityEngine;

public class GameStateMachine
{
    /// <summary> 現在のフェーズ </summary>
    private IGameState _currentState;

    /// <summary> Draftフェーズの制限時間 </summary>
    private float _draftPhaseTime;
    /// <summary> Shotフェーズの制限時間 </summary>
    private float _shotPhaseTime;

    //プロパティ
    public GameState CurrentState => _currentState.StateType;
    public float DraftPhaseTime { set => _draftPhaseTime = value; }
    public float ShotPhaseTime { set => _shotPhaseTime = value; }

    /// <summary> フェーズを切り替えるメソッド </summary>
    /// <param name="newState"> 切り替わり先フェーズ </param>
    public void ChangeState(IGameState newState)
    {
        _currentState?.Exit();      //フェーズを切り替える前に切り替え前フェーズのExitメソッドを実行

        _currentState = newState;   //フェーズ切り替え

        GameDebug.Log($"フェーズが {_currentState.StateType} に移行しました");

        //フェーズEnterで制限時間を渡すゴリ押し処理。リファクタリング時に修正予定。
        //フェーズ切り替え後に切り替え後フェーズのEnterメソッドを実行
        if (_currentState.StateType == GameState.DraftPhase)
        {
            _currentState.Enter(_draftPhaseTime);
        }
        else if (_currentState.StateType == GameState.ShotPhase)
        {
            _currentState.Enter(_shotPhaseTime);
        }
        else
        {
            _currentState.Enter(0);
        }
    }

    //各フェーズにおいてUpdate処理は常に実行
    public void Update()
    {
        _currentState?.Update();
    }
}
