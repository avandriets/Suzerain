using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class InvitationDialog : MonoBehaviour {

	public GameObject 	invitationPanelObject;

	public Button 		okButton;

	public void ShowDialog(){
		invitationPanelObject.SetActive (true);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (ClosePanel);
		okButton.gameObject.SetActive (true);
	}

	public void ClosePanel () {
		invitationPanelObject.SetActive (false);
	}
}
