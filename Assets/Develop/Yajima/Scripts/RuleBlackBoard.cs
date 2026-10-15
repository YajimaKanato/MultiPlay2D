using System;
using UnityEngine;

namespace Yajima
{
    /// <summary>盤面全体のパラメータを持つクラス</summary>
    [Serializable]
    public sealed class RuleBlackBoard
    {
        [Header("時間")]
        [Tooltip("ブレイクショットの制限時間")] public int BreakShotLimitTime = 30;
        [Tooltip("ドラフトの制限時間")] public int DraftLimitTime = 30;
        [Tooltip("ショットの制限時間")] public int ShotLimitTime = 30;

        [Header("ルール")]
        [Tooltip("ショットしたか")] public bool IsShot = false;
        [Tooltip("ターゲットに初めに触れたか")] public bool TouchFirstTargetBall = true;
        [Tooltip("全ての球が止まったか")] public bool IsAllBallStop = false;
        [Tooltip("ゲームに勝利する条件を満たしたか")] public bool IsWinCondition = false;
    }
}