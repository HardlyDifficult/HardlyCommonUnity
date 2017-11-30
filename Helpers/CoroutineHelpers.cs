using System;
using System.Collections;

namespace HD
{
  public static class CoroutineHelpers
  {
    public static IEnumerator WithSleepIf(
      IEnumerator routine,
      Func<bool> sleepIf)
    {
      if (routine != null)
      {
        while (routine.MoveNext())
        {
          if (sleepIf())
          {
            yield return routine.Current;
          }
        }
      }
    }
  }
}
