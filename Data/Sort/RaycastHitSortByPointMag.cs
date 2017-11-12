using System;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
  public class RaycastHitSortByPointMag : IComparer<RaycastHit>
  {
    readonly Vector3 fromPosition;

    public RaycastHitSortByPointMag(
      Vector3 fromPosition)
    {
      this.fromPosition = fromPosition;
    }

    public int Compare(
      RaycastHit x, 
      RaycastHit y)
    {
      return (x.point - fromPosition).sqrMagnitude.CompareTo((y.point - fromPosition).sqrMagnitude);
    }
  }
}
