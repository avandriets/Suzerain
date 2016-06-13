using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;


public class PremiumDialog : MonoBehaviour {

	public GameObject 	mPanelObject;
	public Button 		yesButton;
	public Button 		noButton;


	public void ShowDialog(UnityAction yesEvent, UnityAction noEvent){
		mPanelObject.SetActive (true);

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
		mPanelObject.SetActive (false);
	}
}
