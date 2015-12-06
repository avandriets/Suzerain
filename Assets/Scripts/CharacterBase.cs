using UnityEngine;
using UnityEngine.UI;

public class CharacterBase : MonoBehaviour
{
  [SerializeField] protected Transform armoTransform = null;
  [SerializeField] private Armo armo = null;
  [SerializeField] private AnimationClip shockClip = null;
  [SerializeField] private Transform spineBone = null;
  [SerializeField] private GameObject shootSparks = null;
  [SerializeField] private GameObject buttonRestart = null;
  [SerializeField] private Text helthIndicator = null;
  [SerializeField] private Text patronsIndicator = null;
  [SerializeField] private Text rayIndicator = null;
  [SerializeField] protected CharacterBase enemyCharacterBase = null;
  [SerializeField] private AudioClip shootAudioClip = null;
  [SerializeField] private float moveSpeed = 2;
  [SerializeField] private float helthStep = 50;
  [SerializeField] private float rotatingSpeed = 2;
  [SerializeField] private float maxAngle = 4;
  [SerializeField] private Animator pistolAnimator = null;
  [HideInInspector] public Vector2 SpineBoneJoystickAngle = Vector2.zero;
  protected float rayLength = 0.1f;
  protected bool isUpdateDone = false;
  protected Animator thisAnimator = null;
  private float currentMoveSpeed = 0;
  private float currentBoneAngle = 0;
  private bool isShooting = false;
  protected bool go = false;
  [HideInInspector] public bool CanShoot = false;
  private bool isDead = false;
  private float helth = 100;
  private bool toHead = false;
  private bool rotatingRight = false;
  private Vector3 spineRotation = Vector3.zero;
  private float currentReductionTime = 0;


  public float Helth
  {
    get { return helth;}
    set
    {
      helth = Mathf.Max(0, value);
      helthIndicator.text = helth.ToString("f0")+"%";
      if (helth <= 0)
        Dead();
    }
  }

  protected virtual void Start () 
  {
    thisAnimator = GetComponent<Animator>();
    Invoke("UpArmo", 1.5f);
    if (pistolAnimator != null)
      pistolAnimator.speed = 0;
    spineRotation = spineBone.eulerAngles;
    currentReductionTime = armo.ReductionTime;
    Time.timeScale = 1;
  }
  private void UpArmo()
  {
    thisAnimator.Play("ArmoUp");
    Invoke("StartGo", 1.0f);
  }

  protected virtual void StartGo()
  {
    go = true;
    thisAnimator.Play("Walk", 1);
    thisAnimator.SetBool("Go", true);
    CanShoot = true;
  }

  private void Update()
  {
    isUpdateDone = true;
    currentMoveSpeed = go && CanShoot ? Mathf.Min(currentMoveSpeed + Time.deltaTime, moveSpeed) : 0f;
    transform.parent.Translate(0, 0, currentMoveSpeed * Time.deltaTime*4);
    currentReductionTime -= Time.deltaTime;
    if (currentReductionTime < 0)
      currentReductionTime = armo.ReductionTime;
  }

  protected virtual void LateUpdate()
  {
    float reductionKoeff = currentReductionTime/armo.ReductionTime;
    if (rotatingRight)
    {
      currentBoneAngle -= Time.deltaTime * rotatingSpeed * reductionKoeff;
      if (currentBoneAngle < -maxAngle * reductionKoeff)
        rotatingRight = false;
    }
    else
    {
      currentBoneAngle += Time.deltaTime * rotatingSpeed;
      if (currentBoneAngle > maxAngle * reductionKoeff)
        rotatingRight = true;
    }
    spineBone.rotation = Quaternion.Euler(spineRotation.x, spineRotation.y + currentBoneAngle + SpineBoneJoystickAngle.x, spineRotation.z - SpineBoneJoystickAngle.y);
  }

  private void FixedUpdate()
  {
    if (isUpdateDone)
    {
      GetRay();
      isUpdateDone = false;
    }
    if (rayIndicator != null)
      rayIndicator.text = rayLength < 70 ? "+" : "-";
  }

  private void OnDrawGizmos()
  {
    float dist = Vector3.Distance(transform.position, enemyCharacterBase.transform.position);
    Gizmos.DrawRay(armoTransform.position, armoTransform.forward * dist);
  }

  public void TryShoot()
  {
    if (CanShoot && !isShooting && !isDead && armo.Patrons > 0)
    {
      Shoot();
    }
  }

  private void Shoot()
  {
    isShooting = true;
    --armo.Patrons;
    patronsIndicator.text = armo.Patrons.ToString();
    GameObject sparks = Instantiate(shootSparks, armoTransform.position, armoTransform.rotation) as GameObject;
    sparks.transform.parent = armoTransform;
    AudioSource thisAudio = GetComponent<AudioSource>();
    thisAudio.clip = shootAudioClip;
    thisAudio.Play();
    Invoke("ReturnFireIdleAnimation", armo.ShootTime);
    if (rayLength < 70)
    {
      enemyCharacterBase.ReduceHelth(toHead);
    }
    if (pistolAnimator != null)
    {
      pistolAnimator.speed = 1;
      //pistolAnimator.Play("PistolShoot");
    }
    currentReductionTime = armo.ReductionTime;
    if (armo.Patrons == 0)
      Invoke("ArmoReload", armo.ReloadTime);
  }

  private void ReturnFireIdleAnimation()
  {
    isShooting = false;
    if (pistolAnimator != null)
    {
      pistolAnimator.speed = 0;
    }
  }

  private void ArmoReload()
  {
    armo.Reload();
    patronsIndicator.text = armo.Patrons.ToString();
  }

  public virtual void ReduceHelth(bool isHead)
  {
    Helth -= helthStep; 
    currentMoveSpeed = 0;
    CanShoot = false;
    enemyCharacterBase.CanShoot = false;
    if (isHead)
      Helth = 0;
    else
    {
      thisAnimator.SetBool("Shock", true);
      if (Helth > 0)
        Invoke("ReturnShock", shockClip.length);
    }
    currentReductionTime = armo.ReductionTime;
  }

  protected virtual void ReturnShock()
  {
    Invoke("EnableShoot", 0.7f);
    thisAnimator.SetBool("Shock", false);
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
      rotatingSpeed = 0;
      go = false;
      Invoke("ShowButtonRestart", 3);
    }
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
}
