using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class OpponentIntroduce : MonoBehaviour {

	public GameObject 	opponentIntroducePanelObject;
	public Text			fightType;
	public Text			rankOpponent;
	public Text			nameOpponent;
	public Image 		avatarOpponent;
	public Button 		playButton;


	public void IntroduceOpponent(UnityAction playEvent){

		OnlineGame ong = OnlineGame.instance;

		opponentIntroducePanelObject.SetActive (true);
	
		playButton.onClick.RemoveAllListeners();
		playButton.onClick.AddListener (playEvent);
		playButton.onClick.AddListener (ClosePanel);

		playButton.gameObject.SetActive (true);

		fightType.text		= ScreensManager.LMan.getString (Utility.FightTypeName(ong.currentFight.FightTypeId));

		if (OnlineGame.statList [0].Fights >= Constants.fightsCount) {
			rankOpponent.text	= ScreensManager.LMan.getString (Utility.GetDifference (ong.opponent, OnlineGame.statList));
		}

		nameOpponent.text	= ong.opponent.UserName;
		Utility.setAvatar(avatarOpponent, OnlineGame.statList);

	}

	public void ClosePanel () {
		opponentIntroducePanelObject.SetActive (false);
	}

}
