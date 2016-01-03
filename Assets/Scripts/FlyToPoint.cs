using UnityEngine;

public class FlyToPoint : MonoBehaviour
{ 
  public float MovingTime = 0.3f;
  private Transform targetTransform = null;  
  private Vector3 startPosition = Vector3.zero;
  private Quaternion startRotation = Quaternion.identity;
  private Vector3 finishPosition = Vector3.zero;
  private Quaternion finishRotation = Quaternion.identity;
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
    startPosition = transform.position;
    finishPosition = targetTransform.position;
    startRotation = transform.rotation;
    finishRotation = targetTransform.rotation;
    transform.parent = null;    
  }

  private void CameraMoving()
  {
    time += Time.deltaTime / MovingTime;
    if (time > 1)
    {
      time = 1;
      enabled = false;
      transform.parent = targetTransform;
    }
    transform.position = Vector3.Lerp(startPosition, finishPosition, time);
    transform.rotation = Quaternion.Lerp(startRotation, finishRotation, time);
  }       
}
