using UnityEngine;

/// <summary> ゲームの結果を表示するフェーズ </summary>
public class ResultPhase : IGameState
{
    private GameStateController _gameStateController;
    private TurnController _turnController;

    public GameState StateType => GameState.ResultPhase;

    public ResultPhase(GameStateController gameStateController, TurnController turnController)
    {
        _gameStateController = gameStateController;
        _turnController = turnController;
    }

    public void Enter(float limitTime)
    {
        GameDebug.Log($"ゲームが終了しました。 " + $"{_turnController.CurrentTurn} が勝利しました。");

        //試合結果、スコアを表示
        
    }

    public void Update()
    {
        // 「もう一度遊ぶ」
        // 「ロビーに戻る」
        // 「ホームに戻る」
    }

    public void Exit()
    {

    }
}
