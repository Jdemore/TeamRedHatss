/*
 * Console - Strippable logging utility.
 *
 * All methods are compiled out of non-development release builds via
 * [Conditional] attributes. Supports optional category tags for filtering
 * in the Unity Console.
 *
 * Usage:
 *   Console.Log("Player spawned");
 *   Console.Log("Damage dealt: 50", "Combat");
 *   Console.Warn("Low memory", context: this);
 */

using UnityEngine;
using System.Diagnostics;

public static class Console
{
    /// <summary>Log an informational message (stripped in release builds).</summary>
    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    public static void Log(object message, Object context = null)
    {
        UnityEngine.Debug.Log(message, context);
    }

    /// <summary>Log with a category prefix for easy filtering.</summary>
    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    public static void Log(object message, string category, Object context = null)
    {
        UnityEngine.Debug.Log(FormatCategory(category, message), context);
    }

    /// <summary>Log a warning message (stripped in release builds).</summary>
    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    public static void Warn(object message, Object context = null)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }

    /// <summary>Warn with a category prefix.</summary>
    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    public static void Warn(object message, string category, Object context = null)
    {
        UnityEngine.Debug.LogWarning(FormatCategory(category, message), context);
    }

    /// <summary>Log an error message (stripped in release builds).</summary>
    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    public static void Err(object message, Object context = null)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    /// <summary>Error with a category prefix.</summary>
    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    public static void Err(object message, string category, Object context = null)
    {
        UnityEngine.Debug.LogError(FormatCategory(category, message), context);
    }

    /// <summary>Log an error if condition is false (stripped in release).</summary>
    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    public static void Assert(bool condition, object message, Object context = null)
    {
        if (!condition)
            UnityEngine.Debug.LogError($"[ASSERT] {message}", context);
    }

    private static string FormatCategory(string category, object message)
    {
        return $"[{category}] {message}";
    }
}