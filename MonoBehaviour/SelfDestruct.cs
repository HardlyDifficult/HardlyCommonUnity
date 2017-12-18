using UnityEngine;

namespace HD
{
  public class SelfDestruct : MonoBehaviour
  {
    [SerializeField]
    float timeTillDeath = 5;

    protected void Start()
    {
      Debug.Log($"Destroy {gameObject} in {timeTillDeath}");
      Destroy(gameObject, timeTillDeath);
    }
  }
}
