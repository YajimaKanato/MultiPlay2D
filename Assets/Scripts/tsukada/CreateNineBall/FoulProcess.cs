using NUnit.Framework.Interfaces;
using UnityEngine;
using GameState = GameStateController.GameState;

public class FoulProcess : MonoBehaviour
{
    [SerializeField] TurnController _turnController = default;
    [SerializeField] GameStateController _gameStateController = default;

    [ContextMenu("Foul")]
    public void Foul()
    {
        GameDebug.Log($"{_turnController.CurrentTurn} がファールしました。");
        //次のプレイヤーが手球を好きな場所に置けるようにする処理、もしくはその通知
        //手球を置いたら、ターンを切り替える処理を行う↓
        _gameStateController.ChangeGameState(GameState.DraftFase);
    }

    [ContextMenu("FoulOfPocketNineBall")]
    public void FoulOfPocketNineBall()
    {
        GameDebug.Log($"{_turnController.CurrentTurn} が不正に9ボールをポケットしました。9ボールが初期位置に戻ります。");
        //9ボールを初期位置に戻す処理、もしくはその通知(全ての球の停止通知を受けてから行うべし)
        _gameStateController.ChangeGameState(GameState.DraftFase);
    }

    
}
