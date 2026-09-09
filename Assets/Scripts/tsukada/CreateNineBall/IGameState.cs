using UnityEngine;

/// <summary> 各ゲームフェーズに処理を持たせるインターフェース </summary>
public interface IGameState
{
    GameState StateType { get; }

    /// <summary> フェーズ開始時に実行する処理 </summary>
    void Enter();

    /// <summary> フェーズ中に実行されるUpdate処理 </summary>
    void Update();

    /// <summary> フェーズ終了時に実行する処理 </summary>
    void Exit();
}
