using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class FightsPanel : MonoBehaviour {

	public GameObject 	fightPanelObject;

	public Button 		randomFightButton;
	public Button 		friendFightButton;

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

	}

	public void ClosePanel () {
		fightPanelObject.SetActive (false);
	}
}
