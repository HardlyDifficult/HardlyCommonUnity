using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace HD
{
  public static class QuaternionExtensions
  {
    public static bool IsValid(
      this Quaternion quaternion)
    {
      var result = Math.Abs(1 - (quaternion.x * quaternion.x + quaternion.y * quaternion.y + quaternion.z * quaternion.z + quaternion.w * quaternion.w)) <= .000011;

      Debug.Assert(result);

      return result;
    }

    public static Quaternion Normalize(
      this Quaternion q)
    {
      Quaternion result;
      float sq = q.x * q.x;
      sq += q.y * q.y;
      sq += q.z * q.z;
      sq += q.w * q.w;

      float inv = 1.0f / (float)Math.Sqrt(sq);
      result.x = q.x * inv;
      result.y = q.y * inv;
      result.z = q.z * inv;
      result.w = q.w * inv;
      return result;
    }
  }
}
