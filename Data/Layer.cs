using System;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
  [Serializable]
  public struct Layer
  {
    [SerializeField]
    int layerIndex;

    public Layer(
      int layerIndex)
    {
      this.layerIndex = layerIndex;
    }

    public int mask
    {
      get
      {
        return 1 << layerIndex;
      }
    }

    public static implicit operator int(
      Layer layer)
    {
      return layer.layerIndex;
    } 
  }
}
