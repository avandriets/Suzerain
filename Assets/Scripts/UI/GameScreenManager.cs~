using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;

public delegate void RefreshGameScreen();

public class GameScreenManager : MonoBehaviour {

	public ClockObject 			clock;
	public OpponentIntroduce 	introducePanel;

	public Image 	You;
	public Image 	Opponent;
	public Text 	YouName;
	public Text 	OpponentName;
	public Text 	FightTypeName;

	OnlineGame 					ong;
	ScreensManager 				screenManager;

	private GameBase currentGameTemplate;

	public Transform gameContainer;

	public FightResultDialog 	fightResultDialog;
	public List<Image>			fightsImageList;

	Sprite spriteDraft	= null;
	Sprite spriteWin	= null;
	Sprite spriteLose	= null;
	Sprite spriteDefault	= null;

	private ErrorPanel	errorPanel = null;

	ScreensManager	screensManager	= null;
	bool fightStart = false;

	private void showRoundsResults(){
		
		foreach(var currentFight in ong.fightsList){
			if (currentFight.IsDraw == true) {
				fightsImageList[ong.fightsList.IndexOf(currentFight)].sprite = spriteDraft;
			} else if (currentFight.Winner == UserController.currentUser.Id) {
				fightsImageList[ong.fightsList.IndexOf(currentFight)].sprite = spriteWin;
			} else {
				fightsImageList[ong.fightsList.IndexOf(currentFight)].sprite = spriteLose;
			}
		}
	}

	void OnEnable(){
		
		screensManager	= ScreensManager.instance;

		MainScreenManager.googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Game screen"));

		Texture2D texture = null;

		if (spriteDraft == null) {
			texture = Resources.Load ("draft_sign") as Texture2D;
			if (texture != null) {
				spriteDraft = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
			}
		}

		if (spriteWin == null) {
			texture = Resources.Load ("win_sign") as Texture2D;
			if (texture != null) {
				spriteWin = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
			}
		}

		if (spriteLose == null) {
			texture = Resources.Load ("lose_sign") as Texture2D;
			if (texture != null) {
				spriteLose = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
			}
		}

		if (spriteDefault == null) {
			texture = Resources.Load ("default_sign") as Texture2D;
			if (texture != null) {
				spriteDefault = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
			}
		}

		foreach (var cc in fightsImageList) {
			cc.sprite = spriteDefault;
		}
			
		clock.initClockImages ();
		clock.invertTimer = true;

		fightStart = false;

		ong				= OnlineGame.instance;
		screenManager	= ScreensManager.instance;

		//ong.gameProtocol.SetValue("");

		if (ong.currentFight.FightState != -1) {

			if(!ong.mFightWithFriend || (ong.mFightWithFriend && ong.mFriendsFightOpponent))
				screenManager.ShowMainScreen ();
			else
				screenManager.ShowFriendsScreen ();
			
		} else {

			screenManager.InitTranslateList ();
			screenManager.TranslateUI ();

			InitOpponents ();

			StartCoroutine (AutoStartFight());

			introducePanel.IntroduceOpponent (StartFight);

			if (ong.currentFight.FightTypeId == 1 || ong.currentFight.FightTypeId == 2 || ong.currentFight.FightTypeId == 3) {
				SoundManager.ChoosePlayMusic (Constants.SoundFight123);
			} else {
				SoundManager.ChoosePlayMusic (Constants.SoundFight4567);
			}
		}
	}

	IEnumerator AutoStartFight(){
		
		yield return new WaitForSeconds (5);

		if (!fightStart) {
			introducePanel.ClosePanel ();
			StartFight ();
		}
	}

	public void InitOpponents(){
	
		Utility.setAvatar(You,		Rose.statList);
		Utility.setAvatar(Opponent,	OnlineGame.statList);

		YouName.text = UserController.currentUser.UserName;
		OpponentName.text = ong.opponent.UserName;
		FightTypeName.text = ScreensManager.LMan.getString (Utility.FightTypeName(ong.currentFight.FightTypeId));

	}

	public void StartFight(){

		if (ong.tasksList.Count == 0) {
			errorPanel = screensManager.ShowErrorDialog ("Ошибка вызо", finishGame);
			finishGame ();
		} else {

			fightStart = true;

			Debug.Log ("Time: " + System.DateTime.Now.ToString ());
			Debug.Log ("Start fight");
			Debug.Log ("User: " + UserController.currentUser.UserName);
			Debug.Log ("Userid: " + UserController.currentUser.Id.ToString ());
			Debug.Log ("FightTypeId: " + ong.currentFight.FightTypeId);
			Debug.Log ("fightID: " + ong.currentFight.Id);

			if (currentGameTemplate != null) {
				GameObject.Destroy (currentGameTemplate.gameObject);
				currentGameTemplate = null;
			}

			currentGameTemplate = screenManager.GetGameByNumber (gameContainer, ong.currentFight.FightTypeId);

			currentGameTemplate.gameObject.SetActive (true);
			currentGameTemplate.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, Utility.gameDurationArray[ong.currentFight.FightTypeId-1]);
			currentGameTemplate.StartGame ();
		}

	}

	public void RoundResult(List<TaskAnswer>	answerList, StartNextRoundDelegate nextRoundDelegate){
		ong.answerList = answerList;
		StartCoroutine (ong.SentSINGLEAnswersToServer(nextRoundDelegate, finishGame));
	}

	public void OnFinishGame(List<TaskAnswer> 	answerList){

		Debug.Log ("Time: " + System.DateTime.Now.ToString());
		Debug.Log ("Finish game");

		ong.answerList = answerList;

		StartCoroutine(ong.SentAnswersInQueeThread (ShowFinalScreen, finishGame));
	}

	private void ShowFinalScreen(){

		string template = "Вы получили {0} балл{1} {2}";


		GameObject.Find ("MusicManager").GetComponent<SoundManager> ().musicSource.loop = false;

		if (ong.currentFight.IsDraw == true) {
			SoundManager.ChoosePlayMusic (Constants.SoundDraft);
			template = string.Format (template, ong.currentFight.WinnerScore,"", "");
		} else if (ong.currentFight.Winner == UserController.currentUser.Id) {
			if (Utility.hasSubscription ()) {
				Storage.GiveMoney (2);
				template = string.Format (template, ong.currentFight.WinnerScore, "а", " + 2 таланта");
			} else {
				Storage.GiveMoney (1);
				template = string.Format (template, ong.currentFight.WinnerScore, "а", " + 1 талант");
			}
			SoundManager.ChoosePlayMusic (Constants.SoundWin);

		} else {
			SoundManager.ChoosePlayMusic (Constants.SoundLose);
			template = string.Format (template, ong.currentFight.LooserScore, "ов", "");
		}

		// Dont show fight result description if we have one fight
		if (ong.tasksList.Count > 1) {

			string rightAnswer = "";
			foreach (var ii in ong.tasksList) {
				rightAnswer += ii.GetRightAnswer () + "\n";
			}

			if (ong.currentFight.IsDraw == true) {
				fightResultDialog.SetText ("", 0, finishGame, ong.fightsList, template, rightAnswer);
			} else if (ong.currentFight.Winner == UserController.currentUser.Id) {
				fightResultDialog.SetText ("", 1, finishGame, ong.fightsList, template, rightAnswer);
			} else {
				fightResultDialog.SetText ("", -1, finishGame, ong.fightsList, template, rightAnswer);
			}

		} else {
			
			if (ong.currentFight.IsDraw == true) {
				fightResultDialog.SetText (
					FightResultDescription.getExtendetDraftDescription (ong.fightsList[0]), 
					0, 
					finishGame, 
					ong.fightsList, 
					template,//FightResultDescription.getScoreBothPlayers(ong.fightsList[ong.fightsList.Count -1]),
					ong.tasksList[ong.fightsList.Count -1].GetRightAnswer()
				);
			} else if (ong.currentFight.Winner == UserController.currentUser.Id) {
				fightResultDialog.SetText (
					FightResultDescription.getExtendetWinDescription (ong.fightsList[0]), 
					1, 
					finishGame, 
					ong.fightsList, 
					template,//FightResultDescription.getScoreBothPlayers(ong.fightsList[ong.fightsList.Count -1]),
					ong.tasksList[ong.fightsList.Count -1].GetRightAnswer()
				);
			} else {
				fightResultDialog.SetText (
					FightResultDescription.getExtendetLoseDescription (ong.fightsList[0]),
					-1, 
					finishGame, 
					ong.fightsList, 
					template,//FightResultDescription.getScoreBothPlayers(ong.fightsList[ong.fightsList.Count -1]),
					ong.tasksList[ong.fightsList.Count -1].GetRightAnswer()
				);
			}

		}
	}

	public void finishGame(){

		//UserController.authenticated = false;
		UserController.reNewStatistic = true;

		GameObject.Find ("MusicManager").GetComponent<SoundManager> ().musicSource.loop = true;
		SoundManager.ChoosePlayMusic (Constants.SounrMainTheme);


		GameObject.Destroy (currentGameTemplate.gameObject);
		currentGameTemplate = null;

		if (errorPanel != null) {
			GameObject.Destroy (errorPanel.gameObject);
			errorPanel = null;
		}

		if (!ong.mFightWithFriend || (ong.mFightWithFriend && ong.mFriendsFightOpponent)) {			

			if (screenManager.mStartScreenCanvas != null) {

				if (screenManager.mStartScreenCanvas == screenManager.mFriendsScreenCanvas) {
					screenManager.ShowFriendsScreen ();
				} else if (screenManager.mStartScreenCanvas == screenManager.mProfileScreenCanvas) {
					screenManager.ShowProfileScreen ();
				} else if (screenManager.mStartScreenCanvas == screenManager.mAchivmentScreenCanvas) {
					screenManager.ShowAchivmentsScreen ();
				} else {
					screenManager.ShowMainScreen ();
				}

			}else{
				screenManager.ShowMainScreen ();
			}
				
			screenManager.mStartScreenCanvas = null;
		}
		else
			screenManager.ShowFriendsScreen ();

		ong.currentFight = null;
	}

}
