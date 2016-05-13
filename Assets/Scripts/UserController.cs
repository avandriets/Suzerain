using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;
using Facebook.Unity;

public class UserController : MonoBehaviour {
	
	public static string	UserName;
	public static string	eMail;
	public static string	UserPassword;
	public static string	AccessToken = "";
	
	public static bool		registered;
	public static bool		authenticated;
	public static bool		reNewStatistic = false;

	public static User		currentUser;

	public InitSocialNetworks initNetwork;

	private ProcessLogInDelegate localLogInDelegate;
	private float mLongitiude, mLatitude;

	private static UserController s_Instance = null;
	
	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	public static UserController instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.

				s_Instance =  FindObjectOfType(typeof (UserController)) as UserController;

				s_Instance.InitUserFromLocalStore ();
			}
			
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("GameManager");
				s_Instance = obj.AddComponent(typeof (UserController)) as UserController;

				s_Instance.InitUserFromLocalStore ();

				Debug.Log ("Could not locate an UserController object. \n UserController was Generated Automaticly.");
			}

			return s_Instance;
		}
	}

	public void InitUserFromLocalStore()
	{
		if (PlayerPrefs.HasKey ("username")) {
			UserName = PlayerPrefs.GetString ("username");
			registered = true;
		} else {
			registered = false;
		}
		
		if (PlayerPrefs.HasKey ("email"))
			eMail = PlayerPrefs.GetString ("email");
		
		if (PlayerPrefs.HasKey ("password"))
			UserPassword = PlayerPrefs.GetString("password");
	}
	
	public void LogIn(ProcessLogInDelegate postLogInExecute, float longitiude, float latitude ){

		localLogInDelegate = postLogInExecute;
		mLongitiude = longitiude;
		mLatitude	= latitude;

		int regType = PlayerPrefs.GetInt ("regType");

		if ((regType != Constants.LOGIN_PASS && AccessToken.Length > 0) || regType == Constants.LOGIN_PASS) {
			StartCoroutine (LogInThread (postLogInExecute, longitiude, latitude));

		} else if (regType == Constants.FACEBOOK && AccessToken.Length == 0) {		
			if (!FB.IsInitialized) {
				// Initialize the Facebook SDK
				FB.Init (InitCallback, null);
			} else {
				// Already initialized, signal an app activation App Event
				FB.ActivateApp ();
				initNetwork.RenewToken (SucsessGetToken, FailGetToken, regType);
			}
		} else if (regType == Constants.GOOGLE_PLAY && AccessToken.Length == 0){
			Debug.Log ("INIT InitNetworks");
			initNetwork.InitNetworks ();
			Debug.Log ("INIT InitGoogle");
			initNetwork.InitGoogle ();
			Debug.Log ("INIT RenewToken");
			initNetwork.RenewToken (SucsessGetToken, FailGetToken, regType);
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp ();

			initNetwork.RenewToken (SucsessGetToken, FailGetToken, PlayerPrefs.GetInt ("regType"));

		} else {
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}
	}

	public void SucsessGetToken(int typeRed, Dictionary<string,object> resuLtReg){

		Debug.Log ("INIT SucsessGetToken");
		AccessToken = resuLtReg ["token"].ToString();
		StartCoroutine (LogInThread (localLogInDelegate, mLongitiude, mLatitude));

	}

	public void FailGetToken(int typeRed){

		localLogInDelegate(true, Constants.LOGIN_ERROR, "");
	}

	public IEnumerator LogInThread(ProcessLogInDelegate postLogInExecute, float longitiude, float latitude){
	
		bool error = false;

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add("Content-Type", "text/json");

		WWWForm form = new WWWForm();
		form.AddField("Content-Type", "text/json");

		int regType = PlayerPrefs.GetInt ("regType");

		WWW www = null;

		if (!authenticated) {
			
			if (regType == Constants.LOGIN_PASS) {
				www = new WWW (NetWorkUtils.buildLogInURL (), null, dictHeader);
			} else {
				www = new WWW (NetWorkUtils.buildLogEXInURL (regType), null, dictHeader);
			}
	
			while (!www.isDone) {
				yield return null;
			}

			if (www.error == null) {
				Debug.Log ("WWW login Ok!: " + www.text);
				
				var N = JSON.Parse (www.text);
				var mLoginResult = N ["LoginResult"];

				if (regType != Constants.LOGIN_PASS) {
					mLoginResult = N ["LoginExResult"];
				}

				User loginUser = Utility.ParseGetUserResponse (mLoginResult.ToString ());
				UserController.currentUser = loginUser;
				authenticated = true;
				reNewStatistic = false;
				ScreensManager.instance.InitLanguage ();
				//language			= currentUser.Language;
			} else {
				authenticated = false;
				reNewStatistic = false;
				Debug.Log ("WWW login error: " + www.error);
				error = true;

				postLogInExecute (true, Constants.LOGIN_ERROR, www.error + " " + www.text);
			}
		}

		if (reNewStatistic) {
		

			www = new WWW (NetWorkUtils.buildUserInfoURL (UserController.currentUser.Id), form);

			while (!www.isDone) {
				yield return null;
			}

			if (www.error == null) {
				Debug.Log ("WWW login Ok!: " + www.text);

				var NN = JSON.Parse (www.text);
				var mLoginResult1 = NN ["GetUserInfoResult"];

				User loginUser1 = Utility.ParseGetUserResponse (mLoginResult1.ToString ());
				string token = currentUser.Token;
				loginUser1.Token = token;

				UserController.currentUser = loginUser1;
				reNewStatistic = false;
				ScreensManager.instance.InitLanguage ();
				//language			= currentUser.Language;
			} else {
				authenticated = false;
				reNewStatistic = false;
				Debug.Log ("WWW login error: " + www.error);
				error = true;

				postLogInExecute (true, Constants.LOGIN_ERROR, www.error + " " + www.text);
			}

		}

		if (!error) {
			//POST
			www = new WWW (NetWorkUtils.buildMyStatURL (longitiude, latitude), null, dictHeader);

			while (!www.isDone) {
				yield return null;
			}

			if (www.error == null) {
				Debug.Log ("WWW get State Ok!: " + www.text);

				Rose.statList = Utility.GetListOfFightsStates (www.text);

				postLogInExecute (false, "", "");

			} else {

				authenticated = false;
				Debug.Log ("WWW login error: " + www.error);
				error = true;

				postLogInExecute (true, Constants.LOGIN_ERROR, www.error + " " + www.text);
			}
				
		} 
	}

	public void LoginErrorAction()
	{
		Application.Quit ();
	}

}
