using PlayEveryWare.EpicOnlineServices;
using UnityEngine;

/// <summary>
/// セッション一覧を扱うコンポーネント
/// </summary>
public class FishNetRoomListController : MonoBehaviour
{
    public void TestInitialize()
    {
        InitializeAttribute("NewRoom");
    }

    /// <summary>
    /// セッションに各種データを埋め込む
    /// </summary>
    public void InitializeAttribute(string sessionName)
    {
        //部屋立て前に、部屋にデータを入れる器を用意する
        //完了したら、FishNetRoomAttributeManager.AttributeInitializeState が Done になる
        //TODO: セッション情報の更新完了後に部屋立ての処理を行うようにする
        FishNetRoomAttributeManager.GetAttributeStack()
        .AddStringAttribute(AttributeTypes.HostPUID, EOSManager.Instance.GetProductUserId().ToString())
        .AddStringAttribute(AttributeTypes.RoomName, sessionName)
        .SetAttributes();
    }

    /// <summary>
    /// 部屋を検索する
    /// </summary>
    public void SearchRooms()
    {
        //部屋を検索し、検索結果は FishNetRoomSearchManager.FoundSessions に保存される
        FishNetRoomSearchManager.SearchSession();
    }

    /// <summary>
    /// 部屋名を読み取る
    /// </summary>
    public void GetRoomName()
    {
        //テストとして、検索結果0番の部屋名を読み取る
        //文字列のため、string型用のメソッドで読み取る
        Debug.Log($"RoomName: {FishNetRoomAttributeManager.ReadStringAttribute(0, AttributeTypes.RoomName)}");
    }
}
