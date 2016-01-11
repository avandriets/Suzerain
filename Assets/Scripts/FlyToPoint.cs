using UnityEngine;

public class FlyToPoint : MonoBehaviour
{ 
  public float MovingTime = 0.3f;
  private Transform targetTransform = null;  
  private Vector3 startPosition = Vector3.zero;
  private Quaternion startRotation = Quaternion.identity;
  private float time = 0;
  	
	private void Update ()
  {
    CameraMoving();
  }

  public void StartFly(Transform _target)
  {    
    targetTransform = _target;
    enabled = true;
    time = 0;
    transform.parent = null;
    startPosition = transform.position;
    startRotation = transform.rotation;    
  }

  private void CameraMoving()
  {
    time += Time.deltaTime / MovingTime;
    if (time > 1)
    {       
      transform.parent = targetTransform;
      transform.localPosition = Vector3.zero;
      transform.localRotation = Quaternion.identity;
      enabled = false;
    }
    else
    {
      transform.position = Vector3.Lerp(startPosition, targetTransform.position, time);
      transform.rotation = Quaternion.Lerp(startRotation, targetTransform.rotation, time);
    }    
  }       
}
