using System;
using System.Diagnostics;
using UnityEngine;

namespace HD
{
  public class OnXInstantiate : OnXDoY
  {
    [SerializeField]
    GameObject[] prefabList;

    protected override void DoChange()
    {
      for (int i = 0; i < prefabList.Length; i++)
      {
        GameObject prefab = prefabList[i];
        Instantiate(prefab, transform);
      }
    }
  }
}
