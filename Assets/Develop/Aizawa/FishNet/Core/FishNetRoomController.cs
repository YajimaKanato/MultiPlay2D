using UnityEngine;
using TMPro;

/// <summary>
/// FishNetによるネットワーク接続を実行させるコンポーネント
/// </summary>
public class FishNetRoomController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _roomNameInputField;
    [SerializeField]
    private StartRoomEvent _joinRoomEvent;

    /// <summary>
    /// クライアントとして部屋に合流する
    /// </summary>
    public void JoinRoom()
    {
        if(string.IsNullOrEmpty(_roomNameInputField.text))
        {
            GameDebug.LogError("部屋名が空欄です。");
            return;
        }

        _joinRoomEvent?.Invoke(_roomNameInputField.text);
    }
}
