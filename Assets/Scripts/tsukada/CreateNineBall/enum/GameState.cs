using UnityEngine;

public enum GameState
{
    /// <summary> ゲーム開始前の準備フェーズ </summary>
    [InspectorName("準備フェーズ")]
    ReadyFase,

    /// <summary> ブレイクショットのフェーズ </summary>
    [InspectorName("ブレイクショットフェーズ")]
    BreakeShotFase,

    /// <summary> ドラフトのフェーズ。ドラフトエフェクトの選択と使用の両方を含む </summary>
    [InspectorName("ドラフトフェーズ")]
    DraftFase,

    /// <summary> ショットのフェーズ。ショットの実行と結果の確認を含む </summary>
    [InspectorName("ショットフェーズ")]
    ShotFase,

    /// <summary> ファールのフェーズ。ファール判定後にファール処理を実行する </summary>
    [InspectorName("ファールフェーズ")]
    FoulFase,

    /// <summary> ゲーム終了後のフェーズ。試合結果の確認や、「もう一度遊ぶ、ホームに戻る」などの選択を含む </summary>
    [InspectorName("リザルトフェーズ")]
    ResultFase,
}