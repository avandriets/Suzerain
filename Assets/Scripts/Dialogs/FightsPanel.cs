using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class FightsPanel : MonoBehaviour {

	public GameObject 	fightPanelObject;

	public Button 		randomFightButton;
	public Button 		friendFightButton;

	public Button 		trienerFightButton;

	public void SetText(AskForFightDelegate pFightDelegate){
		fightPanelObject.SetActive (true);

		randomFightButton.onClick.RemoveAllListeners();
		randomFightButton.onClick.AddListener (() => pFightDelegate(0));
		randomFightButton.onClick.AddListener (ClosePanel);
		randomFightButton.gameObject.SetActive (true);

		friendFightButton.onClick.RemoveAllListeners();
		friendFightButton.onClick.AddListener (() => pFightDelegate(-1));
		friendFightButton.onClick.AddListener (ClosePanel);
		friendFightButton.gameObject.SetActive (true);

		trienerFightButton.onClick.RemoveAllListeners();
		trienerFightButton.onClick.AddListener (() => pFightDelegate(-5));
		trienerFightButton.onClick.AddListener (ClosePanel);
		trienerFightButton.gameObject.SetActive (true);

	}

	public void ClosePanel () {
		fightPanelObject.SetActive (false);
	}
}
