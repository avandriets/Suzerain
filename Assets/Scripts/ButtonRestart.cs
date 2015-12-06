using UnityEngine;

public class ButtonRestart : MonoBehaviour 
{

	public void OnPress() 
  {
	  Application.LoadLevel(Application.loadedLevel);
	}
}
