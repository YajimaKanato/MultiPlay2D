using UnityEngine;

public static class DebugLogger
{
    public static void Log(string message, Object context = null)
    {
#if UNITY_EDITOR
        Debug.Log(message, context);
#endif
    }

    public static void LogWarning(string message, Object context = null)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message, context);
#endif
    }

    public static void LogError(string message, Object context = null)
    {
#if UNITY_EDITOR
        Debug.LogError(message, context);
#endif
    }
}
