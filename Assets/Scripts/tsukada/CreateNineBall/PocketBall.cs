using UnityEngine;

/// <summary>
/// 9ボールで、ボールがポケットに入った時の処理を行うクラス
/// </summary>
public class PocketBall : MonoBehaviour
{
    [SerializeField] FoulProcess _foulProcess = default;

    [SerializeField] CollideBalls _collideBalls = default;

    [SerializeField] GameStateController _gameStateController = default;


    public void PocketObjectBall(int ballNumber)
    {
        if (ballNumber == 0)        //手球が落ちたらファール
        {
            GameDebug.Log("手球が落ちました。");
            _foulProcess.Foul();
        }
        else if (ballNumber == 9)        //手球が最小の的球に当たる前に9ボールが落ちたら、9ボールファール
        {
            if (_collideBalls.HasCollideMinObjectBall)
            {
                //9ボールが落ちた場合の処理。勝敗判定を行う。
                GameDebug.Log("合法的に9ボールが落ちました。");
                _gameStateController.MeetConditionOfGameClear();
            }
            else
            {
                GameDebug.Log("不正に9ボールが落ちました。");
                _foulProcess.FoulOfPocketNineBall();
            }
            
        }
        else
        {
            GameDebug.Log($"的球 {ballNumber} が落ちました。");
            _collideBalls.RemoveObjectBallNum(ballNumber);      //落ちた的球の番号を配列から削除
        }
        
    }
}
