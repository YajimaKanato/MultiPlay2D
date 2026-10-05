using System.Collections.Generic;
public class PlayerRegistry
{
    /// <summary>
    /// PlayerIDからプレイヤーの情報を検索するためのDictionary
    /// </summary>
    private readonly Dictionary<PlayerInfo.PlayerID, PlayerInfo> _playerRegistry = new();

    /// <summary>
    /// プレイヤーの情報と登録するメソッド
    /// </summary>
    /// <param name="playerID">プレイヤーID</param>
    /// <param name="playerInfo">プレイヤーの情報</param>
    public void RegisterPlayer(PlayerInfo.PlayerID playerID, PlayerInfo playerInfo)
    {
        _playerRegistry.Add(playerID, playerInfo);
    }
}
