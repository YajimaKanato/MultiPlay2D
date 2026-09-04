using UnityEngine;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// <para>入力された部屋名を元に部屋への接続を行う</para>
/// <para>Photon版</para>
/// </summary>
public class FusionRoomController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _roomCodeField;
    [SerializeField]
    private StartRoomEvent _createRoomEvent;
    [SerializeField]
    private StartRoomEvent _joinRoomEvent;
    [SerializeField]
    private UnityEvent _searchRoomEvent;

    /// <summary>
    /// 部屋立て時の処理
    /// </summary>
    public void CreateRoom()
    {
        _createRoomEvent?.Invoke(_roomCodeField.text);
    }

    /// <summary>
    /// 部屋合流時の処理
    /// </summary>
    public void JoinRoom()
    {
        _joinRoomEvent?.Invoke(_roomCodeField.text);
    }

    /// <summary>
    /// 部屋検索時の処理
    /// </summary>
    public void SearchRoom()
    {
        _searchRoomEvent?.Invoke();
    }
}
