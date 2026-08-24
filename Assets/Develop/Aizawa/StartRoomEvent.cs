using System;
using UnityEngine.Events;

/// <summary>
/// 立てる/合流する部屋の名前を送るUnityEvent
/// </summary>
[Serializable]
public class StartRoomEvent : UnityEvent<string>{}