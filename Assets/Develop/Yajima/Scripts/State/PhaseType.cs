using System;
using UnityEngine;

namespace Yajima
{
    /// <summary>フェーズの種類</summary>
    public enum PhaseType : Byte
    {
        [InspectorName("ブレイクショット")] BreakShotPhase,
        [InspectorName("ドラフト")] DraftPhase,
        [InspectorName("ショット")] ShotPhase,
        [InspectorName("ショット後")] AfterShotPhase,
        [InspectorName("リザルト")] ResultPhase,
    }
}
