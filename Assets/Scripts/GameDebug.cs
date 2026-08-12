using System.Diagnostics;
using UnityEngine;

public static class GameDebug
{
    // Editor / Development Buildでのみ呼び出される
    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    public static void Log(object message, Object context = null)
    {
        UnityEngine.Debug.Log(message, context);
    }

    public static void LogWarning(object message, Object context = null)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }

    public static void LogError(object message, Object context = null)
    {
        UnityEngine.Debug.LogError(message, context);
    }
}
