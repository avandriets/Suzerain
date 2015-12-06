using UnityEngine;

public class Armo : MonoBehaviour 
{
  [SerializeField] private float shootTime = 1;
  [SerializeField] private float reloadTime = 5;
  [SerializeField] private float reductionTime = 15;
  [SerializeField] private int maxPatrons = 10;
  [HideInInspector] public int Patrons = 10;

  public float ShootTime
  {
    get { return shootTime;}
  }

  public float ReloadTime
  {
    get { return reloadTime; }
  }

  public float ReductionTime
  {
    get { return reductionTime; }
  }

  public void Reload()
  {
    Patrons = maxPatrons;
  }
}
