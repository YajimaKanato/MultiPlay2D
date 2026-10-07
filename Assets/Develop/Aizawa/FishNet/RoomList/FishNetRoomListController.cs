using Epic.OnlineServices;
using PlayEveryWare.EpicOnlineServices;
using UnityEngine;

/// <summary>
/// セッション一覧を扱うコンポーネント
/// </summary>
public class FishNetRoomListController : MonoBehaviour
{
    /// <summary>
    /// セッションに各種データを埋め込む
    /// </summary>
    public void SetAttribute()
    {
        FishNetRoomAttributeManager.GetAttributeStack()
        .AddStringAttribute(AttributeTypes.HostPUID, EOSManager.Instance.GetProductUserId().ToString())
        .AddStringAttribute(AttributeTypes.RoomName, "NewRoom")
        .SetAttributes();
    }

    /// <summary>
    /// 部屋を検索する
    /// </summary>
    public void SearchRooms()
    {
        FishNetRoomSearchManager.SearchSession();
    }

    /// <summary>
    /// 部屋名を読み取る
    /// </summary>
    public void GetRoomName()
    {
        Debug.Log($"RoomName: {FishNetRoomAttributeManager.ReadStringAttribute(0,AttributeTypes.RoomName)}");
    }
}
