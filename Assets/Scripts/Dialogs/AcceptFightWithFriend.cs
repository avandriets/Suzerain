using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class AcceptFightWithFriend : MonoBehaviour {

	public GameObject 	AcceptFightPanelObject;

	public Text			fightType;
	public Text			rankOpponent;
	public Text			nameOpponent;
	public Image 		avatarOpponent;

	public Button 		yesButton;
	public Button 		noButton;

	public void ShowDialog(UnityAction yesEvent, UnityAction noEvent){

		AcceptFightPanelObject.SetActive (true);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (ClosePanel);
		yesButton.onClick.AddListener (yesEvent);
		yesButton.gameObject.SetActive (true);

		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener (ClosePanel);
		noButton.onClick.AddListener (noEvent);
		noButton.gameObject.SetActive (true);

		OnlineGame ong = OnlineGame.instance;

		fightType.text		= ScreensManager.LMan.getString (Utility.FightTypeName(ong.currentFight.FightTypeId));

		if (OnlineGame.statList [0].Fights >= Constants.fightsCount) {
			rankOpponent.text	= ScreensManager.LMan.getString (Utility.GetDifference (ong.opponent, OnlineGame.statList));
		}

		nameOpponent.text	= ong.opponent.UserName;
		Utility.setAvatar(avatarOpponent, OnlineGame.statList);
	}

	public void ClosePanel () {
		AcceptFightPanelObject.SetActive (false);
	}
}
