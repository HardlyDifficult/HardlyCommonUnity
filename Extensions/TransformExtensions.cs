using System;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
  public static class TransformExtensions
  {
    public static Transform[] GetAllChildsWithName(
      this Transform transform, 
      string name)
    {
      List<Transform> collection = new List<Transform>();

      for (int i = 0; i < transform.childCount; i++)
      {
        if (transform.GetChild(i).name == name)
        {
          collection.Add(transform.GetChild(i));
        }
      }

      return collection.ToArray();
    }
  }
}