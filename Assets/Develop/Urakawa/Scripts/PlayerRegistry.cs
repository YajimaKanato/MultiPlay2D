using System.Collections.Generic;

/// <summary>
/// プレイヤー情報を保持するクラス
/// プレイヤーIDを指定して操作する
/// </summary>
public class PlayerRegistry
{
    /// <summary>
    /// プレイヤーIDからプレイヤー情報を検索するためのDictionary
    /// </summary>
    private readonly Dictionary<PlayerID, PlayerInfo> _playerRegistry = new();

    /// <summary>
    /// プレイヤー情報を登録するメソッド
    /// </summary>
    /// <param name="playerInfo">登録するプレイヤー情報</param>
    public void RegisterPlayerInfo(PlayerInfo playerInfo)
    {
        _playerRegistry.Add(playerInfo.Player_ID, playerInfo);
    }

    /// <summary>
    /// 指定したプレイヤーIDに対応するプレイヤー情報を取得するメソッド
    /// </summary>
    /// <param name="playerId">取得対象のプレイヤーID</param>
    /// <returns>プレイヤーIDに対応したプレイヤー情報</returns>
    public PlayerInfo TryGetPlayerInfo(PlayerID playerId)
    {
        if (_playerRegistry.TryGetValue(playerId, out var foundPlayerInfo))
        {
            GameDebug.Log("プレイヤー情報の取得成功");
            return foundPlayerInfo;
        }

        throw new KeyNotFoundException($"プレイヤー情報が見つかりません。プレイヤーID：{ playerId.ID }");
    }

    /// <summary>
    /// プレイヤー情報を削除するメソッド
    /// </summary>
    /// <param name="playerId">削除対象のプレイヤーID</param>
    public void DeletePlayerInfo(PlayerID playerId)
    {
        _playerRegistry.Remove(playerId);
    }
}
