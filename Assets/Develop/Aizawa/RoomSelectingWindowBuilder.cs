using UnityEngine;
using System.Collections.Generic;
using Fusion;

/// <summary>
/// 部屋選択ウィンドウを構築するコンポーネント
/// </summary>
public class RoomSelectingWindowBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject _roomSelectButtonPrefab;
    [SerializeField]
    private StartRoomEvent _joinRoomEvent;
    [SerializeField]
    private float _buttonUnderPadding;

    void Start()
    {
        Destroy(transform.GetChild(0).gameObject);
    }

    /// <summary>
    /// ボタンを配置してウィンドウを構築する
    /// </summary>
    /// <param name="searchedRooms">検索で見つかった部屋のリスト</param>
    public void Build(List<SessionInfo> searchedRooms)
    {
        for(int i = 0; i < searchedRooms.Count; i++)
        {
            var roomSelectButton = Instantiate(_roomSelectButtonPrefab).GetComponent<UIRoomSelectButton>();
            roomSelectButton.AssignRoom(searchedRooms[i], _joinRoomEvent);
            roomSelectButton.transform.SetParent(transform);
            ((RectTransform)roomSelectButton.transform).anchoredPosition = Vector3.down * (i * _buttonUnderPadding);
            roomSelectButton.transform.localScale = _roomSelectButtonPrefab.transform.localScale;
        }
    }
}
