using System;
using UnityEngine;

namespace HD
{
  public abstract class SingletonMonoBehaviour<TChildClass> : MonoBehaviour
    where TChildClass : SingletonMonoBehaviour<TChildClass>
  {
    static TChildClass _instance;

    public static TChildClass instance
    {
      get
      {
        if(_instance == null)
        {
          _instance = GameObject.FindObjectOfType<TChildClass>();
          Debug.Assert(_instance != null, "Singleton object not found");
        }

        return _instance;
      }
    }

    protected virtual void Awake()
    {
      Debug.Assert(_instance == null || _instance == this);

      _instance = (TChildClass)this;
    }

    protected virtual void OnDestroy()
    {
      _instance = null;
    }
  }
}
