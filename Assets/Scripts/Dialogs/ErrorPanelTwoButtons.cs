using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ErrorPanelTwoButtons : MonoBehaviour {

	public GameObject 	errorPanelObject;
	public Text			actionDescription;
	public Button 		yesButton;
	public Button 		noButton;

	private ErrorPanel errorPanel;

	public void SetText(string text, UnityAction yesEvent, UnityAction noEvent){
		errorPanelObject.SetActive (true);
		actionDescription.text = text;

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);

		yesButton.gameObject.SetActive (true);

		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener (noEvent);
		noButton.onClick.AddListener (ClosePanel);

		noButton.gameObject.SetActive (true);

	}

	public void ClosePanel () {
		errorPanelObject.SetActive (false);
	}
}
