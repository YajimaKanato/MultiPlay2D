using UnityEngine;
using System.Collections.Generic;
using System.Linq;


/// <summary> 9ボールで、手球がボールに衝突した時の処理を行うクラス </summary>
public class CollideBalls : MonoBehaviour
{
    ///<summary> 存在している的球の番号を格納するList </summary>
    List<Balls> _existObjectBallNumber = new List<Balls> { Balls.OneBall, Balls.TwoBall, Balls.ThreeBall,
                                                            Balls.FourBall, Balls.FiveBall, Balls.SixBall,
                                                            Balls.SevenBall, Balls.EightBall, Balls.NineBall};

    /// <summary> 落下したボールを記憶(一時保存)しておくList </summary>
    List<Balls> _pocketBalls = new List<Balls>();

    ///<summary> 存在するうちの最小値の的球 </summary>
    Balls _minObjectBallNumber = Balls.OneBall;

    ///<summary> いずれかの的球に衝突したかどうかのフラグ </summary>
    bool _hasCollideAnyObjectBall = false;

    ///<summary> 最小値の的球に衝突したかどうかのフラグ </summary>
    bool _hasCollideMinObjectBall = false;

    [SerializeField] FoulProcess _foulProcess = null;

    //プロパティ
    public Balls MinObjectBallNumber => _minObjectBallNumber;
    public bool HasCollideMinObjectBall => _hasCollideMinObjectBall;
    public List<Balls> PocketBalls => _pocketBalls;

    /// <summary> 手球が的球に衝突した時の処理を行うメソッド。最小値の的球に衝突する前に、他の的球に衝突した場合はファール処理を行う。 </summary>
    /// <param name="beBumpedBallNumber"> 衝突した的球の番号 </param>
    public void CollideCueBall(Balls beBumpedBallNumber)
    {
        _hasCollideAnyObjectBall = true;


        if (!_hasCollideMinObjectBall)          //まだ最小値の的球に衝突していないかどうか
        {
            if (beBumpedBallNumber == _minObjectBallNumber)     //最小値の的球に初めて衝突した場合
            {
                _hasCollideMinObjectBall = true;                                            //↑フラグを立てる
                GameDebug.Log($"手球が最小値の的球 {_minObjectBallNumber} に衝突しました。");
            }
            else if (beBumpedBallNumber != _minObjectBallNumber) //最小値の的球に衝突する前に、他の的球に衝突した場合
            {
                _foulProcess.Foul();                                                         //ファール
            }
        }
    }

    /// <summary> 存在している的球の中の最小値を求めるメソッド。shotフェーズ開始時か、ショット時に呼び出したい </summary>
    public void CheckMinObjectBallNum()
    {
        _minObjectBallNumber = _existObjectBallNumber.Min();
    }

    /// <summary> ポケットに落ちた的球の番号を配列から削除するメソッド </summary>
    public void RemoveObjectBallNum()
    {
        foreach (Balls removeBalls in _pocketBalls)
        {
            _existObjectBallNumber.Remove(removeBalls);
        }
    }

    /// <summary> ポケットに落ちた的球の記憶をリセットする </summary>
    public void ResetMemoryOfPocketBalls()
    {
        _pocketBalls.Clear();
    }

    /// <summary> 最小値の的球に衝突したかどうかのフラグをリセットするメソッド </summary>
    public void ResetCollideObjectBallFlag()
    {
        _hasCollideMinObjectBall = false;
        _hasCollideAnyObjectBall = false;
    }

    /// <summary> ポケットに落ちたボールを受け取り、記憶用List(_pocketBalls)に格納するメソッド </summary>
    public void MemorizePocketedBall(Balls pocketedBall)
    {
        _pocketBalls.Add(pocketedBall);
    }

    /// <summary> どの的球にも衝突しなかったとこと通知するメソッド。いらない可能性あり </summary>
    public void NotifyNoCollide()
    {
        if (!_hasCollideAnyObjectBall)
        {
            _foulProcess.Foul();
        }
    }
}
