using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class FinishTrainingDialog : MonoBehaviour {

	public GameObject 	mPanelObject;
	public Button 		okButton;

	public Text	gameCount;
	public Text timeInGame;

	public Text TextGamerName;
	public Text TextSQ;

	public void ShowDialog(double pSQ, int pGameCount, int pTimeInGame, UnityAction yesEvent){
		mPanelObject.SetActive (true);

		gameCount.text = pGameCount.ToString ();

		var t = System.TimeSpan.FromSeconds( (double)pTimeInGame );

		timeInGame.text = string.Format ("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

		TextGamerName.text = UserController.currentUser.UserName;

		if (pSQ > 0)
			TextSQ.text = string.Format ("{0:N2}", pSQ);
		else
			TextSQ.text = "0";
		
		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (yesEvent);
		okButton.onClick.AddListener (ClosePanel);

		okButton.gameObject.SetActive (true);
	}

	public void ClosePanel () {
		mPanelObject.SetActive (false);
	}

}
