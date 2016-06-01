using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using Soomla.Store;
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

	public InvitationDialog invitationDlg;

	void OnEnable ()
	{
		
		googleAnalytics = GameObject.Find ("GAv4").GetComponent<GoogleAnalyticsV4> ();
		googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Main Menu"));

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

		return;

		//TODO fight choice
//		if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) != 0 || Utility.TESTING_MODE || pFightType > 0) {
//			waitForOpponentPanel = screensManager.ShowWaitOpponentDialog ("Вызываю на дуэль", CancelFightByUser);
//
//			OnlineGame ing = OnlineGame.instance;
//			ing.AskForFight (CancelFightByServer, ReadyToFight, ErrorFightRequest, pFightType);
//		}
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
		}else if (pFightType == -5) {
			errorPanel = screensManager.ShowErrorDialog ("Сезон тренировок для подготовки к «Летнему турниру Сюзерена» начнется в июне 2016 года", ErrorCancelByServer);
		}else if (pFightType == -6) {
			errorPanel = screensManager.ShowErrorDialog ("Ближайший «Летний турнир Сюзерена» начнется в июле 2016 года", ErrorCancelByServer);
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

					if (TextRank.text.Length == 0) {
						TextRank.text = Rose.statList [0].Fights.ToString ();
					}

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

			int showInvitation = 1;
			if (PlayerPrefs.HasKey ("showInvitation")) {
				showInvitation = 0;
			}
			else {
				PlayerPrefs.SetInt ("showInvitation", 1);
				invitationDlg.ShowDialog ();
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

			if (UserController.currentUser.SysMessage != null && UserController.currentUser.SysMessage.Length > 0) {
				string sysMessage = UserController.currentUser.SysMessage;
				UserController.currentUser.SysMessage = "";

				errorPanel = screensManager.ShowErrorDialog (sysMessage, ErrorCancelByServer);
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

}