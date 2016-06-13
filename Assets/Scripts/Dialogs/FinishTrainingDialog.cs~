using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class FinishTrainingDialog : MonoBehaviour {

	public GameObject 	mPanelObject;
	public Button 		okButton;

	public Text	gameCount;
	public Text timeInGame;

	public void ShowDialog(int pGameCount, int pTimeInGame, UnityAction yesEvent){
		mPanelObject.SetActive (true);

		gameCount.text = pGameCount.ToString ();

		var t = System.TimeSpan.FromSeconds( (double)pTimeInGame );

		timeInGame.text = string.Format ("{0}:{1}", t.Minutes, t.Seconds);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (yesEvent);
		okButton.onClick.AddListener (ClosePanel);

		okButton.gameObject.SetActive (true);
	}

	public void ClosePanel () {
		mPanelObject.SetActive (false);
	}

}
