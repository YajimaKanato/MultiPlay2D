using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine.Events;

/// <summary>
/// 部屋検索結果を送るUnityEvent
/// </summary>
[Serializable]
public class RoomSearchedEvent : UnityEvent<List<SessionInfo>>{}