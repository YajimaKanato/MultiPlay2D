using System.Collections.Generic;
using UnityEngine;

public class BallTracker : MonoBehaviour
{
    [Header("球リスト")]
    [SerializeField] private List<GameObject> _ballList = new List<GameObject>();

    private void OnEnable()
    {
        PocketDetector.OnBallPocketed += HandleBallPocketed;
    }

    private void OnDisable()
    {
        PocketDetector.OnBallPocketed -= HandleBallPocketed;
    }

    /// <summary>
    /// 落下したボールをConsole上で表示する処理
    /// </summary>
    /// <param name="ball">落下した球</param>
    private void HandleBallPocketed(GameObject ball)
    {
        if (_ballList.Contains(ball))
        {
            GameDebug.Log($"{ball.name} が落下");
        }
    }
}
