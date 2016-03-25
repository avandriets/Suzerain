using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public delegate void CloseMessagePanel(string pmessage);

public class SendMessage : MonoBehaviour {

	public GameObject 	errorPanelObject;
	public Text			actionDescription;
	public Button 		yesButton;
	public Button 		cancelButton;

	CloseMessagePanel SendDelegate = null;

	private ErrorPanel errorPanel;

	public void ShowDialog(string pInitText, CloseMessagePanel yesEvent, UnityAction cancelEvent){

		actionDescription.text = pInitText;

		SendDelegate = yesEvent;

		errorPanelObject.SetActive (true);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (ClosePanelAndSend);
		yesButton.gameObject.SetActive (true);

		cancelButton.onClick.AddListener (ClosePanel);
		cancelButton.onClick.AddListener (cancelEvent);
		cancelButton.gameObject.SetActive (true);

	}

	public void ClosePanelAndSend () {
		
		SendDelegate (actionDescription.text);
		errorPanelObject.SetActive (false);

	}

	public void ClosePanel () {
		
		errorPanelObject.SetActive (false);

	}

}
