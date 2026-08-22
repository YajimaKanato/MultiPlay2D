using UnityEngine;
using System.Collections.Generic;
using Fusion;

/// <summary>
/// 部屋選択ウィンドウを構築するコンポーネント
/// </summary>
public class RoomSelectingWindowBuilder : MonoBehaviour
{
    [SerializeField]
    private UIRoomSelectButton _roomSelectButtonPrefab;
    [SerializeField]
    private StartRoomEvent _joinRoomEvent;
    [SerializeField]
    private float _buttonUnderPadding;

    void Awake()
    {
        Destroy(transform.GetChild(0).gameObject);

        if(_roomSelectButtonPrefab == null)
        {
            GameDebug.LogError("ボタンプレハブがアサインされていません。");
        }
    }

    /// <summary>
    /// ボタンを配置してウィンドウを構築する
    /// </summary>
    /// <param name="searchedRooms">検索で見つかった部屋のリスト</param>
    public void Build(List<SessionInfo> searchedRooms)
    {
        for(int i = 0; i < searchedRooms.Count; i++)
        {
            var roomSelectButton = Instantiate(_roomSelectButtonPrefab);
            roomSelectButton.AssignRoom(searchedRooms[i], _joinRoomEvent);
            roomSelectButton.transform.SetParent(transform);
            roomSelectButton.transform.localScale = _roomSelectButtonPrefab.transform.localScale;
        }
    }
}
