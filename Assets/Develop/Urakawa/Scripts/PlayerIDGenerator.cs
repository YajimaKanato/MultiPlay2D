/// <summary>
/// プレイヤーIDを作成するクラス
/// 参加するプレイヤーを管理する場所で一度インスタンス化する
/// </summary>
public class PlayerIDGenerator
{
    /// <summary>
    /// 次に作成するPlayerID
    /// </summary>
    private uint _nextPlayerID = 1;

    /// <summary>
    /// PlayerIDを1から順番に作成するメソッド
    /// </summary>
    /// <returns>作成したPlayerID</returns>
    public PlayerInfo.PlayerID CreatePlayerID()
    {
        var playerID = new PlayerInfo.PlayerID(_nextPlayerID);

        _nextPlayerID++; // 次のPlayerIDを準備する

        return playerID;
    }
}
