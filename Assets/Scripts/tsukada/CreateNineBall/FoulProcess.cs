using NUnit.Framework.Interfaces;
using UnityEngine;
using GameState = GameStateController.GameState;


/// <summary> ファールに関する処理を行うクラス </summary>
public class FoulProcess : MonoBehaviour
{
    [Tooltip("不正に9番球をポケットしたファールの有無を管理するフラグ")]
    bool _hasFoulOfPocketNineBall = false;

    [Tooltip("不正に9番球をポケットしたファール以外のファールの発生有無を管理するフラグ")]
    bool _hasFoul = false;

    [SerializeField] TurnController _turnController = default;
    [SerializeField] GameStateController _gameStateController = default;

    public void Foul()
    {
        GameDebug.Log($"{_turnController.CurrentTurn} がファールしました。");
        _hasFoul = true;
    }

    [ContextMenu("FoulOfPocketNineBall")]
    /// <summary> 不正に9番球をポケットした際のファール処理 </summary>
    public void FoulOfPocketNineBall()
    {
        GameDebug.Log($"{_turnController.CurrentTurn} が不正に9番球をポケットしました。");
        _hasFoulOfPocketNineBall = true;
    }

    [ContextMenu("ファール処理")]
    /// <summary> ファール結果を統合し、結果によりファール処理を行うメソッド。全ての球が停止したら呼び出されたい </summary>
    public void IntegrationFoulResult()
    {
        if (_hasFoul)
        {
            GameDebug.Log("好きな場所に手球を配置する");
            //次のプレイヤーが手球を好きな場所に置けるようにする処理、もしくはその通知
            //新しく「Foulfase」を作って、そこでファール後の特殊操作したほうがいいかも？
        }

        if (_hasFoulOfPocketNineBall)
        {
            GameDebug.Log("9番球を初期位置に戻す");
            //9ボールを初期位置に戻す処理、もしくはその通知(全ての球の停止通知を受けてから行うべし)
        }
        
        //手球を置いたら、ターンを切り替える処理を行う↓
        _gameStateController.ChangeGameState(GameState.DraftFase);      //(仮)
        _hasFoul = false;
        _hasFoulOfPocketNineBall = false;
    }
}
