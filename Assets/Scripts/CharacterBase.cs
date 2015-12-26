using UnityEngine;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour
{
  [SerializeField] protected Transform armoTransform = null;
  [SerializeField] private Armo armo = null;
  [SerializeField] private AnimationClip shockClip = null;
  [SerializeField] private AnimationClip reloadClip = null;
  [SerializeField] private AnimationClip schootClip = null;
  [SerializeField] private Transform spineBone = null;
  [SerializeField] private GameObject shootSparks = null;
  [SerializeField] private GameObject buttonRestart = null;
  [SerializeField] private Text helthIndicator = null;
  [SerializeField] private Text patronsIndicator = null;  
  [SerializeField] protected CharacterBase enemyCharacterBase = null;
  [SerializeField] private AudioClip shootAudioClip = null;
  [SerializeField] private float moveSpeed = 2;
  [SerializeField] private float helthStep = 50;
  [SerializeField] private float rotatingSpeed = 2;
  [SerializeField] private float stabilityTime = 1;
  [SerializeField] private float maxAngle = 4;
  [SerializeField] private float shootAiInterval = 1;
  [SerializeField] private Animator pistolAnimator = null;
  [SerializeField] protected NetworkManager networkManager = null;
  [SerializeField] private PlayerSync playerSync = null;
  [SerializeField] private Camera demoCamera = null;
  [SerializeField] private Camera mainCamera = null;
  [SerializeField] private Transform cameraDeadPosition = null;
  [SerializeField] private float cameraMovingTime = 0.3f;
  /*[HideInInspector] */public Vector3 SpineBoneJoystickAngle = Vector3.zero;
  [HideInInspector] public Vector3 SpineBoneNetworkAngle = Vector3.zero;
  [HideInInspector] public bool CanRotating = true;
  [HideInInspector] public bool CanShoot = false;
  [HideInInspector] public bool IsMine = false;
  private bool ai = false;
  protected float rayLength = 0.1f;
  protected bool isUpdateDone = false;
  protected Animator thisAnimator = null;
  private float currentMoveSpeed = 0;
  private float currentBoneAngle = 0;
  private bool isShooting = false;
  protected bool go = false;
  private bool isDead = false;
  private float helth = 100;
  private bool toHead = false;
  private bool rotatingRight = false;
  private Vector3 spineRotation = Vector3.zero;
  private float currentReductionTime = 0;
  private float currentRotatingSpeed = 0;   
  private GUIController guiController = null;
  private Transform handBone = null;
  private bool isCameraMoving = false;
  private Vector3 startCameraPosition = Vector3.zero;
  private Quaternion startCameraLocalRotation = Quaternion.identity;
  private Vector3 finishCameraPosition = Vector3.zero;
  private Quaternion finishCameraLocalRotation = Quaternion.identity;
  private float time = 0;
  private bool moveToPistol = false;
  private bool isNearBarrier = false;

  public float Helth
  {
    get { return helth;}
    set
    {
      helth = Mathf.Max(0, value);
      if (helthIndicator != null)
        helthIndicator.text = helth.ToString("f0")+"%";
      if (helth <= 0)
        Dead();
    }
  }

  public bool Ai
  {
    get { return ai; }
    set
    {
      ai = value;
      Invoke("RunAiShoot", 2);      
    }
  }

  protected virtual void Start () 
  {
    thisAnimator = GetComponent<Animator>();
    thisAnimator.Play("Idle");
    networkManager = FindObjectOfType<NetworkManager>();
    guiController = FindObjectOfType<GUIController>();
    Time.timeScale = 1;
    spineRotation = spineBone.eulerAngles;
    if (pistolAnimator != null)
      pistolAnimator.speed = 0;
    currentReductionTime = armo.ReductionTime;
    Joistick joistick = FindObjectOfType<Joistick>();
    if (IsMine)
    {
      joistick.Character = this;
      helthIndicator = guiController.MyHelth;
      patronsIndicator = guiController.MyPatrons;      
    }
    else
    {
      helthIndicator = guiController.EnemyHelth;
      patronsIndicator = guiController.EnemyPatrons;
    }
    patronsIndicator.text = armo.Patrons.ToString();

    demoCamera = GameObject.Find("DemoCamera").GetComponent<Camera>();
    buttonRestart = FindObjectOfType<GUIController>().ButtonRestart; 
  }

  public void StartDuel()
  {
    Invoke("UpArmo", 1.5f);
    CharacterBase[] characterBases = FindObjectsOfType<CharacterBase>();
    foreach (var _characterBase in characterBases)
    {
      if (_characterBase != this)
        enemyCharacterBase = _characterBase;
    }    
  }
  private void UpArmo()
  {
    thisAnimator.Play("ArmoUp");
    Invoke("StartGo", 1.0f);
  }

  protected virtual void StartGo()
  {
    go = true;
    thisAnimator.Play("Walk");
    thisAnimator.SetBool("Go", true);
    CanShoot = true;
    Invoke("ChangeCamera", 1);
    handBone = mainCamera.transform.parent;
  }

  private void Update()
  {
    isUpdateDone = true;
    currentMoveSpeed = go && CanShoot ? Mathf.Min(currentMoveSpeed + Time.deltaTime, moveSpeed) : 0f;
    transform.parent.Translate(0, 0, currentMoveSpeed * Time.deltaTime*4);
    currentReductionTime -= Time.deltaTime;
    if (currentReductionTime < 0)
      currentReductionTime = armo.ReductionTime;
    currentRotatingSpeed = Mathf.Max(currentRotatingSpeed - Time.deltaTime * rotatingSpeed/stabilityTime, 0);
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Helth = 0;
    }
  }

  protected virtual void LateUpdate()
  {
    float reductionKoeff = currentReductionTime/armo.ReductionTime;
    if (rotatingRight)
    {
      currentBoneAngle -= Time.deltaTime * currentRotatingSpeed * reductionKoeff;
      if (currentBoneAngle < -maxAngle * reductionKoeff)
        rotatingRight = false;
    }
    else
    {
      currentBoneAngle += Time.deltaTime * currentRotatingSpeed;
      if (currentBoneAngle > maxAngle * reductionKoeff)
        rotatingRight = true;
    }
    if (!isDead)
    {
      if (IsMine)
        spineBone.rotation = Quaternion.Euler(spineBone.eulerAngles.x - SpineBoneJoystickAngle.y, spineBone.eulerAngles.y + currentBoneAngle + SpineBoneJoystickAngle.x, spineBone.eulerAngles.z);
      //spineBone.rotation = Quaternion.Euler(spineRotation.x - SpineBoneJoystickAngle.y, spineRotation.y + currentBoneAngle + SpineBoneJoystickAngle.x, spineRotation.z);      
      //else
      //  spineBone.rotation = Quaternion.Euler(spineRotation.x, spineRotation.y + currentBoneAngle, spineRotation.z);
    }
    CameraMoving();
  }

  private void FixedUpdate()
  {
    if (isUpdateDone)
    {
      GetRay();
      isUpdateDone = false;
    }    
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    float dist = 50;// Vector3.Distance(transform.position, enemyCharacterBase.transform.position);
    Gizmos.DrawRay(armoTransform.position, armoTransform.forward * dist);
  }
  public void NetworkTryShoot(bool hasTarget, bool _toHead)
  {
    if (!IsMine)
    {
      Shoot(false);
      if (hasTarget)
        enemyCharacterBase.ReduceHelth(_toHead);
    }
  }

  public void TryShoot(bool canReduceHelth)
  {
    if (CanShoot && !isShooting && !isDead && armo.Patrons > 0)
    {
      Shoot(canReduceHelth);
    }
  }

  private void Shoot(bool canReduceHelth)
  {
    //thisAnimator.Play("Shoot", 1);
    isShooting = true;
    --armo.Patrons;
    if (patronsIndicator != null)
      patronsIndicator.text = armo.Patrons.ToString();
    GameObject sparks = Instantiate(shootSparks, armoTransform.position, armoTransform.rotation) as GameObject;
    sparks.transform.parent = armoTransform;
    AudioSource thisAudio = GetComponent<AudioSource>();
    thisAudio.clip = shootAudioClip;
    thisAudio.Play();
    
    currentRotatingSpeed = rotatingSpeed;
    if (canReduceHelth)
    {      
      if (rayLength < 70)
      {
        enemyCharacterBase.ReduceHelth(toHead);
      }      
    }
    thisAnimator.Play("Shoot");    
    pistolAnimator.speed = 1;
    pistolAnimator.Play("Shoot");
    
    go = false;
    thisAnimator.SetBool("Go", false);
    
    if (IsMine)
        playerSync.TryNetworkShoot(rayLength < 70, toHead);

    Invoke("EndShoot", schootClip.length);
  }

  private void EndShoot()
  {    
    currentReductionTime = armo.ReductionTime;
    if (armo.Patrons == 0)
    {
      thisAnimator.Play("Reload");
      pistolAnimator.speed = 1;
      pistolAnimator.Play("Reload");
    }
    else
    {
      ReturnFireIdleAnimation();
    }
    Invoke("ArmoReload", armo.ReloadTime);
  }

  private void ReturnFireIdleAnimation()
  {
    isShooting = false;
    thisAnimator.Play("IdleShoot");
    if (!isNearBarrier)
    {
      go = true;
      thisAnimator.SetBool("Go", true);
    }
    if (pistolAnimator != null)
    {
      pistolAnimator.Play("Idle");      
    }
  }

  private void ArmoReload()
  {
    Invoke("ReturnFireIdleAnimation", reloadClip.length * 0.5f);
    armo.Reload();
    if (patronsIndicator != null)
      patronsIndicator.text = armo.Patrons.ToString();
  }

  private void ExitArmoReload()
  {
  }

  public virtual void ReduceHelth(bool isHead)
  {
    Helth -= helthStep; 
    currentMoveSpeed = 0;
    CanShoot = false;
    enemyCharacterBase.CanShoot = false;
    Time.timeScale = 0.2f;
    if (isHead)
      Helth = 0;
    else
    {
      thisAnimator.SetBool("Shock", true);
      if (Helth > 0)
        Invoke("ReturnShock", shockClip.length);
    }
    currentReductionTime = armo.ReductionTime;
    if (IsMine)
    {
      isCameraMoving = true;
      CanRotating = false;
      time = 0;
      moveToPistol = false;

      startCameraPosition = mainCamera.transform.position;
      finishCameraPosition = cameraDeadPosition.position;
      startCameraLocalRotation = mainCamera.transform.rotation;
      finishCameraLocalRotation = cameraDeadPosition.rotation;
      mainCamera.transform.parent = null;
    }
  }

  protected virtual void ReturnShock()
  {
    Invoke("EnableShoot", 0.7f);
    thisAnimator.SetBool("Shock", false);
    Invoke("ParentingCameraToPistol", 0.90f);
  }

  private void EnableShoot()
  {
    CanShoot = true;
    enemyCharacterBase.CanShoot = true;
  }

  protected virtual void Dead()
  {
    if (!isDead)
    {
      isDead = true;
      thisAnimator.enabled = false;
      currentRotatingSpeed = 0;
      go = false;
      Invoke("ShowButtonRestart", 3);
    }
    CanRotating = false;
    mainCamera.transform.parent = null;
    mainCamera.transform.position = cameraDeadPosition.position;
    mainCamera.transform.rotation = cameraDeadPosition.rotation;
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Barrier")
      StopNearBarrier();
  }

  private void StopNearBarrier()
  {
    go = false;
    thisAnimator.SetBool("Go", false);
    isNearBarrier = true;
  }
  
  private void GetRay()
  {
    int layerMask = 1 << 8;
    RaycastHit[] hits;
    hits = Physics.RaycastAll(armoTransform.position, armoTransform.forward, 100, layerMask);
    int i = 0;
    rayLength = 100;
    toHead = false;
    while (i < hits.Length)
    {
      RaycastHit hit = hits[i];

      if (hit.distance < rayLength)
      {
        rayLength = hit.distance;
        toHead = hit.collider.gameObject.tag == "Head";
      }
      i++;
    }
  }

  private void ShowButtonRestart()
  {
    buttonRestart.SetActive(true);
  }

  private void RunAiShoot()
  {
    if (enemyCharacterBase.Helth > 0)
    {
      if (rayLength < 60 || Random.value < 0.4f)
        TryShoot(true);
      Invoke("RunAiShoot", shootAiInterval);
    }
  }

  private void ChangeCamera()
  {
    if (demoCamera != null)
      demoCamera.gameObject.SetActive(false);
    if (IsMine)
      mainCamera.gameObject.SetActive(true);
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
        if (moveToPistol)
        {
          mainCamera.transform.parent = handBone;
          Time.timeScale = 1;
          CanRotating = true;
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

  private void ParentingCameraToPistol()
  {
    if (Helth > 0)
    {
      isCameraMoving = true;
      CanRotating = false;
      moveToPistol = true;
      time = 0;
      startCameraPosition = mainCamera.transform.position;
      startCameraLocalRotation = mainCamera.transform.localRotation;
    }
  }
}
