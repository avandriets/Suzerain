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

	public void SetText(List<Fight> fight, int roundNum){

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


	}

	public void ClosePanel () {
		fightResultPanelObject.SetActive (false);
	}
}