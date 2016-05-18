using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using Soomla.Store;
using Facebook.Unity;


public class MainScreenManager : MonoBehaviour
{
	private WaitPanel	waitPanel = null;
	private ErrorPanel	errorPanel = null;
	private WaitOpponentDialog waitForOpponentPanel	= null;
	private InstructionDialog instDialog = null;

	UserController user_controller = null;
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
	public RankDialog		rankDialog;
	bool firstStart = false;

	InterstitialAd interstitial;
	public static GoogleAnalyticsV4 googleAnalytics;

	public GameObject spr1;
	public GameObject spr2;

	private int CurrentTypeGame = Constants.RANDOM_GAME;
	public Text GameTypeCaption;

	BannerView bannerView = null;

	public LiveMessenger	messengerLive;

	public AcceptFightWithFriend mAcceptFightDialogWithFriend;

	public GameObject ProRing;
	public GameObject UsualRing;

	int currentFightId = -1;
	public InitSocialNetworks	initNetwork;
	public FightsPanel			mFightPanel;

	void Start ()
	{

		//initNetwork.InitNetworks ();
		//Turn off banners show
		if (false) {
			if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) == 0) {
				RequestBanner ();
			}
		}

	}

	private void RequestBanner ()
	{
		#if UNITY_ANDROID
		string adUnitId = Constants.BANNER_ID_KEY_ANDROID;
		#elif UNITY_IPHONE
		string adUnitId = Constants.BANNER_ID_KEY_IOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif


		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Top);

		bannerView.OnAdLoaded += HandleAdLoaded;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ()
		.TagForChildDirectedTreatment (true)
		.Build ();

		// Load the banner with the request.
		bannerView.LoadAd (request);
	}

	public void HandleAdLoaded (object sender, System.EventArgs args)
	{
		bannerView.Show ();
	}

	void OnEnable ()
	{

//		if (!initNetwork.WasInit) {
//			initNetwork.InitNetworks ();
//		}

		googleAnalytics = GameObject.Find ("GAv4").GetComponent<GoogleAnalyticsV4> ();
		googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Main Menu"));

		InitUser ();
		StartCoroutine (RotateRings ());

		if (gameCounts == 0) {

			if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) == 0 && !Utility.hide_ad) {				
				RequestInterstitial ();
			}

			gameCounts = Constants.adShowsCount;
		}

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

		//Turn off banner show
		if (false) {
			if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) == 0) {
				RequestBanner ();
			}
		}
	}

	void OnDisable ()
	{
		if (bannerView != null) {
			bannerView.Hide ();
			bannerView.Destroy ();
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
		if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) != 0 || Utility.TESTING_MODE || pFightType > 0) {
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

			if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) != 0 || Utility.TESTING_MODE || pFightType == 0) {
				waitForOpponentPanel = screensManager.ShowWaitOpponentDialog ("Вызываю на дуэль", CancelFightByUser);

				OnlineGame ing = OnlineGame.instance;
				ing.AskForFight (CancelFightByServer, ReadyToFight, ErrorFightRequest, pFightType);

			} else {

				errorPanel = screensManager.ShowErrorDialog ("Выбор поединка доступен в PREMIUM версии игры.", ErrorCancelByServer);

			}

		} else if (pFightType == -1) {
			screensManager.ShowFriendsScreen ();
		}

	}

	public void CancelFightByUser ()
	{

		OnlineGame ing = OnlineGame.instance;
		ing.CancelFight ();
		SoundManager.ChoosePlayMusic (0);
	}

	public void CancelFightByServer (Fight pfight)
	{

		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		errorPanel = screensManager.ShowErrorDialog (ScreensManager.LMan.getString ("@fight_finished_by_server"), ErrorCancelByServer);
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

	public void ErrorFightRequest ()
	{
		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		errorPanel = screensManager.ShowErrorDialog (ScreensManager.LMan.getString ("@server_side_error"), ErrorCancelByServer);
		SoundManager.ChoosePlayMusic (0);
	}

	public void ErrorCancelByServer ()
	{
		GameObject.Destroy (errorPanel.gameObject);
		errorPanel = null;
		SoundManager.ChoosePlayMusic (0);
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

	public void InitLabels (bool error, string source_error, string error_text)
	{

		if (waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		screensManager.InitTranslateList ();
		screensManager.TranslateUI ();

		if (error == false) {

			messengerLive.StartPulseRequest ();


			TextNickName.text	= UserController.currentUser.UserName;

			if (Rose.statList.Count > 0) {
				if (Rose.statList [0].Fights >= Constants.fightsCount) {
					TextRank.text = ScreensManager.LMan.getString (Utility.GetDifference (UserController.currentUser, Rose.statList));	
				} else {
					TextRank.text = "Через " + (Constants.fightsCount - Rose.statList [0].Fights).ToString ();
				}
			}


			string shieldNumber = Utility.getNumberOfShield (Rose.statList).shieldNumber;
			if (!Utility.ShieldIsOwned (shieldNumber)) {
				shieldDialog.InitDialog (UserController.currentUser, Rose.statList, shielClosedDialog);
			}

			if (shieldNumber != "1") {
				string differenceNumber = Utility.GetDifference (UserController.currentUser, Rose.statList);
				if (!Utility.DifferenceIsOwned (differenceNumber)) {
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
			if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) != 0 || Utility.TESTING_MODE) {
				ProRing.SetActive (true);
				UsualRing.SetActive (false);

				//eagle.gameObject.SetActive (true);
				//Utility.setEagle (eagle, Rose.statList);
			}

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

	private void RequestInterstitial ()
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
		interstitial = new InterstitialAd (adUnitId);

		interstitial.OnAdLoaded += HandleOnAdLoaded;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ()
		.TagForChildDirectedTreatment (true)
		.Build ();
		// Load the interstitial with the request.
		interstitial.LoadAd (request);
	}

	public void HandleOnAdLoaded (object sender, System.EventArgs args)
	{
		Debug.Log ("AdMOB big was loaded");
		// Handle the ad loaded event.

		if (interstitial.IsLoaded ()) {
			interstitial.Show ();
		}
	}

	public void AskForFightFromFriend (int fightId)
	{

		currentFightId = fightId;
		OnlineGame ing = OnlineGame.instance;

		ing.InitGameParameters (true, true);

		StartCoroutine (ing.StateFightRequestWithFriendByID (fightId, CancelFightByServer, ReadyToFightWithFriend, ErrorFightRequest));

	}

	public void ReadyToFightWithFriend ()
	{
		mAcceptFightDialogWithFriend.ShowDialog (AcceptFightWithFriend, CancelFightWithFriend);		
	}

	public void AcceptFightWithFriend ()
	{

		waitPanel = screensManager.ShowWaitDialog (ScreensManager.LMan.getString ("@connecting"));
		OnlineGame ing = OnlineGame.instance;
		StartCoroutine (ing.AcceptFightWithFriendByID (currentFightId, CancelFightByServer, IntoFight, ErrorFightRequest));		
	}

	public void IntoFight ()
	{

		if (waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		screensManager.ShowGameScreen ();
	}

	public void CancelFightWithFriend ()
	{

		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		OnlineGame ing = OnlineGame.instance;
		ing.CancelFightWithFriend ();
	}

}