using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class FightResultDialog : MonoBehaviour
{

	public GameObject fightResultPanelObject;
	public Text actionDescription;
	public Button okButton;
	public Text scoreUser;

	public GameObject imageWin;
	public GameObject imageLose;
	public GameObject imageDraft;

	//public GameObject	rightAnswerObject;
	//public Text			answerDescription;

	public List<Image>	fightsImageList;

	public GameObject	buyButton;
	public Text			rightAnswer;
	public Text			askMoney;
	public GameObject	wrongAnswerObject;
	public Image oneTalant;
	public Image twoTalant;

	public Image oneTalantPrise;
	public Image twoTalantPrise;

	int fightType;

	public void SetText (string text, int fightState, UnityAction okEvent, List<Fight> fight, string scrUs, string pRightAnswer)
	{

		twoTalantPrise.gameObject.SetActive (false);
		oneTalantPrise.gameObject.SetActive (false);

		Sprite spriteDraft		= null;
		Sprite spriteWin		= null;
		Sprite spriteLose		= null;
		Sprite spriteDefault	= null;

		Texture2D texture = Resources.Load ("draft_sign") as Texture2D;
		if (texture != null) {
			spriteDraft = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
		}

		texture = Resources.Load ("win_sign") as Texture2D;
		if (texture != null) {
			spriteWin = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
		}

		texture = Resources.Load ("lose_sign") as Texture2D;
		if (texture != null) {
			spriteLose = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
		}

		if (spriteDefault == null) {
			texture = Resources.Load ("default_sign") as Texture2D;
			if (texture != null) {
				spriteDefault = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
			}
		}

		fightsImageList [0].gameObject.SetActive (true);
		fightsImageList [1].gameObject.SetActive (true);
		fightsImageList [2].gameObject.SetActive (true);

		foreach (var cc in fightsImageList) {
			cc.sprite = spriteDefault;
		}

		fightResultPanelObject.SetActive (true);

		if (fight.Count > 1) {
			
			foreach (var currentFight in fight) {
				
				if (currentFight.IsDraw == true) {
					fightsImageList [fight.IndexOf (currentFight)].sprite = spriteDraft;
				} else if (currentFight.Winner == UserController.currentUser.Id) {
					fightsImageList [fight.IndexOf (currentFight)].sprite = spriteWin;
				} else {
					fightsImageList [fight.IndexOf (currentFight)].sprite = spriteLose;
				}

//				var ong = OnlineGame.instance;
//
//				if (!ong.mFightWithFriend) {
//					if (Utility.hasSubscription ()) {
//						twoTalantPrise.gameObject.SetActive (true);
//					} else {
//						oneTalantPrise.gameObject.SetActive (true);
//					}
//				}

			}
		} else {
			
			fightsImageList [0].gameObject.SetActive (false);
			fightsImageList [2].gameObject.SetActive (false);

			if (fight[0].IsDraw == true) {
				fightsImageList [1].sprite = spriteDraft;
			} else if (fight[0].Winner == UserController.currentUser.Id) {
				fightsImageList [1].sprite = spriteWin;

//				if (Utility.hasSubscription ()) {
//					twoTalantPrise.gameObject.SetActive (true);
//				} else {
//					oneTalantPrise.gameObject.SetActive (true);
//				}

			} else {
				fightsImageList [1].sprite = spriteLose;
			}
		}

		actionDescription.text = System.Text.RegularExpressions.Regex.Unescape (text);
		scoreUser.text = scrUs;

		if (fightState == -1) {
			imageLose.SetActive (true);
		} else if (fightState == 0) {
			imageDraft.SetActive (true);
		} else {
			imageWin.SetActive (true);

			var ong = OnlineGame.instance;

			if (!ong.mFightWithFriend) {
				if (Utility.hasSubscription ()) {
					twoTalantPrise.gameObject.SetActive (true);
				} else {
					oneTalantPrise.gameObject.SetActive (true);
				}
			}
		}

		okButton.onClick.RemoveAllListeners ();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);
		
		okButton.gameObject.SetActive (true);

		fightType = fight [0].FightTypeId;

		bool showRightAnswer = false;
		foreach(var ii in fight){
			if (ii.Looser == UserController.currentUser.Id || (ii.IsDraw && ii.RealInitiatorAnswer < 0 && ii.RealOpponentAnswer < 0)) {
				showRightAnswer = true;
			}
		}

		rightAnswer.text = pRightAnswer;

		if (fight.Count == 1 && 
			(fight [0].FightTypeId != Utility.Intuition &&
		     fight [0].FightTypeId != Utility.Reflex &&
			fight [0].FightTypeId != Utility.Iridizia)) {
		
			if (showRightAnswer) {
				if (fight [0].FightTypeId == Utility.Mudrost || fight [0].FightTypeId == Utility.Logic || fight [0].FightTypeId == Utility.Razum) {
					askMoney.text = string.Format ("открыть за {0} таланта", 2);
					twoTalant.gameObject.SetActive (true);
				} else {
					askMoney.text = string.Format ("открыть за {0} талант", 1);
					oneTalant.gameObject.SetActive (true);
				}

				buyButton.gameObject.SetActive (true);
				wrongAnswerObject.gameObject.SetActive (true);
			}
		}

//		//Show right answer
//		if(fight.Count == 1 && 
//			(fight[fight.Count-1].FightTypeId == 1 || fight[fight.Count-1].FightTypeId == 2 || fight[fight.Count-1].FightTypeId == 3 
//				|| fight[fight.Count-1].FightTypeId == 5)){
//
//			rightAnswerObject.SetActive (true);
//			answerDescription.text = pRightAnswer;
//		}
//
//		if (Storage.GetMoneyBalance () > 0) {
//
//			buyButton.gameObject.SetActive (false);
//			rightAnswer.gameObject.SetActive (true);
//
//			if (fightType == Utility.Mudrost || fightType == Utility.Logic) {
//				Storage.TakeMoney (2);
//			} else {
//				Storage.TakeMoney (1);
//			}
//		}

	}

	public void showRightAnswer(){

		if (Storage.GetMoneyBalance () > 0) {

			buyButton.gameObject.SetActive (false);
			rightAnswer.gameObject.SetActive (true);

			if (fightType == Utility.Mudrost || fightType == Utility.Logic || fightType == Utility.Razum) {
				Storage.TakeMoney (2);
			} else {
				Storage.TakeMoney (1);
			}
		}
	}

	public void ClosePanel ()
	{

		twoTalantPrise.gameObject.SetActive (false);
		oneTalantPrise.gameObject.SetActive (false);

		oneTalant.gameObject.SetActive (false);
		twoTalant.gameObject.SetActive (false);

		buyButton.gameObject.SetActive(true);
		rightAnswer.gameObject.SetActive (false);
		wrongAnswerObject.gameObject.SetActive (false);

		imageLose.SetActive (false);
		imageDraft.SetActive (false);
		imageWin.SetActive (false);

		fightResultPanelObject.SetActive (false);

	}
}
