using UnityEngine;

public class Joistick : MonoBehaviour
{
  [SerializeField] private Transform joystick = null;
  public CharacterBase Character = null;
  [SerializeField] private float size = 200;
  [SerializeField] private float sensitivityX = 4f;
  [SerializeField] private float sensitivityY = 0.2f;
  private Vector2 joystickPosition = Vector2.zero;
  private Vector2 startPressPosition = Vector2.zero;
  private bool isTouch = false;

  private void Start()
  {
    size = size*Screen.width/2048;
  }

  private void LateUpdate () 
  {
    if (Input.GetMouseButtonDown(0))
    {
      if ((Mathf.Abs(Input.mousePosition.x - transform.position.x) < size) &&
          Mathf.Abs(Input.mousePosition.y - transform.position.y) < size)
      {
        isTouch = true;
        startPressPosition = new Vector2(Input.mousePosition.x - joystick.transform.position.x, Input.mousePosition.y - joystick.transform.position.y);
      }
    }
    if (Input.GetMouseButtonUp(0) && isTouch)
    {
      Character.TryShoot(true);
      isTouch = false;
    }

    if (Input.GetMouseButton(0) && isTouch && Character.CanRotating)
    {
      float x = Mathf.Clamp(Input.mousePosition.x - startPressPosition.x, transform.position.x - size, transform.position.x + size);
      float y = Mathf.Clamp(Input.mousePosition.y - startPressPosition.y, transform.position.y - size, transform.position.y + size);
      joystickPosition = new Vector2(x, y);
      joystick.transform.position = new Vector3(x, y, 0);
      joystickPosition = new Vector2((x - transform.position.x)/size, (y - transform.position.y)/size);
      Character.SpineBoneJoystickAngle = new Vector2(joystickPosition.x * sensitivityX, joystickPosition.y * sensitivityY);
    }
  }
}
