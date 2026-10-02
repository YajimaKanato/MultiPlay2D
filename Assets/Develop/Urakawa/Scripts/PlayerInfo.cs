using System;

/// <summary>
/// プレイヤーの情報を持つ構造体
/// </summary>
public readonly struct PlayerInfo : IEquatable<PlayerInfo>
{
    public PlayerName Name { get; }
    public PlayerId Id { get; }
    public PlayerSlot Slot { get; }

    /// <summary>
    /// PlayerInfoのコンストラクタ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="id"></param>
    /// <param name="slot"></param>
    public PlayerInfo(PlayerName name, PlayerId id, PlayerSlot slot)
    {
        Name = name;
        Id = id;
        Slot = slot;
    }

    public bool Equals(PlayerInfo other)
    {
        return Name.Equals(other.Name) && Id.Equals(other.Id) && Slot == other.Slot;
    }

    public override bool Equals(object obj)
    {
        return obj is PlayerInfo other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + Name.GetHashCode();
            hash = hash * 31 + Id.GetHashCode();
            hash = hash * 31 + Slot.GetHashCode();
            return hash;
        }
    }

    /// <summary>
    /// プレイヤー名
    /// </summary>
    public readonly struct PlayerName : IEquatable<PlayerName>
    {
        public readonly string _playerName;
        public PlayerName(string playerName)
        {
            _playerName = playerName;
        }
        public bool Equals(PlayerName other)
        {
            return _playerName == other._playerName;
        }
        public override bool Equals(object obj)
        {
            return obj is PlayerName other && Equals(other);
        }
        public override int GetHashCode()
        {
            return _playerName?.GetHashCode() ?? 0;
        }
    }

    public static bool operator ==(PlayerInfo left, PlayerInfo right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PlayerInfo left, PlayerInfo right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// 何番目のプレイヤーかを示す
    /// </summary>
    public enum PlayerSlot
    {
        Player1,
        Player2,
        Player3,
        Player4
    }

    public readonly struct PlayerId : IEquatable<PlayerId>
    {
        public readonly int _playerID;

        public PlayerId(int playerID)
        {
            _playerID = playerID;
        }

        public bool Equals(PlayerId other)
        {
            return _playerID == other._playerID;
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _playerID.GetHashCode();
        }
    }
}
