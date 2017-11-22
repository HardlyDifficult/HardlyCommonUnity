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
    /// <summary>
    /// Do not use renderer.material or .sharedMaterial... use this InstancedMaterial class instead.
    /// </summary>
    public readonly Renderer renderer;

    static readonly int 
      mainColorPropertyId,
      mainTexturePropertyId;

    readonly MaterialPropertyBlock materialPropertyBlock;
    readonly Material material;

    readonly HashSet<int> instancedProperties = new HashSet<int>();
    #endregion

    #region Properties
    // FYI .color is excluded because we cannot know if it is instanced or not

    /// <summary>
    /// True if there is no object specific (i.e. 'instanced') data.
    /// </summary>
    public bool isPropertyBlockEmpty
    {
      get
      {
        return materialPropertyBlock.isEmpty;
      }
    }

    public bool doubleSidedGI
    {
      get
      {
        return material.doubleSidedGI;
      }
      set
      {
        material.doubleSidedGI = value;
      }
    }

    public MaterialGlobalIlluminationFlags globalIlluminationFlags
    {
      get
      {
        return material.globalIlluminationFlags;
      }
      set
      {
        material.globalIlluminationFlags = value;
      }
    }

    public string name
    {
      get
      {
        return material.name;
      }
      set
      {
        material.name = value;
      }
    }

    public int passCount
    {
      get
      {
        return material.passCount;
      }
    }

    public int renderQueue
    {
      get
      {
        return material.renderQueue;
      }
      set
      {
        material.renderQueue = value;
      }
    }

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

    public string[] shaderKeywords
    {
      get
      {
        return material.shaderKeywords;
      }
      set
      {
        material.shaderKeywords = value;
      }
    }

    public Vector2 mainTextureOffset
    {
      get
      {
        return material.mainTextureOffset;
      }
      set
      {
        material.mainTextureOffset = value;
      }
    }

    public Vector2 mainTextureScale
    {
      get
      {
        return material.mainTextureScale;
      }
      set
      {
        material.mainTextureScale = value;
      }
    }
    #endregion

    #region Init
    static InstancedMaterial()
    {
      mainColorPropertyId = Shader.PropertyToID("_Color");
      mainTexturePropertyId = Shader.PropertyToID("_MainTex");
    }

    public static InstancedMaterial GetMaterial(
      Renderer renderer)
    {
      return new InstancedMaterial(renderer);
    }

    public static InstancedMaterial[] GetMaterials(
      Renderer renderer)
    {
      Material[] materialList = renderer.sharedMaterials;
      InstancedMaterial[] instancedMaterialList = new InstancedMaterial[materialList.Length];
      for (int i = 0; i < materialList.Length; i++)
      {
        instancedMaterialList[i] = new InstancedMaterial(renderer, materialList[i]);
      }

      return instancedMaterialList;
    }

    InstancedMaterial(
      Renderer renderer) 
      : this(renderer, renderer.sharedMaterial) { }

    InstancedMaterial(
      Renderer renderer,
      Material material)
    {
      Debug.Assert(renderer != null);
      Debug.Assert(material != null);

      this.renderer = renderer;
      this.material = material;
      materialPropertyBlock = new MaterialPropertyBlock();
    }
    #endregion

    #region Write
    /// <summary>
    /// Clears all object specific (i.e. 'instanced') information.
    /// </summary>
    public void ClearPropertyBlock()
    {
      materialPropertyBlock.Clear();
      instancedProperties.Clear();
    }

    public void CopyPropertiesFromMaterial(
      Material materialToCopyFrom)
    {
      material.CopyPropertiesFromMaterial(materialToCopyFrom);
    }

    public void DisableKeyword(
      string keyword)
    {
      material.DisableKeyword(keyword);
    }

    public void EnableKeyword(
      string keyword)
    {
      material.EnableKeyword(keyword);
    }

    /// <summary>
    /// Takes the lerped Color and Float values from start and end.
    /// </summary>
    public void Lerp(
      Material materialStart,
      Material materialEnd,
      float t)
    {
      material.Lerp(materialStart, materialEnd, t);
    }

    public void SetBuffer(
      string propertyName,
      ComputeBuffer value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetBuffer(propertyId, value, isInstanced);
    }

    public void SetBuffer(
      int propertyId,
      ComputeBuffer value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetBuffer(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
      }
      else
      {
        material.SetBuffer(propertyId, value);
      }
    }

    public void SetColor(
      string propertyName,
      Color color,
      bool isInstanced)
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
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetColor(propertyId, color);
      }
    }

    public void SetMainColor(
      Color color,
      bool isInstanced)
    {
      SetColor(mainColorPropertyId, color, isInstanced);
    }

    public void SetColorArray(
      string propertyName,
      Color[] colorList,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetColorArray(propertyId, colorList, isInstanced);
    }

    public void SetColorArray(
      int propertyId,
      Color[] colorList,
      bool isInstanced)
    {
      Vector4[] vectorList = new Vector4[colorList.Length];
      for (int i = 0; i < vectorList.Length; i++)
      {
        vectorList[i] = colorList[i];
      }
      SetVectorArray(propertyId, vectorList, isInstanced);
    }

    public void SetColorArray(
      string propertyName,
      List<Color> colorList,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetColorArray(propertyId, colorList, isInstanced);
    }

    public void SetColorArray(
      int propertyId,
      List<Color> colorList,
      bool isInstanced)
    {
      Vector4[] vectorList = new Vector4[colorList.Count];
      for (int i = 0; i < vectorList.Length; i++)
      {
        vectorList[i] = colorList[i];
      }
      SetVectorArray(propertyId, vectorList, isInstanced);
    }

    public void SetFloat(
      string propertyName,
      float value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetFloat(propertyId, value, isInstanced);
    }

    public void SetFloat(
      int propertyId,
      float value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetFloat(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetFloat(propertyId, value);
      }
    }

    public void SetFloatArray(
      string propertyName,
      float[] value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetFloatArray(propertyId, value, isInstanced);
    }

    public void SetFloatArray(
      int propertyId,
      float[] value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetFloatArray(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetFloatArray(propertyId, value);
      }
    }

    public void SetFloatArray(
      string propertyName,
      List<float> value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetFloatArray(propertyId, value, isInstanced);
    }

    public void SetFloatArray(
      int propertyId,
      List<float> value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetFloatArray(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetFloatArray(propertyId, value);
      }
    }

    /// <summary>
    /// Int cannot be instanced.
    /// </summary>
    public void SetInt(
      string propertyName,
      int value)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetInt(propertyId, value);
    }

    /// <summary>
    /// Int cannot be instanced.
    /// </summary>
    public void SetInt(
      int propertyId,
      int value)
    {
      material.SetInt(propertyId, value);
    }

    public void SetMainTexture(
      Texture value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetTexture(mainTexturePropertyId, value);
      }
      else
      {
        material.mainTexture = value;
      }
    }

    public void SetMatrix(
      string propertyName,
      Matrix4x4 value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetMatrix(propertyId, value, isInstanced);
    }

    public void SetMatrix(
      int propertyId,
      Matrix4x4 value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetMatrix(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetMatrix(propertyId, value);
      }
    }

    public void SetMatrixArray(
      string propertyName,
      Matrix4x4[] value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetMatrixArray(propertyId, value, isInstanced);
    }

    public void SetMatrixArray(
      int propertyId,
      Matrix4x4[] value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetMatrixArray(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetMatrixArray(propertyId, value);
      }
    }

    public void SetMatrixArray(
      string propertyName,
      List<Matrix4x4> value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetMatrixArray(propertyId, value, isInstanced);
    }

    public void SetMatrixArray(
      int propertyId,
      List<Matrix4x4> value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetMatrixArray(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetMatrixArray(propertyId, value);
      }
    }


    public void SetOverrideTag(
      string tag,
      string value)
    {
      material.SetOverrideTag(tag, value);
    }

    public bool SetPass(
      int pass)
    {
      return material.SetPass(pass);
    }

    public void SetShaderPassEnabled(
      string passName,
      bool enabled)
    {
      material.SetShaderPassEnabled(passName, enabled);
    }

    public void SetTexture(
      string propertyName,
      Texture value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetTexture(propertyId, value, isInstanced);
    }

    public void SetTexture(
      int propertyId,
      Texture value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetTexture(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetTexture(propertyId, value);
      }
    }

    public void SetTextureOffset(
      string propertyName,
      Vector2 value)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetTextureOffset(propertyId, value);
    }

    public void SetTextureOffset(
      int propertyId,
      Vector2 value)
    {
      material.SetTextureOffset(propertyId, value);
    }

    public void SetTextureScale(
      string propertyName,
      Vector2 value)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetTextureScale(propertyId, value);
    }

    public void SetTextureScale(
      int propertyId,
      Vector2 value)
    {
      material.SetTextureScale(propertyId, value);
    }

    public void SetVector(
      string propertyName,
      Vector4 value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetVector(propertyId, value, isInstanced);
    }

    public void SetVector(
      int propertyId,
      Vector4 value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetVector(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetVector(propertyId, value);
      }
    }

    public void SetVectorArray(
      string propertyName,
      Vector4[] value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetVectorArray(propertyName, value, isInstanced);
    }

    public void SetVectorArray(
      int propertyId,
      Vector4[] value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetVectorArray(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetVectorArray(propertyId, value);
      }
    }

    public void SetVectorArray(
      string propertyName,
      List<Vector4> value,
      bool isInstanced)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      SetVectorArray(propertyName, value, isInstanced);
    }

    public void SetVectorArray(
      int propertyId,
      List<Vector4> value,
      bool isInstanced)
    {
      if (isInstanced)
      {
        materialPropertyBlock.SetVectorArray(propertyId, value);
        renderer.SetPropertyBlock(materialPropertyBlock);
        instancedProperties.Add(propertyId);
      }
      else
      {
        material.SetVectorArray(propertyId, value);
      }
    }
    #endregion

    #region Read
    public int FindPass(
      string passName)
    {
      return material.FindPass(passName);
    }

    public Color GetColor(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetColor(propertyId);
    }

    public Color GetColor(
      int propertyId)
    {
      return GetVector(propertyId);
    }

    public Color GetMainColor()
    {
      return GetColor(mainColorPropertyId);
    }

    public Color[] GetColorArray(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetColorArray(propertyId);
    }

    public Color[] GetColorArray(
      int propertyId)
    {
      if (instancedProperties.Contains(propertyId))
      {
        Vector4[] vectorList = GetVectorArray(propertyId);
        Color[] colorList = new Color[vectorList.Length];
        for (int i = 0; i < vectorList.Length; i++)
        {
          colorList[i] = vectorList[i];
        }

        return colorList;
      }
      else
      {
        return material.GetColorArray(propertyId);
      }
    }

    public float GetFloat(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetFloat(propertyId);
    }

    public float GetFloat(
      int propertyId)
    {
      if (instancedProperties.Contains(propertyId))
      {
        return materialPropertyBlock.GetFloat(propertyId);
      }
      else
      {
        return material.GetFloat(propertyId);
      }
    }

    public float[] GetFloatArray(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetFloatArray(propertyId);
    }

    public float[] GetFloatArray(
      int propertyId)
    {
      if (instancedProperties.Contains(propertyId))
      {
        return materialPropertyBlock.GetFloatArray(propertyId);
      }
      else
      {
        return material.GetFloatArray(propertyId);
      }
    }

    /// <summary>
    /// Int cannot be instanced.
    /// </summary>
    public int GetInt(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetInt(propertyId);
    }

    /// <summary>
    /// Int cannot be instanced.
    /// </summary>
    public int GetInt(
      int propertyId)
    {
      return material.GetInt(propertyId);
    }

    public Texture GetMainTexture()
    {
      if (instancedProperties.Contains(mainTexturePropertyId))
      {
        return materialPropertyBlock.GetTexture(mainTexturePropertyId);
      }
      else
      {
        return material.mainTexture;
      }
    }

    public Matrix4x4 GetMatrix(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetMatrix(propertyId);
    }

    public Matrix4x4 GetMatrix(
      int propertyId)
    {
      if (instancedProperties.Contains(propertyId))
      {
        return materialPropertyBlock.GetMatrix(propertyId);
      }
      else
      {
        return material.GetMatrix(propertyId);
      }
    }

    public Matrix4x4[] GetMatrixArray(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetMatrixArray(propertyId);
    }

    public Matrix4x4[] GetMatrixArray(
      int propertyId)
    {
      if (instancedProperties.Contains(propertyId))
      {
        return materialPropertyBlock.GetMatrixArray(propertyId);
      }
      else
      {
        return material.GetMatrixArray(propertyId);
      }
    }

    public string GetPassName(
      int pass)
    {
      return material.GetPassName(pass);
    }

    public bool GetShaderPassEnabled(
      string passName)
    {
      return material.GetShaderPassEnabled(passName);
    }

    public Texture GetTexture(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetTexture(propertyId);
    }

    public Texture GetTexture(
      int propertyId)
    {
      if (instancedProperties.Contains(propertyId))
      {
        return materialPropertyBlock.GetTexture(propertyId);
      }
      else
      {
        return material.GetTexture(propertyId);
      }
    }

    public Vector2 GetTextureOffset(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetTextureOffset(propertyId);
    }

    public Vector2 GetTextureOffset(
      int propertyId)
    {
      return material.GetTextureOffset(propertyId);
    }

    public Vector2 GetTextureScale(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetTextureScale(propertyId);
    }

    public Vector2 GetTextureScale(
      int propertyId)
    {
      return material.GetTextureScale(propertyId);
    }

    public Vector4 GetVector(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetVector(propertyId);
    }

    public Vector4 GetVector(
      int propertyId)
    {
      if (instancedProperties.Contains(propertyId))
      {
        return materialPropertyBlock.GetVector(propertyId);
      }
      else
      {
        return material.GetVector(propertyId);
      }
    }

    public Vector4[] GetVectorArray(
      string propertyName)
    {
      int propertyId = Shader.PropertyToID(propertyName);
      return GetVectorArray(propertyName);
    }

    public Vector4[] GetVectorArray(
      int propertyId)
    {
      if (instancedProperties.Contains(propertyId))
      {
        return materialPropertyBlock.GetVectorArray(propertyId);
      }
      else
      {
        return material.GetVectorArray(propertyId);
      }
    }

    public bool HasProperty(
      string propertyName)
    {
      return material.HasProperty(propertyName);
    }

    public bool HasProperty(
      int propertyId)
    {
      return material.HasProperty(propertyId);
    }

    public bool IsKeywordEnabled(
      string keyword)
    {
      return material.IsKeywordEnabled(keyword);
    }
    #endregion
  }
}
