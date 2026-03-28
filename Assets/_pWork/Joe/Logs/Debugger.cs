/*
 * Debugger - Abstract MonoBehaviour with per-instance toggleable logging.
 *
 * Inherit from this instead of MonoBehaviour when you want convenient,
 * strippable, per-component debug logging with an Inspector toggle.
 *
 * Usage:
 *   public class EnemyAI : Debugger
 *   {
 *       protected override string LogCategory => "AI";
 *
 *       private void Start()
 *       {
 *           Log("EnemyAI initialized");
 *       }
 *   }
 */

using UnityEngine;
using System.Diagnostics;

public abstract class Debugger : MonoBehaviour
{
    [Header("Debug Settings")]
    [Tooltip("Enable to print debug messages from this component.")]
    [SerializeField] private bool _enableLogs;

    /// <summary>
    /// Override to provide a log category tag (e.g. "AI", "Combat", "UI").
    /// Defaults to the class name.
    /// </summary>
    protected virtual string LogCategory => GetType().Name;


    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    protected void Log(object message)
    {
        if (!_enableLogs) return;
        Console.Log(message, LogCategory, this);
    }


    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    protected void Warn(object message)
    {
        if (!_enableLogs) return;
        Console.Warn(message, LogCategory, this);
    }


    [Conditional("UNITY_EDITOR")]
    [Conditional("DEVELOPMENT_BUILD")]
    protected void Err(object message)
    {
        if (!_enableLogs) return;
        Console.Err(message, LogCategory, this);
    }
}