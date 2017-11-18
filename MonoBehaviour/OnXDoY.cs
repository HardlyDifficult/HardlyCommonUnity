using System;
using UnityEngine;

namespace HD
{
  public abstract class OnXDoY : MonoBehaviour
  {
    [SerializeField]
    MonoBehaviourEventType eventType = MonoBehaviourEventType.Awake;

    protected void Awake()
    {
      TryChange(MonoBehaviourEventType.Awake);
    }

    protected void Start()
    {
      TryChange(MonoBehaviourEventType.Start);
    }

    void TryChange(
      MonoBehaviourEventType eventType)
    {
      if (this.eventType != eventType)
      {
        return;
      }

      DoChange();
    }

    protected abstract void DoChange();
  }
}
