using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// MOVED to Common
/// 
/// 
/// TODO add a method that does string format - e.g. assert(ifTrue, message, params)
/// </summary>
public class Debug
{
  #region Write
  [Conditional("DEBUG")]
  public static void Assert(
    bool isTrue,
    object message = null,
    [CallerMemberName] string memberName = "",
    [CallerFilePath] string sourceFilePath = "",
    [CallerLineNumber] int sourceLineNumber = 0)
  {
    if (isTrue == false)
    {
      string error = message?.ToString() ?? "assert failed";

      throw new Exception($"{error} from {memberName} @ {GetFileName(sourceFilePath)}:{sourceLineNumber}");
    }
  }

  private static string GetFileName(string sourceFilePath)
  {
    int indexOfStartOfName = sourceFilePath.LastIndexOf("\\");
    string fileName;
    if (indexOfStartOfName >= 0)
    {
      fileName = sourceFilePath.Substring(indexOfStartOfName + 1);
    }
    else
    {
      fileName = sourceFilePath;
    }

    return fileName;
  }

  // TODO don't use params
  [Conditional("DEBUG")]
  public static void Assert(
    bool isTrue,
    string message,
    params object[] formatParams)
  {
    if (isTrue == false)
    {
      throw new Exception(string.Format(message, formatParams));
    }
  }

  [Conditional("DEBUG")]
  public static void Fail(
    string message = null)
  {
    Assert(false, message);
  }

  [Conditional("DEBUG")]
  public static void TODO()
  {
    //TODO UnityEngine.Debug.LogWarning("TODO!");
  }

  [Conditional("DEBUG")]
  public static void LogIf(
    object message,
    bool condition)
  {
    if (condition)
    {
      Log(message);
    }
  }

  [Conditional("DEBUG")]
  public static void Log(
    object message,
    [CallerMemberName] string memberName = "",
    [CallerFilePath] string sourceFilePath = "",
    [CallerLineNumber] int sourceLineNumber = 0)
  {
    UnityEngine.Debug.Log($"{message} from {memberName} @ {GetFileName(sourceFilePath)}:{sourceLineNumber}");
  }

  [Conditional("DEBUG")]
  public static void LogError(
    object message)
  {
    UnityEngine.Debug.LogError(message);
  }

  [Conditional("DEBUG")]
  public static void DrawX(
    Vector3 position,
    float lineLength = .2f,
    Color color = new Color(),
    float duration = 0)
  {
    if (color == default(Color))
    {
      color = Color.white;
    }
    float rng = UnityEngine.Random.Range(-2f, 2f);
    DrawLine(position, lineLength, Quaternion.Euler(0, 45 + rng, 0), color, duration);
    DrawLine(position, lineLength, Quaternion.Euler(0, -45 + rng, 0), color, duration);
    DrawLine(position, lineLength, Quaternion.Euler(90, 45 + rng, 0), color, duration);
    DrawLine(position, lineLength, Quaternion.Euler(90, -45 + rng, 0), color, duration);
  }

  [Conditional("DEBUG")]
  public static void DrawLine(
    Vector3 position,
    float lineLength,
    Quaternion rotation,
    Color color,
    float duration = 0)
  {
    Vector3 lineVector = new Vector3(0, 0, lineLength);
    Vector3 start = position - rotation * lineVector;
    Vector3 end = position + rotation * lineVector;

    UnityEngine.Debug.DrawLine(start, end, color, duration);
  }

  [Conditional("DEBUG")]
  public static void DrawLine(
  Vector3 start,
  Vector3 end,
  Color color = default(Color),
  float duration = 0)
  {
    if (color == default(Color))
    {
      color = Color.white;
    }

    for (int i = 0; i < 5; i++)
    {
      UnityEngine.Debug.DrawLine(start, end, color, duration);
    }
  }

  public static void LogWarning(
    object message)
  {
    UnityEngine.Debug.LogWarning(message);
  }

  public static void DrawGrid(
    Vector3 center,
    float gridSize,
    float radius)
  {
    float y = center.y;
    float fromZ = center.z - radius;
    float toZ = center.z + radius;
    for (float x = center.x - radius; x < center.x + radius; x += gridSize)
    {
      DrawLine(new Vector3(x, y, fromZ), new Vector3(x, y, toZ));
    }
    float fromX = center.x - radius;
    float toX = center.x + radius;
    for (float z = center.z - radius; z < center.z + radius; z += gridSize)
    {
      DrawLine(new Vector3(fromX, y, z), new Vector3(toX, y, z));
    }
  }

  #endregion
}