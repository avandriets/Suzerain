using UnityEngine;

public class Trace : MonoBehaviour
{
  [HideInInspector] public float WayLength = 0;
  [SerializeField] private ParticleEmitter particleEmitter = null;
  [SerializeField] private GameObject bloodPrefab = null;
  private float currentWay = 0;
  private bool hasTarget = false;

  private void Start()
  {
    transform.parent = null;
  }

  private void Update ()
  {
    currentWay += 200 * Time.deltaTime;
    if (!hasTarget)
    {
      if (currentWay < WayLength)
        transform.Translate(0, 0, 200 * Time.deltaTime);      
      else
      {
        hasTarget = true;
        particleEmitter.emit = false;
        Instantiate(bloodPrefab, transform.position, transform.rotation);
      }
    }

  }
}
