using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
  [SerializeField] private PlayerSync playerSync = null;
  public int ArmoType = 0;
  [SerializeField] private Armo[] armos = null;  
  [SerializeField] private Transform armoRayTransform = null;
  [SerializeField] private FlyToPoint mainCamera = null;
  [SerializeField] private AnimationClip shockClip = null;
  [SerializeField] private AnimationClip reloadClip = null;
  [SerializeField] private AnimationClip schootClip = null;
  [SerializeField] private Transform spineBone = null;
  [SerializeField] private AudioClip shootAudioClip = null;
  [SerializeField] private float moveSpeed = 2;
  [SerializeField] private float helthStep = 50;
  [SerializeField] private float rotatingSpeed = 2;
  [SerializeField] private float stabilityTime = 1;
  [SerializeField] private float maxAngle = 4;
  [SerializeField] private float shootAiInterval = 1; 
  [SerializeField] private Camera demoCamera = null;  
  [SerializeField] private Transform cameraDeadPosition = null;  
  [HideInInspector] public Vector3 SpineBoneJoystickAngle = Vector3.zero;
  [HideInInspector] public Vector3 SpineBoneNetworkAngle = Vector3.zero;  
  [HideInInspector] public bool CanShoot = false;
  [HideInInspector] public bool IsMine = false;
  private Armo currentArmo = null;
  private GameObject buttonRestart = null;
  private Text helthIndicator = null;
  private Text patronsIndicator = null;
  private Character enemyCharacterBase = null;
  private Animator pistolAnimator = null;
  private bool ai = false;
  private float rayLength = 0.1f;
  private bool isUpdateDone = false;
  private Animator thisAnimator = null;
  private float currentMoveSpeed = 0;
  private float currentBoneAngle = 0;
  private bool isShooting = false;
  private bool go = false;
  private bool isDead = false;
  private float helth = 100;
  private bool toHead = false;
  private bool rotatingRight = false;
  private float currentReductionTime = 0;
  private float currentRotatingSpeed = 0;   
  private GUIController guiController = null;  
  private bool isNearBarrier = false;
  private Transform cameraParent = null;

  public float Helth
  {
    get { return helth;}
    set
    {
      helth = Mathf.Max(0, value);
      if (helthIndicator != null)
        helthIndicator.text = helth.ToString("f0")+"%";      
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

  private void Start () 
  {
    thisAnimator = GetComponent<Animator>();
    currentArmo = armos[ArmoType];
    int i = 0;
    foreach (var armo in armos)
    {
      armo.gameObject.SetActive(i == ArmoType);
      i++;
    }
    pistolAnimator = currentArmo.GetComponent<Animator>();
    guiController = FindObjectOfType<GUIController>();
    Time.timeScale = 1;
    currentReductionTime = currentArmo.ReductionTime;
    Joistick joistick = FindObjectOfType<Joistick>();    
    if (IsMine)
    {
      joistick.Character = this;
      helthIndicator = guiController.MyHelth;
      patronsIndicator = guiController.MyPatrons;
      cameraParent = mainCamera.transform.parent;
    }
    else
    {
      helthIndicator = guiController.EnemyHelth;
      patronsIndicator = guiController.EnemyPatrons;
      Destroy(mainCamera.gameObject);
    }
    patronsIndicator.text = currentArmo.Patrons.ToString();
    demoCamera = GameObject.Find("DemoCamera").GetComponent<Camera>();
    buttonRestart = FindObjectOfType<GUIController>().ButtonRestart; 
  }

  public void StartDuel()
  {
    Invoke("UpArmo", 1.5f);
    Character[] characterBases = FindObjectsOfType<Character>();
    foreach (var _characterBase in characterBases)
    {
      if (_characterBase != this)
        enemyCharacterBase = _characterBase;
    }    
  }
  private void UpArmo()
  {
    thisAnimator.SetTrigger("ArmoUp");
    Invoke("StartGo", 1.0f);
  }

  private void StartGo()
  {
    go = true;    
    CanShoot = true;
    Invoke("ChangeCamera", 1);    
  }

  private void Update()
  {
    isUpdateDone = true;
    currentMoveSpeed = go && CanShoot ? Mathf.Min(currentMoveSpeed + Time.deltaTime*0.1f, moveSpeed) : 0f;
    playerSync.transform.Translate(0, 0, currentMoveSpeed * Time.deltaTime * 4);
    currentReductionTime -= Time.deltaTime;
    if (currentReductionTime < 0)
      currentReductionTime = currentArmo.ReductionTime;
    currentRotatingSpeed = Mathf.Max(currentRotatingSpeed - Time.deltaTime * rotatingSpeed/stabilityTime, 0);
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      if (!IsMine)
      {
        enemyCharacterBase.ReduceHelth(false);
      }
    }    
  }

  private void LateUpdate()
  {
    float reductionKoeff = currentReductionTime/currentArmo.ReductionTime;
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
    }    
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
    float dist = 50;
    Gizmos.DrawRay(armoRayTransform.position, armoRayTransform.forward * dist);
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
    if (CanShoot && !isShooting && !isDead && currentArmo.Patrons > 0)
    {
      Shoot(canReduceHelth);
    }
  }

  private void Shoot(bool canReduceHelth)
  {
    isShooting = true;
    --currentArmo.Patrons;
    thisAnimator.SetBool("Reload", currentArmo.Patrons == 0);    
    if (patronsIndicator != null)
      patronsIndicator.text = currentArmo.Patrons.ToString();
    GameObject sparks = Instantiate(currentArmo.ParticlesPrefab, armoRayTransform.position, armoRayTransform.rotation) as GameObject;
    sparks.transform.parent = armoRayTransform;
    sparks.GetComponentInChildren<Trace>().WayLength = rayLength;    
    AudioSource thisAudio = GetComponent<AudioSource>();
    thisAudio.clip = shootAudioClip;
    thisAudio.Play();
    
    currentRotatingSpeed = rotatingSpeed;
    if (canReduceHelth)
    {      
      if (rayLength < 80)
      {
        enemyCharacterBase.ReduceHelth(toHead);
        if (enemyCharacterBase.Helth <= 0)
        {          
          Win();
        }
      }      
    }    
    thisAnimator.SetTrigger("Shoot");         
    go = false;
    thisAnimator.SetBool("Go", false);
    
    if (IsMine)
      playerSync.TryNetworkShoot(rayLength < 80, toHead);

    Invoke("EndShoot", schootClip.length);
  }

  private void EndShoot()
  {    
    currentReductionTime = currentArmo.ReductionTime;
    if (currentArmo.Patrons == 0)
    {
      Invoke("EndReload", reloadClip.length); 
    }
    else
    {
      ReturnFireIdleAnimation();
    }    
  }

  private void ReturnFireIdleAnimation()
  {
    isShooting = false;    
    go = !isNearBarrier;
    thisAnimator.SetBool("Go", !isNearBarrier);          
  }

  private void EndReload()
  {
    ReturnFireIdleAnimation();
    currentArmo.Reload();
    if (patronsIndicator != null)
      patronsIndicator.text = currentArmo.Patrons.ToString();
  }

  public void ReduceHelth(bool isHead)
  {    
    currentMoveSpeed = 0;
    currentReductionTime = currentArmo.ReductionTime;
    CanShoot = false;    
    enemyCharacterBase.CanShoot = false;
    Time.timeScale = 0.2f;    
    if (isHead)
      Helth = 0;
    else
      Helth -= helthStep;

    if (Helth > 0)
    {
      thisAnimator.SetTrigger("Shock");
      Invoke("ReturnShock", shockClip.length + 0.5f);
    }
    else
      Dead();

    if (IsMine)    
      mainCamera.StartFly(cameraDeadPosition);        
  }

  private void ReturnShock()
  {    
    Invoke("EnableShoot", 0.5f);
    if (IsMine)
      mainCamera.StartFly(cameraParent);
  }

  private void EnableShoot()
  {
    CanShoot = true;    
    enemyCharacterBase.CanShoot = true;
    Time.timeScale = 1;
  }

  private void Dead()
  {
    if (!isDead)
    {
      Time.timeScale = 1;
      isDead = true;      
      currentRotatingSpeed = 0;
      go = false;
      thisAnimator.SetTrigger("Dead");
      Invoke("ShowButtonRestart", 3);
    }    
    CanShoot = false;    
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
    hits = Physics.RaycastAll(armoRayTransform.position, armoRayTransform.forward, 150, layerMask);
    int i = 0;
    rayLength = 150;
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
      if (rayLength < 150 || Random.value < 0.4f)
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

  private void Win()
  {
    if (IsMine)
      mainCamera.StartFly(cameraDeadPosition);
    thisAnimator.SetTrigger("Win");
  }
}
