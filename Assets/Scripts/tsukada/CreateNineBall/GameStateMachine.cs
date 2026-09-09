using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    /// <summary> 現在のフェーズ </summary>
    private IGameState _currentState;

    //プロパティ
    public GameState CurrentState => _currentState.StateType;

    /// <summary> フェーズを切り替えるメソッド </summary>
    /// <param name="newState"> 切り替わり先フェーズ </param>
    public void ChangeState(IGameState newState)
    {
        _currentState?.Exit();      //フェーズを切り替える前に切り替え前フェーズのExitメソッドを実行

        _currentState = newState;   //フェーズ切り替え

        GameDebug.Log($"フェーズが {_currentState.StateType} に移行しました");

        _currentState.Enter();      //フェーズ切り替え後に切り替え後フェーズのEnterメソッドを実行
    }

    //各フェーズにおいてUpdate処理は常に実行
    public void Update()
    {
        _currentState?.Update();
    }
}
