using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;
using System.IO;
using Soomla.Store;
#if UNITY_ANDROID
using GooglePlayGames;
#endif

public class ProfileManager : BaseUIClass {

	public Text			nicname;
	public Toggle		toggleRussian;
	public Toggle		toggleEnglish;

	public Slider 		soundSlider;

	public SendMessage 		pSendMessage;
	private SendMessage		localSendMessage = null;

	private InstructionDialog	instDialog = null;


	bool wasShown = false;

	public Text TextRank;
	public Image avatar;


	// Use this for initialization
	void OnEnable() {
	
		//wasGetEmail = false;
		MainScreenManager.googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Profile screen"));

		screensManager = ScreensManager.instance;

		if(PlayerPrefs.HasKey("Volume")){
			soundSlider.value = SetAudioLevels.volumeSize;
		}

		if (!wasShown) {
			
			screensManager.InitTranslateList ();
			wasShown = true;
		}

		InitComponents();

		screensManager.InitLanguage ();
		screensManager.InitTranslateList ();
		screensManager.TranslateUI ();
	}

//	private void InitLang(){
//		
//		var textComponents = gameObject.GetComponentsInChildren (typeof(Text));
//
//		foreach (Text c in textComponents) {
//			if (c.text.Contains ("@")) {
//				c.text = LMan.getString (c.text);
//				Debug.Log (c.text);
//			}
//		}
//	}

	public void OnLanguageChange(){

		if (ScreensManager.LMan != null) {

			if (toggleEnglish.isOn) {
				//ScreensManager.LMan.setLanguage (Path.Combine (Application.dataPath, "lang.xml"), "English");
				ScreensManager.LMan.setLanguageFromRes ("lang", "English");
			} else {			
				//ScreensManager.LMan.setLanguage (Path.Combine (Application.dataPath, "lang.xml"), "Russian");
				ScreensManager.LMan.setLanguageFromRes ("lang", "Russian");
			}
			
			screensManager.TranslateUI ();
		}
	}

	private void InitComponents(){
	
		if (Rose.statList.Count > 0) {
			if (Rose.statList [0].Fights >= Constants.fightsCount) {
				TextRank.text = ScreensManager.LMan.getString (Utility.GetDifference (UserController.currentUser, Rose.statList));	
			} else {
				TextRank.text = "Через " + (Constants.fightsCount - Rose.statList [0].Fights).ToString(); // ScreensManager.LMan.getString (Utility.GetDifference (UserController.currentUser, Rose.statList));	
			}
		}

		Utility.setAvatar(avatar, Rose.statList);

		nicname.text = UserController.currentUser.UserName;
		
		if (UserController.currentUser.LanguageId == 1) {
			toggleEnglish.isOn = true;
			toggleRussian.isOn = false;
		} else {
			toggleEnglish.isOn = false;
			toggleRussian.isOn = true;
		}

	}

	void OnDisable(){

		PlayerPrefs.SetFloat ("Volume",  soundSlider.value);
		SetAudioLevels.volumeSize = soundSlider.value;

	}

	public void OnSaveClick(){
		
		if(toggleEnglish.isOn){
			UserController.currentUser.LanguageId = 1; 
		}else{
			UserController.currentUser.LanguageId = 2; 
		}
		
		SaveUser ();
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
		
		postScoreURL = 
			postScoreURL + method + "?" 
				+ token + UserController.currentUser.Token + "&"
				+ birthDate + "01/01/0001" + "&"
				+ motto + System.Uri.EscapeUriString (UserController.currentUser.Motto) + "&"
				+ AddressTo + System.Uri.EscapeUriString (UserController.currentUser.AddressTo) + "&"
			+ SignName + System.Uri.EscapeUriString (UserController.currentUser.SignName) + "&"
				+ countryId + UserController.currentUser.CountryId + "&"
				+ languageId + UserController.currentUser.LanguageId;

		WWWForm form = new WWWForm();
		form.AddField("Content-Type", "text/json");
		
		var request = new WWW(postScoreURL, form);
		
		StartCoroutine(WaitForRequest(request));
		
	}
	
	public void LoginErrorAction()
	{
		errorPanel.ClosePanel();
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
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
			screensManager.ShowMainScreen();
			
		} else {
			errorPanel = screensManager.ShowErrorDialog(www.error + " " + www.text ,LoginErrorAction);
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	public void OnSendMessageToDevelopers(){
	
		SendMessage NewWaitPanel = GameObject.Instantiate(pSendMessage) as SendMessage;

		NewWaitPanel.transform.SetParent(screensManager.currentScreenCanvas.transform);
		NewWaitPanel.transform.localScale = new Vector3(1,1,1);
		NewWaitPanel.ShowDialog ("", SendMessageOptions, cancelMessageDialog);

		RectTransform rctr = NewWaitPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);

		rctr.anchoredPosition3D = new Vector3(0,0,0);

		localSendMessage = NewWaitPanel;
		//SendMessage.ShowDialog ("", SendMessageOptions);
	}

	public void cancelMessageDialog(){
		localSendMessage.ClosePanel ();
		GameObject.Destroy (localSendMessage.gameObject);
		localSendMessage = null;
	}

	public void SendMessageOptions(string message){
		
		if(message.Length > 0){
			GameProtocol ddd = new GameProtocol ();
			StartCoroutine (ddd.SendMessageToSrv (message));
		}

		localSendMessage.ClosePanel ();
		GameObject.Destroy (localSendMessage.gameObject);
		localSendMessage = null;		
	}

	public void ShowInstructionDialog(){
		instDialog = screensManager.ShowInstructionDialog(closeInstruction);
	}

	public void closeInstruction(){
		screensManager.CloseInstructionPanel (instDialog);
		instDialog = null;
	}
		
	public void LogOut(){

		if (PlayerPrefs.GetInt ("regType") == Constants.GOOGLE_PLAY && Social.localUser.authenticated) {
			#if UNITY_ANDROID
			((GooglePlayGames.PlayGamesPlatform)Social.Active).SignOut ();
			#endif
		} else if (PlayerPrefs.GetInt ("regType") == Constants.FACEBOOK) {
			Facebook.Unity.FB.LogOut ();
		}

		UserController.UserName = "";
		UserController.currentUser		= null;
		UserController.registered		= false;
		UserController.authenticated	= false;

		UserController.AccessToken = "";

		PlayerPrefs.DeleteAll ();
		screensManager.ShowRegistrationScreen ();

	}

//	void Update ()
//	{
//
//		if (Input.GetKey (KeyCode.Escape) && this.gameObject.activeSelf) {
//			Debug.Log ("Click back");
//			#if UNITY_ANDROID
//			screensManager.ShowMainScreen();
//			#endif
//		}
//	}
}
