using UnityEngine;

public class Armo : MonoBehaviour 
{
  [SerializeField] private GameObject particlesPrefab = null;
  [SerializeField] private float reductionTime = 5;
  [SerializeField] private int maxPatrons = 2;
  [HideInInspector] public int Patrons = 0;

  public GameObject ParticlesPrefab
  {
    get { return particlesPrefab; }
  }
  
  public float ReductionTime
  {
    get { return reductionTime; }
  }

  private void Awake()
  {
    Patrons = maxPatrons;
  }

  public void Reload()
  {
    Patrons = maxPatrons;
  }
}
