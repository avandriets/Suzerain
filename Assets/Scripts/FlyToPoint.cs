using UnityEngine;

public class FlyToPoint : MonoBehaviour
{
  private Transform targetTransform = null;
  [SerializeField] private float cameraMovingTime = 0.3f;
  private bool isCameraMoving = false;
  private Vector3 startCameraPosition = Vector3.zero;
  private Quaternion startCameraLocalRotation = Quaternion.identity;
  private Vector3 finishCameraPosition = Vector3.zero;
  private Quaternion finishCameraLocalRotation = Quaternion.identity;
  //private Transform handBone = null;
  private float time = 0;
  //private bool moveToPistol = false;
  

  private void Start ()
  {
	
	}
	
	private void LateUpdate ()
  {
    CameraMoving();
  }

  public void StartFly(Transform _target)
  {
    targetTransform = _target;
    //handBone = transform.parent;
    isCameraMoving = true;    
    time = 0;
    //moveToPistol = false;
    startCameraPosition = transform.position;
    finishCameraPosition = targetTransform.position;
    startCameraLocalRotation = transform.rotation;
    finishCameraLocalRotation = targetTransform.rotation;
    transform.parent = null;
  }

  private void CameraMoving()
  {
    if (isCameraMoving)
    {
      time += Time.deltaTime / cameraMovingTime;
      if (time > 1)
      {
        time = 1;
        isCameraMoving = false;
        transform.parent = targetTransform;          
      }
      transform.position = Vector3.Lerp(startCameraPosition, finishCameraPosition, time);
      transform.rotation = Quaternion.Lerp(startCameraLocalRotation, finishCameraLocalRotation, time);     
    }
  }

  /*private void ParentingCameraToPistol()
  {
    if (Helth > 0)
    {
      isCameraMoving = true;
      CanRotating = false;
      moveToPistol = true;
      time = 0;
      startCameraPosition = transform.position;
      startCameraLocalRotation = transform.localRotation;
    }
  }*/
}
