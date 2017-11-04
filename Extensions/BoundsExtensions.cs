using System;
using UnityEngine;

namespace HD
{
  public static class BoundsExtensions
  {
    public static Vector3 RandomPosition(
      this Bounds bounds,
      float radius)
    {
      Vector3 randomPosition = new Vector3(
       UnityEngine.Random.Range(bounds.min.x + radius, bounds.max.x - radius),
       0,
       UnityEngine.Random.Range(bounds.min.z + radius, bounds.max.z - radius));

      return randomPosition;
    }
  }
}
