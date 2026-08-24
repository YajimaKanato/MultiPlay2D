using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// 9ボールで、ターンの切り替えを行うクラス
/// </summary>
public class TurnController : MonoBehaviour
{
    public enum Turn
    {
        Player1,
        Player2,
        Player3,
        Player4,
        count
    }

    /// <summary> 参加プレイヤーを格納するList </summary>
    List<Turn> players = null;

    /// <summary> 現在誰のターン </summary>
    [SerializeField] Turn _currentTurn = Turn.Player1;

    public Turn CurrentTurn => _currentTurn;

    [ContextMenu("GetPlayerNum")]
    void test()
    {
        ConfirmPlayerNum(4);
    }

    /// <summary> プレイヤー人数を確定させるメソッド </summary>
    /// param name="playerNum"> プレイヤー人数 </param>
    //参加人数情報を保持しているクラスが、試合開始時に呼び出したい。途中退出が発生した際にplayer配列を調整するメソッドも必要かも。
    public void ConfirmPlayerNum(int playerNum)
    {
        if (playerNum > (int)Turn.count) return;

        //プレイヤーの人数を取得する処理
        players = new List<Turn>();
        for (int i = 0; i < playerNum; i++)
        {
            players.Add((Turn)i);
        }

        GameDebug.Log($"参加プレイヤーは {players.Count} 人です。");
    }

    ///</summary> 途中退出をしたプレイヤーを参加プレイヤーリストから削除するメソッド <summary>
    /// param name="playerNum"> 途中退出したプレイヤーの番号 </param>
    public void RemovePlayer()
    {
        players.Remove((Turn)2);
    }
    //public void RemovePlayer(int playerNum)
    //{
    //    players.Remove((Turn)playerNum - 1);
    //}


    /// <summary> ターンの切り替えを行うメソッド。ショットの結果確認終了時に呼び出したい。 </summary>
    public Turn ChangeTurn()
    {
        //ターンの切り替え処理
        int indexOfCurrentTurn = (int)_currentTurn;

        indexOfCurrentTurn++;

        if (indexOfCurrentTurn >= players.Count)
        {
            indexOfCurrentTurn = 0;
        }

        _currentTurn = players[indexOfCurrentTurn];

        GameDebug.Log("次のターンは " + _currentTurn + " です。");
        return _currentTurn;
    }
}
