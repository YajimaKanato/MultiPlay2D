using UnityEngine;
using TMPro;
using Fusion;

/// <summary>
/// 部屋選択ボタンのコンポーネント
/// </summary>
public class UIRoomSelectButton : MonoBehaviour
{
    private StartRoomEvent _joinRoomEvent;

    private string _roomName;

    void Start()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _roomName;
    }

    /// <summary>
    /// 部屋合流時の処理
    /// </summary>
    public void JoinRoom()
    {
        _joinRoomEvent?.Invoke(_roomName);
    }

    /// <summary>
    /// コンポーネントに部屋を割り当てる
    /// </summary>
    /// <param name="session">割り当てる部屋情報</param>
    public void AssignRoom(SessionInfo session, StartRoomEvent joinEvent)
    {
        _roomName = session.Name;
        _joinRoomEvent = joinEvent;
    }
}
