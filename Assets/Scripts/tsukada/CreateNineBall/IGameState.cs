using UnityEngine;

/// <summary> 各ゲームフェーズに処理を持たせるインターフェース </summary>
public interface IGameState
{
    GameState StateType { get; }

    /// <summary> フェーズ開始時に実行する処理 </summary>
    //Enterメソッドで制限時間を渡すゴリ押し処理。制限時間とは無縁なフェーズにまで引数が渡されるので美しくない。
    //リファクタリング時に修正予定。
    void Enter(float limitTime);

    /// <summary> フェーズ中に実行されるUpdate処理 </summary>
    void Update();

    /// <summary> フェーズ終了時に実行する処理 </summary>
    void Exit();
}
