using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class AddToFriends : MonoBehaviour {

	public GameObject 	AddPanelObject;
	public Text			UserName;
	public Button 		yesButton;
	public Button 		noButton;

	private ErrorPanel errorPanel;

	public void ShowDialog(string pUserName, UnityAction yesEvent, UnityAction noEvent){
		
		AddPanelObject.SetActive (true);
		UserName.text = pUserName;

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
		AddPanelObject.SetActive (false);
	}

}
