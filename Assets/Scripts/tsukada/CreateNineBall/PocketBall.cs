using UnityEngine;

/// <summary> 9ボールで、ボールがポケットに入った時の処理を行うクラス </summary>
public class PocketBall : MonoBehaviour
{
    [SerializeField] FoulProcess _foulProcess = null;

    [SerializeField] CollideBalls _collideBalls = null;

    [SerializeField] GameStateController _gameStateController = null;

    ///

    /// <summary> ポケットに落ちた球によってファールや勝利条件達成を判定するメソッド </summary>
    /// <param name="ballNumber"></param>
    public void PocketObjectBall(Balls PocketBall)
    {
        _gameStateController.SwitchFlagOfPocketAnyBall();        //何かしらの球が落ちたフラグを切り替える

        if (PocketBall == Balls.CueBall)        //手球が落ちたらファール
        {
            GameDebug.Log("手球が落ちました。");
            _foulProcess.Foul();
        }
        else if (PocketBall == Balls.NineBall)        //手球が最小の的球に当たる前に9ボールが落ちたら、9ボールファール
        {
            if (_collideBalls.HasCollideMinObjectBall)
            {
                //9ボールが落ちた場合の処理。勝敗判定を行う。
                GameDebug.Log("合法的に9ボールが落ちました。");
                _gameStateController.MeetConditionOfGameClear();
            }
            else
            {
                GameDebug.Log("不正に9番球が落ちました。");
                _foulProcess.Foul();
            }
        }
        else
        {
            GameDebug.Log($"的球 {PocketBall} が落ちました。");
            _collideBalls.MemorizePocketedBall(PocketBall);       //落ちた的球の番号を記憶
        }
    }
}
