using UnityEngine;

namespace HD
{
  public class SelfDestruct : MonoBehaviour
  {
    [SerializeField]
    float timeTillDeath = 5;

    protected void Start()
    {
      Destroy(gameObject, timeTillDeath);
    }
  }
}
