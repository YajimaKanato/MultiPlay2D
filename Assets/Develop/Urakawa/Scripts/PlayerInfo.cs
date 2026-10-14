/// <summary>
/// プレイヤー情報を持つ構造体
/// </summary>
public readonly struct PlayerInfo
{
    /// <summary>
    /// プレイヤーIDを外部から取得するためのプロパティ
    /// </summary>
    public readonly PlayerID Player_ID;

    /// <summary>
    /// プレイヤー名を外部から取得するためのプロパティ
    /// </summary>
    public readonly PlayerName Player_Name;

    /// <summary>
    /// プレイヤーが何Pかを外部から取得するためのプロパティ
    /// </summary>
    public readonly PlayerSlot Player_Slot;

    /// <summary>
    /// プレイヤー情報のコンストラクタ
    /// </summary>
    /// <param name="playerId">プレイヤーID</param>
    /// <param name="playerName">プレイヤー名</param>
    /// <param name="playerSlot">何P</param>
    public PlayerInfo(PlayerID playerId, PlayerName playerName, PlayerSlot playerSlot)
    {
        Player_ID = playerId;
        Player_Name = playerName;
        Player_Slot = playerSlot;
    }
}
