/// <summary>
/// プレイヤーIDを作成するクラス
/// </summary>
public sealed class PlayerIDGenerator
{
    /// <summary>
    /// 唯一のインスタンスを保持する静的フィールド
    /// </summary>
    private static readonly PlayerIDGenerator _instance = new PlayerIDGenerator();

    /// <summary>
    /// 唯一のインスタンスを取得するプロパティ
    /// </summary>
    public static PlayerIDGenerator Instance => _instance;

    /// <summary>
    /// lock専用の非公開オブジェクト
    /// </summary>
    private readonly object _lock = new object();

    /// <summary>
    /// 次に作成するプレイヤーID
    /// </summary>
    private uint _nextPlayerId = 1;

    /// <summary>
    /// privateのコンストラクタ
    /// </summary>
    private PlayerIDGenerator() { }

    /// <summary>
    /// プレイヤーIDを1から順番に作成するメソッド
    /// </summary>
    /// <returns>作成したプレイヤーID</returns>
    public PlayerID CreatePlayerID()
    {
        // スレッドセーフを実現
        lock (_lock)
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
}
