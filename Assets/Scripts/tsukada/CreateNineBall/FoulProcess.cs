using UnityEngine;


/// <summary> ファールに関する処理を行うクラス </summary>
public class FoulProcess : MonoBehaviour
{
    /////<summary> 不正に9番球をポケットしたファール以外のファールの発生有無を管理するフラグ </summary>
    //bool _hasFoul = false;

    [SerializeField] TurnController _turnController = null;
    [SerializeField] GameStateMachine _gameStateMachine = null;
    [SerializeField] GameStateController _gameStateController = null;

    /// <summary> 一般的なファール </summary>
    public void Foul()
    {
        GameDebug.Log($"{_turnController.CurrentTurn} がファールしました。");
        _gameStateController.SwitchFlagofFouled();
    }

    /// <summary> ファール結果を統合し、結果によりファール処理を行うメソッド。全ての球が停止したら呼び出されたい </summary>
    public void IntegrationFoulResult()
    {
        if (_gameStateController.HadFoul)
        {
            GameDebug.Log("好きな場所に手球を配置する");
            //次のプレイヤーが手球を好きな場所に置けるようにする処理、もしくはその通知
            _gameStateMachine.ChangeState(new FoulFase(_gameStateMachine, _gameStateController));
        }
    }
}
