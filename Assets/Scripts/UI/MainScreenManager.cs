using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using Soomla.Store;

public class MainScreenManager : MonoBehaviour {

	private WaitPanel	waitPanel  = null;
	private ErrorPanel	errorPanel = null;
	private WaitOpponentDialog		waitForOpponentPanel	= null;
	private InstructionDialog		instDialog = null;

	UserController 	user_controller = null;
	ScreensManager	screensManager	= null;

	public RectTransform firstRing;
	public RectTransform secondRing;

	public Rose centerElement;

	public Text TextNickName;
	public Text TextRank;

	public Image avatar;

	bool InitMuteState = false;

	SoundManager soundMan = null;
	public Toggle muteButton;

	public static int gameCounts = Constants.adShowsCount;
	public MedalShowDialog	shieldDialog;
	bool firstStart = false;

	InterstitialAd interstitial;
	public GoogleAnalyticsV4 googleAnalytics;

	public GameObject spr1;
	public GameObject spr2;

	void OnEnable(){

		InitUser ();
		StartCoroutine(	RotateRings ());

		if (gameCounts == 0) {

			if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) == 0) {
				
				//if (!Utility.StopCoroutine) {
					RequestInterstitial ();
				//}

			}

			gameCounts = Constants.adShowsCount;
		}

		//centerElement.setDataToRose (null);
		centerElement.toZeroPosition ();
		//centerElement.ShowData ();


		if (!InitMuteState) {
			soundMan = GameObject.Find("MusicManager").GetComponent<SoundManager>();
			InitMuteState = true;
		}

		muteButton.onValueChanged.RemoveAllListeners ();

		SoundManager.InitMuteState (muteButton);

		muteButton.onValueChanged.AddListener ( (value) => {   // you are missing this
			handleCheckbox(value);       // this is just a basic method call within another method
		}   // and this one
		);
	}

	void handleCheckbox(bool value)
	{
		soundMan.OnMuteButtonClick ();
	}

	IEnumerator RotateRings() {

			float startingTime = 0;

			while (gameObject.activeSelf) {

				startingTime += Time.deltaTime;

				spr1.transform.rotation = Quaternion.Euler (0, 0, 25 * startingTime);
				//spr2.transform.rotation = Quaternion.Euler (0, 0, -25 * startingTime);
					
//				firstRing.rotation = Quaternion.Euler (0, 0, 25 * startingTime);
//				secondRing.rotation = Quaternion.Euler (0, 0, -25 * startingTime);

				if (startingTime > 360)
					startingTime = 0;
			
				//Debug.Log (" RotateRings RotateRings RotateRings RotateRings RotateRings !!!!!");
				//yield return new WaitForSeconds(1);
				yield return null;
			}

	}

	public void OnFightClick(){

		// Builder Hit with minimum required Event parameters.
		googleAnalytics.LogEvent(new EventHitBuilder()
			.SetEventCategory("game")
			.SetEventAction("ask for game"));
		
		waitForOpponentPanel = screensManager.ShowWaitOpponentDialog("Вызываю на дуэль",CancelFightByUser);

		OnlineGame ing = OnlineGame.instance;
		ing.AskForFight (CancelFightByServer, ReadyToFight, ErrorFightRequest);
	}
		
	public void CancelFightByUser(){

		OnlineGame ing = OnlineGame.instance;
		ing.CancelFight ();
		SoundManager.ChoosePlayMusic (0);
	}

	public void CancelFightByServer(){

		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString ("@fight_finished_by_server"), ErrorCancelByServer);
		SoundManager.ChoosePlayMusic (0);
	}

	public void ReadyToFight(){
		
		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		screensManager.ShowGameScreen ();
		//SoundManager.ChoosePlayMusic (2);
	}

	public void ErrorFightRequest(){
		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString ("@server_side_error"), ErrorCancelByServer);
		SoundManager.ChoosePlayMusic (0);
	}

	public void ErrorCancelByServer(){
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
		SoundManager.ChoosePlayMusic (0);
	}

	public void onClickClearSettings()
	{
		PlayerPrefs.DeleteAll ();
		Application.Quit ();
	}

	public void InitUser(){

		user_controller	= UserController.instance;
		screensManager	= ScreensManager.instance;

		screensManager.InitLanguage ();

		if (UserController.registered) {
			

			if(!UserController.authenticated){
				
				waitPanel = screensManager.ShowWaitDialog(ScreensManager.LMan.getString ("@connecting"));

				user_controller.LogIn (InitLabels, -1 , -1);
			}else{
				InitLabels(false,"","");
			}

		} else {
			firstStart = true;
			screensManager.ShowRegistrationScreen();			
		}

	}

	public void InitLabels(bool error, string source_error, string error_text){

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		screensManager.InitTranslateList ();
		screensManager.TranslateUI ();

		if (error == false) {
			//Init user interface

			//if (!Utility.StopCoroutine) {
				googleAnalytics.LogScreen ("Main Menu");
				//Builder Hit with all App View parameters (all parameters required):
				googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Main Menu"));
			//}
			
			TextNickName.text	= UserController.currentUser.UserName;

			if (Rose.statList.Count > 0) {
				if (Rose.statList [0].Fights >= Constants.fightsCount) {
					TextRank.text = ScreensManager.LMan.getString (Utility.GetDifference (UserController.currentUser, Rose.statList));	
				} else {
					TextRank.text = "Через " + (Constants.fightsCount - Rose.statList [0].Fights).ToString(); // ScreensManager.LMan.getString (Utility.GetDifference (UserController.currentUser, Rose.statList));	
				}
			}

			string shieldNumber = Utility.getNumberOfShield (UserController.currentUser, Rose.statList);

			if (!Utility.ShieldIsOwned (shieldNumber)) {
				shieldDialog.InitDialog (UserController.currentUser, Rose.statList, shielClosedDialog);
			}

			if (firstStart) {
				//instDialog.ShowDialog ();
				instDialog = screensManager.ShowInstructionDialog(closeInstruction);
				firstStart = false;
			}

			//TextRank.text = Utility.getDcumentsPath ();
			Utility.setAvatar (avatar, UserController.currentUser, Rose.statList);

			if (Rose.statList [0].Fights >= Constants.fightsCount) {				
				centerElement.ShowData ();
			}

		} else {
			if(source_error == Constants.LOGIN_ERROR){				
				errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString ("@connection_error"), OnErrorButtonClick);
			}else{
				errorPanel = screensManager.ShowErrorDialog(error_text, OnErrorButtonClick);
			}
		}
	}

	public void closeInstruction(){
		screensManager.CloseInstructionPanel (instDialog);
		instDialog = null;
	}

	public void shielClosedDialog(){
	
	}

	public void OnErrorButtonClick(){

		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;

		//if (!UserController.registered) {
			Application.Quit();
		//}

	}

	void Update(){

//		if (gameObject.activeSelf) {
//
//			startingTime1 += Time.deltaTime;
//
//			firstRing.rotation  = Quaternion.Euler(0,0,25*startingTime1);
//			secondRing.rotation = Quaternion.Euler(0,0,-25*startingTime1);
//
//			if (startingTime1 > 360)
//				startingTime1 = 0;
//		}

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

	private void RequestInterstitial()
	{
		if (interstitial != null) {
			interstitial.Destroy ();
			interstitial = null;
		}
		
		#if UNITY_ANDROID
		string adUnitId = Constants.FULL_SIZE_BANNER_ID_KEY_ANDROID;
		#elif UNITY_IPHONE
		string adUnitId = Constants.FULL_SIZE_BANNER_ID_KEY_IOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);

		interstitial.OnAdLoaded += HandleOnAdLoaded;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder()
		.TagForChildDirectedTreatment(true)
		.Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}

	public void HandleOnAdLoaded(object sender, System.EventArgs args)
	{
		Debug.Log ("AdMOB big was loaded");
		// Handle the ad loaded event.

		if (interstitial.IsLoaded ()) {
			interstitial.Show ();
		}
	}
}