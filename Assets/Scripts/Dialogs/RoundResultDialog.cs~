using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class RoundResultDialog : MonoBehaviour {

	public GameObject 	fightResultPanelObject;
	public Text			actionDescription;
	public Text			roundNumber;
	public List<Image>	fightsImageList;
	public Text			scoreUser;

//	public GameObject	rightAnswerObject;
//	public Text			answerDescription;

	public GameObject	buyButton;
	public Text			rightAnswer;
	public Text			askMoney;
	public GameObject	wrongAnswerObject;

	int fightType;

	public Image oneTalant;
	public Image twoTalant;

	public void SetText(List<Fight> fight, List<TaskAnswer> answers, List<TestTask> 		tasksList){

		int roundNum = answers.Count;
		Sprite spriteDraft	= null;
		Sprite spriteWin	= null;
		Sprite spriteLose	= null;

		Texture2D texture = Resources.Load("draft_sign") as Texture2D;
		if(texture != null){
			spriteDraft = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
		}

		texture = Resources.Load("win_sign") as Texture2D;
		if(texture != null){
			spriteWin = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
		}

		texture = Resources.Load("lose_sign") as Texture2D;
		if(texture != null){
			spriteLose = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
		}

		fightResultPanelObject.SetActive (true);

		//Init
		actionDescription.text = "";

		foreach(var currentFight in fight){
			if (currentFight.IsDraw == true) {
				actionDescription.text = System.Text.RegularExpressions.Regex.Unescape(FightResultDescription.getExtendetDraftDescription (currentFight));
				fightsImageList[fight.IndexOf(currentFight)].sprite = spriteDraft;
			} else if (currentFight.Winner == UserController.currentUser.Id) {
				actionDescription.text = System.Text.RegularExpressions.Regex.Unescape(FightResultDescription.getExtendetWinDescription (currentFight));
				fightsImageList[fight.IndexOf(currentFight)].sprite = spriteWin;
			} else {
				actionDescription.text = System.Text.RegularExpressions.Regex.Unescape(FightResultDescription.getExtendetLoseDescription (currentFight));
				fightsImageList[fight.IndexOf(currentFight)].sprite = spriteLose;
			}
		}

		//actionDescription.text = System.Text.RegularExpressions.Regex.Unescape(FightResultDescription.getExtendetLoseDescription (fight[fight.Count-1]));
			
		scoreUser.text = FightResultDescription.getScoreBothPlayers (fight[fight.Count-1]);

		roundNumber.text = roundNum.ToString ();

		GameObject.Find("TextRound").GetComponent<Text>().text = ScreensManager.LMan.getString ("@round");

//		//Show right answer
//		if(/*StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) != 0 || Utility.TESTING_MODE 
//			&& */ (fight[fight.Count-1].FightTypeId == 1 || fight[fight.Count-1].FightTypeId == 2 || fight[fight.Count-1].FightTypeId == 3 
//			|| fight[fight.Count-1].FightTypeId == 5)){
//			//rightAnswerObject.SetActive (true);
//			answerDescription.text = tasksList[answers.Count-1].GetRightAnswer();
//		}

		fightType = fight [0].FightTypeId;
		bool showRightAnswer = false;

		if (answers [answers.Count - 1].Answer < 0) {
			showRightAnswer = true;
		}

		rightAnswer.text = tasksList[answers.Count-1].GetRightAnswer();

		if (	(fight [0].FightTypeId != Utility.Intuition &&
				fight [0].FightTypeId != Utility.Reflex &&
				fight [0].FightTypeId != Utility.Iridizia)) {

			if (showRightAnswer) {
				if (fight [0].FightTypeId == Utility.Mudrost || fight [0].FightTypeId == Utility.Logic || fight [0].FightTypeId == Utility.Razum) {
					askMoney.text = string.Format ("открыть за {0} таланта", 2);
					twoTalant.gameObject.SetActive (true);
				} else {
					oneTalant.gameObject.SetActive (true);
					askMoney.text = string.Format ("открыть за {0} талант", 1);
				}

				buyButton.gameObject.SetActive (true);
				wrongAnswerObject.gameObject.SetActive (true);
			}
		}

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

	public void ClosePanel () {

		twoTalant.gameObject.SetActive (false);
		oneTalant.gameObject.SetActive (false);

		buyButton.gameObject.SetActive(true);
		rightAnswer.gameObject.SetActive (false);
		wrongAnswerObject.gameObject.SetActive (false);

		//rightAnswerObject.SetActive (false);
		fightResultPanelObject.SetActive (false);
	}
}