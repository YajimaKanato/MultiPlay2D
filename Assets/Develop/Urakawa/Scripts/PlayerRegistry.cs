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
    /// <returns>プレイヤー情報の登録に対する成否</returns>
    public bool TryRegisterPlayerInfo(PlayerInfo playerInfo)
    {
        return _playerRegistry.TryAdd(playerInfo.Player_ID, playerInfo);
    }

    /// <summary>
    /// 指定したプレイヤーIDに対応するプレイヤー情報を取得するメソッド
    /// </summary>
    /// <param name="playerId">取得対象のプレイヤーID</param>
    /// <returns>プレイヤーIDに対応したプレイヤー情報の取得に対する成否</returns>
    public bool TryGetPlayerInfo(PlayerID playerId, out PlayerInfo foundPlayerInfo)
    {
        if (_playerRegistry.TryGetValue(playerId, out foundPlayerInfo))
        {
            GameDebug.Log("プレイヤー情報の取得成功");
            return true;
        }

        GameDebug.LogWarning($"プレイヤー情報が見つかりませんでした。対象のプレイヤーID：{playerId}");
        return false;

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
