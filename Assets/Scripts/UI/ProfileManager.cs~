using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;
using System.IO;
using Soomla.Store;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;


public class ProfileManager : MonoBehaviour {

	public Text 	nicname;
	public Toggle		toggleRussian;
	public Toggle		toggleEnglish;

	public Slider 		soundSlider;

	public SendMessage 		pSendMessage;
	private SendMessage		localSendMessage = null;

	private WaitPanel	waitP 	= null;
	private ErrorPanel	errP 	= null;
	private InstructionDialog	instDialog = null;

	bool wasShown = false;
	ScreensManager screenManager = null;

	public Text TextRank;
	public Image avatar;
	public PaidVersinDialog buyDialog;
	bool wasGetEmail = false;

	// Use this for initialization
	void OnEnable() {
	
		wasGetEmail = false;
		MainScreenManager.googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Profile screen"));

		screenManager = ScreensManager.instance;

		if(PlayerPrefs.HasKey("Volume")){
			soundSlider.value = SetAudioLevels.volumeSize;
		}

		if (!wasShown) {
			
			screenManager.InitTranslateList ();
			wasShown = true;
		}

		InitComponents();

		screenManager.InitLanguage ();
		screenManager.InitTranslateList ();
		screenManager.TranslateUI ();
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
			
			screenManager.TranslateUI ();
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

		//TextRank.text = Utility.getDcumentsPath ();
		//

		Utility.setAvatar(avatar,UserController.currentUser, Rose.statList);

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

		waitP = screenManager.ShowWaitDialog(ScreensManager.LMan.getString("save_profile_label"));

		Debug.Log ("login to server.");
		
		var postScoreURL 	= Utility.SERVICE_BASE_URL;		
		var method 			= Utility.EDIT_USER_URL;
		
		var token 		= "token=";
		var birthDate 	= "birthDate=";
		var motto 		= "motto=";
		var countryId 	= "countryId=";
		var languageId 	= "languageId=";

//		JSONClass rootNode = new JSONClass();		
//		rootNode.Add ("Mask",	new JSONData(currentMask));
//		rootNode.Add ("Avatar",	new JSONData(currentAvatar));
//
//		JSONClass saveAvatar = new JSONClass();
//		saveAvatar.Add("AvatarAndMask",rootNode);
//		InitUserScripts.currentUser.Motto = saveAvatar.ToString ();
		
		postScoreURL = 
			postScoreURL + method + "?" 
				+ token + UserController.currentUser.Token + "&"
				+ birthDate + "01/01/0001" + "&"
				+ motto + System.Uri.EscapeUriString (UserController.currentUser.Motto) + "&"
				+ countryId + UserController.currentUser.CountryId + "&"
				+ languageId + UserController.currentUser.LanguageId;

		WWWForm form = new WWWForm();
		form.AddField("Content-Type", "text/json");
		
		var request = new WWW(postScoreURL, form);
		
		StartCoroutine(WaitForRequest(request));
		
	}
	
	public void LoginErrorAction()
	{
		errP.ClosePanel();
		GameObject.Destroy(errP.gameObject);
		errP = null;
	}
	
	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;

		screenManager.CloseWaitPanel (waitP);

		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);

			UserController.authenticated = false;
			screenManager.ShowMainScreen();
			
		} else {
			errP = screenManager.ShowErrorDialog(www.error + " " + www.text ,LoginErrorAction);
			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	public void OnSendMessageToDevelopers(){
	
		SendMessage NewWaitPanel = GameObject.Instantiate(pSendMessage) as SendMessage;

		NewWaitPanel.transform.SetParent(screenManager.currentScreenCanvas.transform);
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

	public void OnBuyClick(){
		
		Debug.Log ("SOOMLA count = " + StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId).ToString() );

		if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) > 0) {
			Debug.Log ("SOOMLA You already buy it.");
			errP = screenManager.ShowErrorDialog("Вы уже отключили рекламу." ,LoginErrorAction);
		} else {
			Debug.Log ("SOOMLA BUY IT");
			buyDialog.SetText (PaidVersionOkActionClick);
		}
	}

	public void OnRestoreClick(){
		SoomlaStore.RestoreTransactions ();
	}

	public void PaidVersionOkActionClick(){
		
		try{
			
			StoreInventory.BuyItem (BuyItems.NO_ADS_NONCONS.ItemId);

		}catch(System.Exception ex){
			Debug.Log ("SOOMLA BUY ERROR " + ex.Message);
		}
	}

	public void CancelPurch(){
		if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) > 0) {
			VirtualGood vg = StoreInfo.Goods [0];
			StoreInventory.TakeItem (vg.ItemId,1);
			Debug.Log ("SOOMLA Cancel purch.");
			errP = screenManager.ShowErrorDialog("Покупка отменена." ,LoginErrorAction);
		}
	}

	public void ShowInstructionDialog(){
		instDialog = screenManager.ShowInstructionDialog(closeInstruction);
	}

	public void closeInstruction(){
		screenManager.CloseInstructionPanel (instDialog);
		instDialog = null;
	}

	public void GoogleGetToken(){

		if (!Social.localUser.authenticated) {
			// Authenticate
			waitP = screenManager.ShowWaitDialog("Authenticating...");

			Social.localUser.Authenticate((bool success) => {
				
				if (success) {
					wasGetEmail = false;
					//StartCoroutine(getTokenInThread());
									
				} else {
					errP = screenManager.ShowErrorDialog("Authentication failed." ,LoginErrorAction);
				}
			});
		} else {
			// Sign out!
			errP = screenManager.ShowErrorDialog("Signing out." ,LoginErrorAction);
			((GooglePlayGames.PlayGamesPlatform) Social.Active).SignOut();
		}
	}

	void Update(){		
		if (((PlayGamesLocalUser)Social.localUser).Email != null && wasGetEmail == false) {
			if(((PlayGamesLocalUser)Social.localUser).Email.Length > 0 && ((PlayGamesLocalUser)Social.localUser).accessToken.Length > 0 ){

				screenManager.CloseWaitPanel (waitP);
				wasGetEmail = true;

				string mStatusText = "Welcome " + Social.localUser.userName;
				mStatusText += "\n Email " + ((PlayGamesLocalUser)Social.localUser).Email;

				mStatusText += "\n Token ID is " + GooglePlayGames.PlayGamesPlatform.Instance.GetToken();
				mStatusText += "\n AccessToken is " + GooglePlayGames.PlayGamesPlatform.Instance.GetAccessToken();
				errP = screenManager.ShowErrorDialog(mStatusText ,LoginErrorAction);
			}
		}
	}

}
