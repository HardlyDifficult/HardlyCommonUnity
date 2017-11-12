using System;
using UnityEngine;

namespace HD
{
  public class OnXSetActiveFalse : MonoBehaviour
  {
    public enum EventType
    {
      Awake, Start
    }
    [SerializeField]
    EventType eventType;

    protected void Awake()
    {
      TryChange(EventType.Awake);
    }

    protected void Start()
    {
      TryChange(EventType.Start);
    }

    void TryChange(
      EventType eventType)
    {
      if(this.eventType != eventType)
      {
        return;
      }

      Debug.Assert(enabled);

      gameObject.SetActive(false);
    }
  }
}
