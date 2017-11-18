using System;
using UnityEngine;

namespace HD
{
  public class OnXSetActiveFalse : OnXDoY
  {
    protected override void DoChange()
    {
      Debug.Assert(enabled);

      gameObject.SetActive(false);
    }
  }
}
