using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ErrorPanel : MonoBehaviour {

	public GameObject 	errorPanelObject;
	public Text			actionDescription;
	public Button 		yesButton;

	private ErrorPanel errorPanel;
	
	public void SetText(string text, UnityAction yesEvent){
		errorPanelObject.SetActive (true);
		actionDescription.text = text;

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);

		yesButton.gameObject.SetActive (true);
	}
	
	public void ClosePanel () {
		errorPanelObject.SetActive (false);
	}
}
