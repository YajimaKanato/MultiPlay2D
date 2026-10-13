/// <summary>
/// プレイヤーIDを作成するクラス
/// </summary>
public class PlayerIDGenerator
{
    /// <summary>
    /// 次に作成するプレイヤーID
    /// </summary>
    private uint _nextPlayerId = 1;

    /// <summary>
    /// プレイヤーIDを1から順番に作成するメソッド
    /// </summary>
    /// <returns>作成したプレイヤーID</returns>
    public PlayerID CreatePlayerID()
    {
        var playerId = new PlayerID(_nextPlayerId);

        // uintの扱える範囲を超えたときにエラーを出す
        checked
        {
            _nextPlayerId++; // 次のプレイヤーIDを準備する
        }

        return playerId;
    }
}
