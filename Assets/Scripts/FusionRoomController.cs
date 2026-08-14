using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class FusionRoomController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _roomCodeField;
    [SerializeField]
    private StartRoomEvent _createRoomEvent;
    [SerializeField]
    private StartRoomEvent _joinRoomEvent;

    public void CreateRoom()
    {
        if(!string.IsNullOrEmpty(_roomCodeField.text))
        {
            _createRoomEvent?.Invoke(_roomCodeField.text);
        }
        else
        {
            GameDebug.LogError($"ルーム名が不正です: {_roomCodeField.text}");
        }
    }

    public void JoinRoom()
    {
        if(!string.IsNullOrEmpty(_roomCodeField.text))
        {
            _joinRoomEvent?.Invoke(_roomCodeField.text);
        }
        else
        {
            GameDebug.LogError($"ルーム名が不正です: {_roomCodeField.text}");
        }
    }

    [Serializable]
    private class StartRoomEvent : UnityEvent<string>{}
}
