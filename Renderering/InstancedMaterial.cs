using System;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
  /// <summary>
  /// Manages a single material and its property block.
  /// 
  /// Goal of this class, you do not need to know property blocks are a thing.
  /// </summary>
  public class InstancedMaterial
  {
    #region Data
    public readonly Renderer renderer;
    readonly MaterialPropertyBlock materialPropertyBlock;
    readonly Material material;
    #endregion

    #region Properties
    /// <summary>
    /// TODO we have an issue. Changing the material is saved when you end the game.
    /// Either need to revert(is it possible to do this reliably?) or use instanced or find how to put in the block.
    /// 
    /// Orrrr.... use a shader variable instead of a different shader.
    /// 
    /// 
    /// snsty: you can append a material to any renderer that is "selected" and have that material use a stencil shader, then use that in a post-processing effect to do the outline
    /// ^^^^^^^^^^^^^^^^^^^^^^^
    /// Maybe it's a companion component on each renderer.  If not there, then simply hide that renderer on ghost.
    /// </summary>
    public Shader shader
    {
      get
      {
        return material.shader;
      }
      set
      {
        material.shader = value;
      }
    }
    #endregion

    public InstancedMaterial(
      GameObject gameObject)
    {
      Debug.Assert(gameObject != null);

      renderer = gameObject.GetComponent<Renderer>();
      material = renderer.sharedMaterial;

      Debug.Assert(renderer != null);
      Debug.Assert(material != null);

      materialPropertyBlock = new MaterialPropertyBlock();
    }

    public InstancedMaterial(
      Renderer renderer,
      Material material)
    {
      Debug.Assert(renderer != null);
      Debug.Assert(material != null);

      this.renderer = renderer;
      this.material = material;
      materialPropertyBlock = new MaterialPropertyBlock();
    }

    public void SetColor(
      string propertyName,
      Color color,
      bool isInstanced = true)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetColor(propertyId, color, isInstanced);
    }

    public void SetColor(
      int propertyId,
      Color color,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetColor(propertyId, color);
        renderer.SetPropertyBlock(materialPropertyBlock);
      }
      else
      {
        material.SetColor(propertyId, color);
      }
    }
  }
}
