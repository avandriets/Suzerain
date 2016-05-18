using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using SimpleJSON;
using UnityEngine.Events;
using System.IO;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using Facebook.Unity;


public class RegistrationScreenManager : MonoBehaviour {

	public InputField 	iUserName;

	public Toggle		ExistUser;

	WaitPanel	waitPanel 	= null;
	ErrorPanel	errorPanel 	= null;
	
	float longitude = 0;
	float lattitude = 0;

	ScreensManager	screensManager	= null;

	public GameObject Google_Play;
	public GameObject FaceBook;
	public GameObject Login_Password;
	public GameObject TypeLogin_Panel;

	public InitSocialNetworks initNetwork;

	public Dictionary<string,object> registerData = new Dictionary<string, object>();

	public void RegistrationType(int type){

		TypeLogin_Panel.SetActive (false);

		if (type == Constants.GOOGLE_PLAY) {
			Google_Play.SetActive (true);
			GoogleGetToken ();
		} else if (type == Constants.FACEBOOK) {
			FaceBook.SetActive (true);
			FBLogin ();
		} else if (type == Constants.LOGIN_PASS) {
			Login_Password.SetActive (true);
		}

	}

	void Start(){
		SoundManager.MusicOFF (true);
	}


	void OnEnable() {
	
		MainScreenManager.googleAnalytics.LogScreen (new AppViewHitBuilder ().SetScreenName ("Registration screen"));

		initNetwork.InitNetworks ();

		screensManager	= ScreensManager.instance;

		waitPanel = screensManager.ShowWaitDialog ("Определение местоположения ...");

		screensManager.InitLanguage ();
		screensManager.InitTranslateList ();
		screensManager.TranslateUI ();

		Login_Password.SetActive (false);

		StartCoroutine( GetLocation () );

	}

//	public void OnLanguageChange(){
//
//		if (ScreensManager.LMan != null) {
//
//			if (english.isOn) {
//				//ScreensManager.LMan.setLanguage (Path.Combine (Application.dataPath, "lang.xml"), "English");
//				ScreensManager.LMan.setLanguageFromRes ("lang", "English");
//			} else {			
//				//ScreensManager.LMan.setLanguage (Path.Combine (Application.dataPath, "lang.xml"), "Russian");
//				ScreensManager.LMan.setLanguageFromRes ("lang", "Russian");
//			}
//
//			screensManager.TranslateUI ();
//		}
//	}

	public void finishError(){
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
	}
	
	public void finishRegistration()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject app = activity.Call<AndroidJavaObject>("getApplicationContext");

		AndroidJavaClass AdWordsConversionReporter = new AndroidJavaClass("com.google.ads.conversiontracking.AdWordsConversionReporter");
		AdWordsConversionReporter.CallStatic("reportWithConversionId", app, "925668342", "-0GECLO2-mQQ9qeyuQM", "0", true);
		#endif

		MainScreenManager.googleAnalytics.LogEvent(new EventHitBuilder()
			.SetEventCategory("game")
			.SetEventAction("Registration success"));
		
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
		SoundManager.MusicOFF (false);

		//Activate default view
		Google_Play.SetActive(false);
		FaceBook.SetActive(false);
		Login_Password.SetActive(false);
		TypeLogin_Panel.SetActive(true);

		screensManager.ShowMainScreen ();
	}
	
	public IEnumerator GetLocation()
	{

		string resultText;

		if (!Input.location.isEnabledByUser) {
			resultText = "Disabled by user";
			Debug.Log("Disabled by user");

			screensManager.CloseWaitPanel(waitPanel);

			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@geolocation_doesnt_swith_on"), finishError);

			yield break;
		}
		
		// Start service before querying location
		Input.location.Start();
		
		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
		{
			resultText = "Waiting";
			Debug.Log("Waiting");
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		
		// Service didn't initialize in 20 seconds
		if (maxWait < 1)
		{
			resultText = "Timed out";
			Debug.Log("Timed out");

			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@time_out") ,finishError);

			screensManager.CloseWaitPanel(waitPanel);

			yield break;
		}
		
		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed)
		{
			resultText = "Unable to determine device location";
			Debug.Log("Unable to determine device location");


			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@cannot_find_location"), finishError);

			screensManager.CloseWaitPanel(waitPanel);

			yield break;
		}
		else
		{
			// Access granted and location value could be retrieved
			resultText = "Location: latitude " + Input.location.lastData.latitude + "\n longitude  " + Input.location.lastData.longitude + "\n altitude " + Input.location.lastData.altitude + "\n horizontalAccuracy " + Input.location.lastData.horizontalAccuracy + "\n lastData.timestamp " + Input.location.lastData.timestamp;
			
			longitude = Input.location.lastData.longitude;
			lattitude = Input.location.lastData.latitude;
		}

		Debug.Log (resultText);
		// Stop service if there is no need to query location updates continuously
		Input.location.Stop();

		screensManager.CloseWaitPanel(waitPanel);
		
	}


	private void SaveUserPrefs(int pRegType)
	{
		if (pRegType == Constants.LOGIN_PASS) {
			UserController.UserName		= registerData["userName"].ToString();
			UserController.UserPassword = registerData["userPassword"].ToString();
		} else {
			UserController.UserName = registerData["email"].ToString().Split('@')[0];
			UserController.AccessToken = registerData ["token"].ToString ();
		}

		PlayerPrefs.SetString ("username", UserController.UserName);
		PlayerPrefs.SetString ("password", UserController.UserPassword);
		PlayerPrefs.SetString ("email", registerData["email"].ToString());

		PlayerPrefs.SetInt ("regType", pRegType);
	}
	
	public void onClickSubmitRegistration()
	{
		bool error = false;

		if (iUserName.text.Length == 0) {
			error = true;
			errorPanel = screensManager.ShowErrorDialog ("Имя пользователя должно быть заполнено.", finishError);
		}

		if (!error) {

			registerData.Clear ();

			registerData.Add ("userName", iUserName.text);
			registerData.Add ("email", iUserName.text+"@suzerain.com");
			registerData.Add ("sex", "0");
			registerData.Add ("userPassword", RandomString(7));
			registerData.Add ("languageId", getLanguageCode ().ToString ());
			registerData.Add ("countryId", "102");
			registerData.Add ("latitude", lattitude.ToString ());
			registerData.Add ("longitude", longitude.ToString ());
			registerData.Add ("token", "");

			if (ExistUser.isOn ) {
				SaveExistUser (Constants.LOGIN_PASS);
			} else {
				registerUser (Constants.LOGIN_PASS);
			}
		}
	}

	public void SaveExistUser(int regType){
	
		UserController.UserName 		= registerData["userName"].ToString ();
		UserController.UserPassword 	= registerData["userPassword"].ToString ();
	
		UserController user_controller	= UserController.instance;

		waitPanel = screensManager.ShowWaitDialog(ScreensManager.LMan.getString ("@connecting"));
		user_controller.LogIn (InitLabels, longitude , lattitude);

	}

	public void InitLabels(bool error, string source_error, string error_text){

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		if (error == false) {
			
			SaveUserPrefs (3);
			UserController.registered 		= true;
			UserController.authenticated 	= false;
			screensManager.InitLanguage();

			errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@registration_success") , finishRegistration);
		} else {
			
			UserController.UserName 		= "";

			errorPanel = screensManager.ShowErrorDialog("Не верные учетные данные", finishError);
		}
	}

	public int getLanguageCode()
	{
//		if (english.isOn)
//			return 1;
		return 2;
	}
	
	private void registerUser(int regType){		
		Debug.Log ("Registration process");
		
		var postScoreURL = Utility.SERVICE_BASE_URL;		
		var method = Utility.REGISTER_USER_URL;
		
		var userName 		= "userName=";
		var eMail 			= "eMail=";
		var userPassword 	= "userPassword=";
		var sex 			= "sex=";
		var languageId 		= "languageId=";
		var countryId 		= "countryId=";
		var latitude 		= "latitude=";
		var longitude 		= "longitude=";
		var name 			= "name=";
		var id 				= "id=";
		var authName		= "authName=";
		var authToken		= "authToken=";

		string authType = "";
		if (regType == Constants.GOOGLE_PLAY) {
			authType = "GP";
		} else if (regType == Constants.FACEBOOK) {
			authType = "FB";
		} else {
			authType = "LOC";
		}

		string mailAdress = registerData ["email"].ToString ();
		var username = mailAdress.Split('@')[0];

		postScoreURL = postScoreURL + method + "?"
			+ userName + username + "&"
			+ eMail + registerData["email"] + "&"
			+ sex + registerData["sex"] + "&"
			+ userPassword + registerData["userPassword"] + "&"
			+ languageId + registerData["languageId"] + "&"
			+ countryId + registerData["countryId"] + "&"
			+ latitude + registerData["latitude"] + "&"
			+ longitude + registerData["longitude"] + "&"
			+ name + "hello" + "&"
			+ id + "333" + "&"
			+ authName + authType + "&"
			+ authToken + ((regType == Constants.LOGIN_PASS) ? registerData["userPassword"].ToString():registerData["token"].ToString()) + "&"
			+ "ver=" + Constants.GAmeVersion.ToString ();
		 
				
		postScoreURL = System.Uri.EscapeUriString (postScoreURL);
		
		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add("Content-Type", "text/json");
		
		var request = new WWW(postScoreURL, null, dictHeader);

		waitPanel = screensManager.ShowWaitDialog (ScreensManager.LMan.getString("@registration"));

		StartCoroutine(WaitForRequest(request, regType));
		
	}
	
	IEnumerator WaitForRequest(WWW www, int regType)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
			try{

				var N = JSON.Parse(www.text);
				var mLoginResult = N["RegisterUserExResult"];

				User newUser = Utility.ParseGetUserResponse(mLoginResult.ToString());

				screensManager.CloseWaitPanel(waitPanel);

				if(newUser.Id != -1){
					UserController.currentUser 	= newUser;
					UserController.registered 	= true;
					SaveUserPrefs(regType);

					UserController.currentUser.Language = "Russian";

					screensManager.InitLanguage();
				
					errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@registration_success") , finishRegistration);
				}else if(newUser.Id == -1){
					errorPanel = screensManager.ShowErrorDialog("Игрок с таким именем уже существует. Пожалуйста, попробуйте ввести другое имя например: " +
						newUser.UserName, finishError);
				}

			}
			catch(Exception e)
			{
				screensManager.CloseWaitPanel(waitPanel);
				errorPanel = screensManager.ShowErrorDialog(e.Message, finishError);
			}

		} else {

			screensManager.CloseWaitPanel(waitPanel);

			if(www.error.Contains("409") || www.error.Contains("conflict") || www.error.Contains("Conflict") || 
				www.text.Contains("409") || www.text.Contains("conflict") || www.text.Contains("Conflict")){
				//"Пользователь с таким именем " + iUserName.text + " существует, попробуйте другое имя."

				MainScreenManager.googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game")
					.SetEventAction("Registration error user exests"));
				
				errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@registration_user_exists"), finishError);
			}else{				
				errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString("@registration_error") , finishError);

				MainScreenManager.googleAnalytics.LogEvent(new EventHitBuilder()
					.SetEventCategory("game")
					.SetEventAction("Registration error unknown"));
			}

			Debug.Log("WWW Error: "+ www.error);
		}    
	}

	public static string RandomString(int length)
	{

		string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		string resultString = "";

		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{ 
			resultString = SystemInfo.deviceUniqueIdentifier;
			Debug.Log ("DEVICE == " + resultString);
		}else{
			for (int i = 0; i < length; i++) {
				resultString = resultString + chars [UnityEngine.Random.Range (0, chars.Length)];
			}
		}

		return resultString;
	}
		
	public void SucsessRegistration(int typeRed, Dictionary<string,object> resuLtReg){
	
		string mStatusText = "";

		registerData.Clear ();

		if (typeRed == Constants.GOOGLE_PLAY) {

			mStatusText = "Welcome " + resuLtReg["username"];
			mStatusText += "\n Email " + resuLtReg["email"];
			mStatusText += "\n AccessToken is " + resuLtReg["token"];

			registerData.Add ("userName", resuLtReg["username"]);
			registerData.Add ("email", resuLtReg["email"]);

		} else if (typeRed == Constants.FACEBOOK) {
			
			mStatusText = "Welcome " + resuLtReg["userId"];
			mStatusText += "\n Email " + resuLtReg["email"];
			mStatusText += "\n AccessToken is " + resuLtReg["token"];

			registerData.Add ("userName", resuLtReg["email"].ToString().Split('@')[0]);
			registerData.Add ("email", resuLtReg["email"]);
		}

		Debug.Log(mStatusText);

		registerData.Add ("sex", "0");
		registerData.Add ("userPassword", RandomString(7));
		registerData.Add ("languageId", getLanguageCode ().ToString ());
		registerData.Add ("countryId", "102");
		registerData.Add ("latitude", lattitude.ToString ());
		registerData.Add ("longitude", longitude.ToString ());
		registerData.Add ("token", resuLtReg["token"]);

		screensManager.CloseWaitPanel (waitPanel);
		//errorPanel = screensManager.ShowErrorDialog(mStatusText ,finishError);

		Google_Play.SetActive(false);
		FaceBook.SetActive(false);
		Login_Password.SetActive(false);
		TypeLogin_Panel.SetActive(true);

		registerUser (typeRed);
	}

	public void FailRegistration(int typeRed){

		Google_Play.SetActive(false);
		FaceBook.SetActive(false);
		Login_Password.SetActive(false);
		TypeLogin_Panel.SetActive(true);

		screensManager.CloseWaitPanel (waitPanel);
		errorPanel = screensManager.ShowErrorDialog("Authentication failed." ,finishError);

	}

	public void GoogleGetToken(){
		waitPanel = screensManager.ShowWaitDialog("Authenticating...");
		initNetwork.GoogleGetToken (SucsessRegistration, FailRegistration);
	}

	public void FBLogin(){
		
		waitPanel = screensManager.ShowWaitDialog("Authenticating...");
		initNetwork.FBLogin (SucsessRegistration, FailRegistration);
	}

	public void BackToMainScreen(){

		Google_Play.SetActive(false);
		FaceBook.SetActive(false);
		Login_Password.SetActive(false);
		TypeLogin_Panel.SetActive(true);

	}
}