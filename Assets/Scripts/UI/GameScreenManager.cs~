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

	public GameTypeOneTwo		gameTypeOneTwo;
	public GameType2			gameType2;
	public GameType3 			gameType3;
	public GameType4			gameType4;
	public GameType5 			gameType5;
	public GameType5_1 			gameType5_1;
	public GameType6			gameType6;
	public GameType6_1			gameType6_1;
	public GameType7			gameType7;

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
	
		Utility.setAvatar(You,		UserController.currentUser, Rose.statList);
		Utility.setAvatar(Opponent,	ong.opponent, OnlineGame.statList);

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

			gameTypeOneTwo.gameObject.SetActive (false);
			gameType2.gameObject.SetActive (false);
			gameType3.gameObject.SetActive (false);
			gameType4.gameObject.SetActive (false);
			//gameType5.gameObject.SetActive (false);
			gameType5_1.gameObject.SetActive (false);
			//gameType6.gameObject.SetActive (false);
			gameType6_1.gameObject.SetActive (false);
			gameType7.gameObject.SetActive (false);

			if (ong.currentFight.FightTypeId == 1) {
				gameTypeOneTwo.gameObject.SetActive (true);
				gameTypeOneTwo.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 30);
				gameTypeOneTwo.StartGame ();
			} else if (ong.currentFight.FightTypeId == 2) {
				gameType2.gameObject.SetActive (true);
				gameType2.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 60);
				gameType2.StartGame ();
			} else if (ong.currentFight.FightTypeId == 3) {
				gameType3.gameObject.SetActive (true);
				gameType3.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 60);
				gameType3.StartGame ();
			} else if (ong.currentFight.FightTypeId == 4) {
				gameType4.gameObject.SetActive (true);
				gameType4.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 20);
				gameType4.StartGame ();
			} else if (ong.currentFight.FightTypeId == 5) {
//			gameType5.gameObject.SetActive (true);
//			gameType5.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 20);
//			gameType5.StartGame ();
				gameType5_1.gameObject.SetActive (true);
				gameType5_1.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 40);
				gameType5_1.StartGame ();
			} else if (ong.currentFight.FightTypeId == 6) {
//			gameType6.gameObject.SetActive (true);
//			gameType6.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 20);
//			gameType6.StartGame ();
				gameType6_1.gameObject.SetActive (true);
				gameType6_1.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 30);
				gameType6_1.StartGame ();

			} else if (ong.currentFight.FightTypeId == 7) {
				gameType7.gameObject.SetActive (true);
				gameType7.InitGameObject (ong.tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 30);
				gameType7.StartGame ();
			}
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

		GameObject.Find ("MusicManager").GetComponent<SoundManager> ().musicSource.loop = false;

		if (ong.currentFight.IsDraw == true) {
			SoundManager.ChoosePlayMusic (Constants.SoundDraft);
		} else if (ong.currentFight.Winner == UserController.currentUser.Id) {
			SoundManager.ChoosePlayMusic (Constants.SoundWin);
		} else {
			SoundManager.ChoosePlayMusic (Constants.SoundLose);
		}

		// Dont show fight result description if we have one fight
		if (ong.tasksList.Count > 1) {
			
			if (ong.currentFight.IsDraw == true) {
				fightResultDialog.SetText ("", 0, finishGame, ong.fightsList, "", "");
			} else if (ong.currentFight.Winner == UserController.currentUser.Id) {
				fightResultDialog.SetText ("", 1, finishGame, ong.fightsList, "", "");
			} else {
				fightResultDialog.SetText ("", -1, finishGame, ong.fightsList, "", "");
			}

		} else {
			
			if (ong.currentFight.IsDraw == true) {			
				fightResultDialog.SetText (
					FightResultDescription.getExtendetDraftDescription (ong.fightsList[0]), 
					0, 
					finishGame, 
					ong.fightsList, 
					FightResultDescription.getScoreBothPlayers(ong.fightsList[ong.fightsList.Count -1]),
					ong.tasksList[ong.fightsList.Count -1].GetRightAnswer()
				);
			} else if (ong.currentFight.Winner == UserController.currentUser.Id) {
				fightResultDialog.SetText (
					FightResultDescription.getExtendetWinDescription (ong.fightsList[0]), 
					1, 
					finishGame, 
					ong.fightsList, 
					FightResultDescription.getScoreBothPlayers(ong.fightsList[ong.fightsList.Count -1]),
					ong.tasksList[ong.fightsList.Count -1].GetRightAnswer()
				);
			} else {
				fightResultDialog.SetText (
					FightResultDescription.getExtendetLoseDescription (ong.fightsList[0]),
					-1, 
					finishGame, 
					ong.fightsList, 
					FightResultDescription.getScoreBothPlayers(ong.fightsList[ong.fightsList.Count -1]),
					ong.tasksList[ong.fightsList.Count -1].GetRightAnswer()
				);
			}

		}
	}

	public void finishGame(){

		UserController.authenticated = false;
		GameObject.Find ("MusicManager").GetComponent<SoundManager> ().musicSource.loop = true;
		SoundManager.ChoosePlayMusic (Constants.SounrMainTheme);

		if (ong.currentFight.FightTypeId == 1 ) {
			gameTypeOneTwo.gameObject.SetActive (false);

		}else if (ong.currentFight.FightTypeId == 2) {
			gameType2.gameObject.SetActive (false);

		}else if (ong.currentFight.FightTypeId == 3) {
			gameType3.gameObject.SetActive (false);

		}else if (ong.currentFight.FightTypeId == 4) {
			gameType4.gameObject.SetActive (false);

		}else if (ong.currentFight.FightTypeId == 5) {
			//gameType5.gameObject.SetActive (false);
			gameType5_1.gameObject.SetActive (false);

		}else if (ong.currentFight.FightTypeId == 6) {
			gameType6.gameObject.SetActive (false);

		}else if (ong.currentFight.FightTypeId == 7) {
			gameType7.gameObject.SetActive (false);
		}

		MainScreenManager.gameCounts -= 1;

		if (errorPanel != null) {
			GameObject.Destroy (errorPanel.gameObject);
			errorPanel = null;
		}

		if(!ong.mFightWithFriend || (ong.mFightWithFriend && ong.mFriendsFightOpponent))
			screenManager.ShowMainScreen ();
		else
			screenManager.ShowFriendsScreen ();
		
	}

}
