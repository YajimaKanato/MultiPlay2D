using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary> 9ボールで、手球がボールに衝突した時の処理を行うクラス </summary>
public class CollideBalls : MonoBehaviour
{
    [Tooltip("存在している的球の番号を格納する配列")]
    List<int> _existObjectBallNumber = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    [Tooltip("存在するうちの最小値の的球")]
    int _minObjectBallNumber = 1;

    [Tooltip("いずれかの的球に衝突したかどうかのフラグ")]
    bool _hasCollideAnyObjectBall = false;

    [Tooltip("最小値の的球に衝突したかどうかのフラグ")]        //ターン開始時に
    bool _hasCollideMinObjectBall = false;
    
    [SerializeField] FoulProcess _foulProcess = default;

    public int MinObjectBallNumber => _minObjectBallNumber;
    public bool HasCollideMinObjectBall => _hasCollideMinObjectBall;



    [ContextMenu("削除①")]
    void delate1()      //Test用
    {
        RemoveObjectBallNum(1);
    }
    [ContextMenu("削除②")]
    void delate2()      //Test用
    {
        RemoveObjectBallNum(2);
    }
    [ContextMenu("削除③")]
    void delate3()      //Test用
    {
        RemoveObjectBallNum(3);
    }
    [ContextMenu("削除④")]
    void delete4()      //Test用
    {
        RemoveObjectBallNum(4);
    }

    [ContextMenu("最小的球")]
    void test2()        //Test用
    {
        CheckMinObjectBallNum();
        GameDebug.Log($"最小値の的球は {_minObjectBallNumber} です。");
    }

    [ContextMenu("現在の的球")]
    void test3()        //Test用
    {
        GameDebug.Log($"現在の的球は {string.Join(", ", _existObjectBallNumber)} です。");
    }

    /// <summary> 手球が的球に衝突した時の処理を行うメソッド。最小値の的球に衝突する前に、他の的球に衝突した場合はファール処理を行う。 </summary>
    /// <param name="beBumpedBallNumber"> 衝突した的球の番号 </param>
    public void CollideCueBall(int beBumpedBallNumber)
    {
        _hasCollideAnyObjectBall = true;

        if (beBumpedBallNumber == _minObjectBallNumber && !_hasCollideMinObjectBall)     //最小値の的球に初めて衝突した場合
        {
            _hasCollideMinObjectBall = true;                                            //↑フラグを立てる
            GameDebug.Log($"手球が最小値の的球 {_minObjectBallNumber} に衝突しました。");
        }
        else if (beBumpedBallNumber != _minObjectBallNumber && !_hasCollideMinObjectBall) //最小値の的球に衝突する前に、他の的球に衝突した場合
        {
            _foulProcess.Foul();                                                         //ファール
        }
    }

    /// <summary> 存在している的球の中の最小値を求めるメソッド。shotフェーズ開始時か、ショット時に呼び出したい </summary>
    public void CheckMinObjectBallNum()
    {
        _minObjectBallNumber = _existObjectBallNumber.Min();
    }

    /// <summary> ポケットに落ちた的球の番号を配列から削除するメソッド </summary>
    /// <param name="pocketedBallNumber"> ポケットに落ちた的球の番号 </param>
    public void RemoveObjectBallNum(int pocketedBallNumber)
    {
        _existObjectBallNumber.Remove(pocketedBallNumber);
    }

    /// <summary> 最小値の的球に衝突したかどうかのフラグをリセットするメソッド </summary>
    public void ResetCollideObjectBallFlag()
    {
        _hasCollideMinObjectBall = false;
        _hasCollideAnyObjectBall = false;
    }

    [ContextMenu("どの的球にも衝突しなかった？")]
    public void NotifyNoCollide()
    {
        if (!_hasCollideAnyObjectBall)
        {
            _foulProcess.Foul();
        }
        
    }
}
