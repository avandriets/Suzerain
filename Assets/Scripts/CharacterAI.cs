using UnityEngine;

public class CharacterAI: CharacterBase
{
  [SerializeField] private float shootInterval = 1;

  protected override void Start()
  {
    base.Start();
    Invoke("RunShoot", shootInterval);
  }

  private void RunShoot()
  {
    if (enemyCharacterBase.Helth > 0)
    {
      if (rayLength < 60 || Random.value < 0.4f)
        TryShoot();
      Invoke("RunShoot", shootInterval);
    }
  }
}
