using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Facebook.Unity;


public class MainScreenManager : BaseUIClass
{

	UserController user_controller = null;
	bool firstStart = false;

	public RectTransform firstRing;
	public RectTransform secondRing;

	public Rose centerElement;

	public Text TextNickName;
	public Text TextRank;

	public Image avatar;

	bool InitMuteState = false;

	SoundManager soundMan = null;
	public Toggle muteButton;

	public MedalShowDialog	shieldDialog;
	public RankDialog		rankDialog;
	public static GoogleAnalyticsV4 googleAnalytics;
	public GameObject spr1;
	public GameObject spr2;

	public Text GameTypeCaption;

	public LiveMessenger	messengerLive;

	public GameObject ProRing;
	public GameObject UsualRing;

	public InitSocialNetworks	initNetwork;
	public FightsPanel			mFightPanel;

	protected InstructionDialog 	instDialog 				= null;

	public Image eagle;

	public NewEagleDialog newEagleDialog;

	void OnEnable ()
	{

		Debug.Log ("Analitycs.InitUser Analitycs.");
		googleAnalytics = GameObject.Find ("GAv4").GetComponent<GoogleAnalyticsV4> ();
		Debug.Log ("Analitycs.Found component.");
		googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Main Menu"));
		Debug.Log ("Analitycs. Send log.");

		InitUser ();
		StartCoroutine (RotateRings ());

		centerElement.toZeroPosition ();


		if (!InitMuteState) {
			soundMan = GameObject.Find ("MusicManager").GetComponent<SoundManager> ();
			InitMuteState = true;
		}

		muteButton.onValueChanged.RemoveAllListeners ();

		SoundManager.InitMuteState (muteButton);

		muteButton.onValueChanged.AddListener ((value) => {   // you are missing this
			handleCheckbox (value);       // this is just a basic method call within another method
		}   // and this one
		);

		purchaseVisualObject.changeSubscriptionDelegate += SubscriptionChange;
	}

	private void SubscriptionChange(){

		//Show ring
		if (Utility.TESTING_MODE || Utility.hasSubscription()) {
			ProRing.SetActive (true);
			UsualRing.SetActive (false);

			InitVisualComponents ();

//			eagle.gameObject.SetActive (true);
//			Utility.setEagle (eagle, Rose.statList);
		}
	}

	void handleCheckbox (bool value)
	{
		soundMan.OnMuteButtonClick ();
	}

	IEnumerator RotateRings ()
	{

		float startingTime = 0;

		while (gameObject.activeSelf) {

			startingTime += Time.deltaTime;
			spr1.transform.rotation = Quaternion.Euler (0, 0, 25 * startingTime);

			if (startingTime > 360)
				startingTime = 0;
			
			yield return null;
		}
	}

	public void OnFightClick (int pFightType)
	{

		if (Utility.TESTING_MODE || Utility.hasSubscription()) {
			waitForOpponentPanel = screensManager.ShowWaitOpponentDialog ("Вызываю на дуэль", CancelFightByUser);

			OnlineGame ing = OnlineGame.instance;
			ing.AskForFight (CancelFightByServer, ReadyToFight, ErrorFightRequest, pFightType);
		}
	}

	public void MainButtonFightClick(){
		mFightPanel.SetText (fightFromPanel);
	}


	public void fightFromPanel(int pFightType){

		if (pFightType >= 0) {

			if (Utility.TESTING_MODE || pFightType == 0 || Utility.hasSubscription()) {
				waitForOpponentPanel = screensManager.ShowWaitOpponentDialog ("Вызываю на дуэль", CancelFightByUser);

				OnlineGame ing = OnlineGame.instance;
				ing.AskForFight (CancelFightByServer, ReadyToFight, ErrorFightRequest, pFightType);

			} else {

				errorPanel = screensManager.ShowErrorDialog ("Выбор поединка доступен в PREMIUM версии игры.", ErrorCancelByServer);

			}

		} else if (pFightType == -1) {
			screensManager.ShowFriendsScreen ();
		}else if (pFightType == -5) {
			screensManager.ShowTrainingScreen ();
		}

	}

	public void CancelFightByUser ()
	{

		OnlineGame ing = OnlineGame.instance;
		ing.CancelFight ();
		SoundManager.ChoosePlayMusic (0);
	}

	public void ReadyToFight ()
	{
		
		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		screensManager.ShowGameScreen ();
	}
		
	public void onClickClearSettings ()
	{
		PlayerPrefs.DeleteAll ();
		Application.Quit ();
	}

	public void InitUser ()
	{

		user_controller	= UserController.instance;
		screensManager	= ScreensManager.instance;

		screensManager.InitLanguage ();

		if (UserController.registered) {
			

		if (!UserController.authenticated || UserController.reNewStatistic) {
				
				waitPanel = screensManager.ShowWaitDialog (ScreensManager.LMan.getString ("@connecting"));

				user_controller.LogIn (InitLabels, -1, -1);
			} else {
				InitLabels (false, "", "");
			}

		} else {
			firstStart = true;
			screensManager.ShowRegistrationScreen ();			
		}

	}

	public void InitVisualComponents(){
	
		purchaseVisualObject.initSubAndCurrency ();

		messengerLive.StartPulseRequest ();


		TextNickName.text	= UserController.currentUser.UserName;

		if (Rose.statList.Count > 0) {
			if (Rose.statList [0].Fights >= Constants.fightsCount) {
				TextRank.text = ScreensManager.LMan.getString (Utility.GetDifference (UserController.currentUser, Rose.statList));

				if (TextRank.text.Length == 0) {
					TextRank.text = Rose.statList [0].Fights.ToString ();
				}

			} else {
				TextRank.text = "Через " + (Constants.fightsCount - Rose.statList [0].Fights).ToString ();
			}
		}


		string shieldNumber = Utility.getNumberOfShield (Rose.statList).shieldNumber;
		if (!Utility.ItemsIsOwned (shieldNumber, "shields", this)) {
			shieldDialog.InitDialog (UserController.currentUser, Rose.statList, shielClosedDialog);
		}

		if (shieldNumber != "1") {
			string differenceNumber = Utility.GetDifference (UserController.currentUser, Rose.statList);
			if (!Utility.ItemsIsOwned (differenceNumber, "difference", this)) {
				if (differenceNumber != "") {
					rankDialog.InitDialog (UserController.currentUser, Rose.statList, shielClosedDialog);
				}
			}
		}

		if (firstStart) {

			instDialog = screensManager.ShowInstructionDialog (closeInstruction);

			firstStart = false;
		}

		Utility.setAvatar (avatar, Rose.statList);

		if (Rose.statList [0].Fights >= Constants.fightsCount) {				
			centerElement.ShowData ();
		}

		//Show ring
		if (Utility.TESTING_MODE || Utility.hasSubscription()) {
			ProRing.SetActive (true);
			UsualRing.SetActive (false);

			if (shieldNumber != "1" /*&& shieldNumber != "2"*/) {

				var eagleCur = EaglsManager.getEagl ( Rose.statList);
				if (eagleCur != null) {

					eagle.gameObject.SetActive (true);
					Utility.setEagle (eagle, Rose.statList);

					if (!Utility.ItemsIsOwned (eagleCur.eagleNumber, "eagles", this)) {							
						newEagleDialog.InitDialog (UserController.currentUser, Rose.statList, shielClosedDialog);
					}
				}
			}
		}

		if (UserController.currentUser.SysMessage != null && UserController.currentUser.SysMessage.Length > 0) {
			string sysMessage = UserController.currentUser.SysMessage;
			UserController.currentUser.SysMessage = "";

			errorPanel = screensManager.ShowErrorDialog (sysMessage, ErrorCancelByServer);
		}

	}

	public void InitLabels (bool error, string source_error, string error_text)
	{

		if (waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		screensManager.InitTranslateList ();
		screensManager.TranslateUI ();

		if (error == false) {

			InitVisualComponents ();

		} else {
			if (source_error == Constants.LOGIN_ERROR) {				
				errorPanel = screensManager.ShowErrorDialog (ScreensManager.LMan.getString ("@connection_error"), OnErrorButtonClick);
			} else {
				errorPanel = screensManager.ShowErrorDialog (error_text, OnErrorButtonClick);
			}
		}
	}

	public void closeInstruction ()
	{
		screensManager.CloseInstructionPanel (instDialog);
		instDialog = null;
	}

	public void shielClosedDialog ()
	{
	
	}

	public void OnErrorButtonClick ()
	{
		GameObject.Destroy (errorPanel.gameObject);
		errorPanel = null;

		Application.Quit ();
	}

	void Update ()
	{

		if (Input.GetKey (KeyCode.Escape)) {
			Debug.Log ("Click exit action");

			Application.Quit ();
			#if UNITY_STANDALONE
			//Quit the application
			Application.Quit();
			#endif

			//If we are running in the editor
			#if UNITY_EDITOR
			//Stop playing the scene
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
		}
	}

	public void SaveUser()
	{

		//waitPanel = screensManager.ShowWaitDialog(ScreensManager.LMan.getString("save_profile_label"));

		Debug.Log ("login to server.");

		var postScoreURL 	= Utility.SERVICE_BASE_URL;		
		var method 			= Utility.EDIT_USER_URL;

		var token 		= "token=";
		var birthDate 	= "birthDate=";
		var motto 		= "motto=";
		var countryId 	= "countryId=";
		var languageId 	= "languageId=";
		var AddressTo = "AddressTo=";
		var SignName = "SignName=";

		var country = 102;
		if (UserController.currentUser.CountryId != 0)
			country = UserController.currentUser.CountryId;

		var lang = 2;
		if (UserController.currentUser.LanguageId != 0)
			lang = UserController.currentUser.LanguageId;

		postScoreURL = 
			postScoreURL + method + "?" 
			+ token + UserController.currentUser.Token + "&"
			+ birthDate + "01/01/0001" + "&"
			+ motto + System.Uri.EscapeUriString (UserController.currentUser.Motto) + "&"
//			+ AddressTo + System.Uri.EscapeUriString (UserController.currentUser.AddressTo) + "&"
//			+ SignName + System.Uri.EscapeUriString (UserController.currentUser.SignName) + "&"
//			+ motto + "" + "&"
//			+ AddressTo + "midved" + "&"
//			+ SignName + "prived" + "&"
			+ countryId + country + "&"
			+ languageId + lang;

		WWWForm form = new WWWForm();
		form.AddField("Content-Type", "text/json");

		var request = new WWW(postScoreURL, form);

		StartCoroutine(WaitForRequest(request));

	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		//screensManager.CloseWaitPanel (waitPanel);

		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);

			//UserController.authenticated = false;
			//screensManager.ShowMainScreen();

		} else {
			//errorPanel = screensManager.ShowErrorDialog(www.error + " " + www.text ,LoginErrorAction);
			Debug.LogError("WWW Error: "+ www.error);
		}    
	}
}