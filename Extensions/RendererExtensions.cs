using System;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
  public static class RendererExtensions
  {
    public static List<InstancedMaterial> GetAllMaterials(
      this Renderer[] rendererList)
    {
      List<InstancedMaterial> materialList = new List<InstancedMaterial>();
      for (int rendererIndex = 0; rendererIndex < rendererList.Length; rendererIndex++)
      {
        Renderer renderer = rendererList[rendererIndex];
        Material[] sharedMaterials = renderer.sharedMaterials;
        for (int materialIndex = 0; materialIndex < sharedMaterials.Length; materialIndex++)
        {
          materialList.Add(new InstancedMaterial(renderer, sharedMaterials[materialIndex]));
        }
      }

      return materialList;
    }
  }
}
