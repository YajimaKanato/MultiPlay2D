/// <summary>
/// プレイヤーの情報を持つ構造体
/// </summary>
public readonly struct PlayerInfo
{
    public readonly struct PlayerID
    {
        public uint Value { get; }
        public PlayerID(uint value)
        {
            Value = value;
        }
    }

    public readonly struct PlayerName
    {
        public string Value { get; }
        public PlayerName(string value)
        {
            Value = value;
        }
    }
}
