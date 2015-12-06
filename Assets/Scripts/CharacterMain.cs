using UnityEngine;

public class CharacterMain : CharacterBase 
{
  [SerializeField] private Camera demoCamera = null;
  [SerializeField] private Camera mainCamera = null;
  [SerializeField] private Transform cameraDeadPosition = null;
  [SerializeField] private float cameraMovingTime = 0.3f;
  private Transform handBone = null;
  private bool isCameraMoving = false;
  private Vector3 startCameraPosition = Vector3.zero;
  private Quaternion startCameraLocalRotation = Quaternion.identity;
  private Vector3 finishCameraPosition = Vector3.zero;
  private Quaternion finishCameraLocalRotation = Quaternion.identity;
  private float time = 0;
  private bool moveToPistol = false;

  protected override void StartGo()
  {
    base.StartGo();
    Invoke("ChangeCamera", 1);
    handBone = mainCamera.transform.parent;
  }

  private void ChangeCamera()
  {
    demoCamera.gameObject.SetActive(false);
    mainCamera.gameObject.SetActive(true);
  }

  protected override void LateUpdate()
  {
    base.LateUpdate();
    
    if (isCameraMoving)
    {
      time += Time.deltaTime / cameraMovingTime;
      if (time > 1)
      {
        time = 1;
        isCameraMoving = false;
        if (moveToPistol)
        {
          mainCamera.transform.parent = handBone;
          Time.timeScale = 1;
        }
      }
      if (moveToPistol)
      {
        mainCamera.transform.position = Vector3.Lerp(startCameraPosition, handBone.transform.position, time);
        mainCamera.transform.rotation = Quaternion.Lerp(startCameraLocalRotation, handBone.transform.rotation, time);
        
      }
      else
      {
        mainCamera.transform.position = Vector3.Lerp(startCameraPosition, finishCameraPosition, time);
        mainCamera.transform.rotation = Quaternion.Lerp(startCameraLocalRotation, finishCameraLocalRotation, time);
      }
    }
  }

  protected override void Dead()
  {
    base.Dead();
    mainCamera.transform.parent = null;
    mainCamera.transform.position = cameraDeadPosition.position;
    mainCamera.transform.rotation = cameraDeadPosition.rotation;
  }

  public override void ReduceHelth(bool isHead)
  {
    base.ReduceHelth(isHead);
    isCameraMoving = true;
    time = 0;
    moveToPistol = false;
    Time.timeScale = 0.2f;
    startCameraPosition = mainCamera.transform.position;
    finishCameraPosition = cameraDeadPosition.position;
    startCameraLocalRotation = mainCamera.transform.rotation;
    finishCameraLocalRotation = cameraDeadPosition.rotation;
    mainCamera.transform.parent = null;
  }

  protected override void ReturnShock()
  {
    base.ReturnShock();
    Invoke("ParentingCameraToPistol", 0.90f);
  }

  private void ParentingCameraToPistol()
  {
    if (Helth > 0)
    {
      isCameraMoving = true;
      moveToPistol = true;
      time = 0;
      startCameraPosition = mainCamera.transform.position;
      startCameraLocalRotation = mainCamera.transform.localRotation;
    }
  }
}
