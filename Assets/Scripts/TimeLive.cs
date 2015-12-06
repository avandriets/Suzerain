using UnityEngine;

public class TimeLive : MonoBehaviour
{
  [SerializeField] private float timeLive = 1;
	
	private void Start () 
  {
	  Destroy(gameObject, timeLive);
	}
}
