using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaitPanel : MonoBehaviour {

	public GameObject 	waitingPanelObject;
	public Text			actionDescription;

	public Text			debugMessage;

	private WaitPanel waitingPanel;
	
	public void SetText(string text){

		waitingPanelObject.SetActive (true);
		actionDescription.text = text;

	}

	public void SetDebugMessage(string message){
	
		debugMessage.text = message;

	}

	public void ClosePanel () {
		waitingPanelObject.SetActive (false);
	}
	
}
