using UnityEngine;

namespace HD
{
  public static class VectorExtensions
  {
    public static Vector3 GetX0Z(
      this Vector3 vector)
    {
      return new Vector3(vector.x, 0, vector.z);
    }
  }
}
