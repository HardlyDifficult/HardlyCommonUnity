using UnityEngine;

namespace HD
{
  public static class ResolutionExtensions
  {
    public static string ToStringWithoutHz(
      this Resolution resolution)
    {
      return $"{resolution.width} x {resolution.height}";
    }
  }
}