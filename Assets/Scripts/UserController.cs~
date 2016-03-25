using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;

public class UserController : MonoBehaviour {
	
	public static string	UserName;
	public static string	eMail;
	public static string	UserPassword;
	
	public static bool		registered;
	public static bool		authenticated;

	public static User		currentUser;

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

		StartCoroutine (LogInThread(postLogInExecute, longitiude, latitude));
	}

	public IEnumerator LogInThread(ProcessLogInDelegate postLogInExecute, float longitiude, float latitude){
	
		bool error = false;

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add("Content-Type", "text/json");

		WWWForm form = new WWWForm();
		form.AddField("Content-Type", "text/json");

		//POST
		WWW www = new WWW(NetWorkUtils.buildLogInURL(), null, dictHeader);
	
		while (!www.isDone)
		{
			yield return null;
		}

		if (www.error == null)
		{
			Debug.Log("WWW login Ok!: " + www.text);
				
			var N = JSON.Parse(www.text);
			var mLoginResult = N["LoginResult"];
				
			User loginUser 	= Utility.ParseGetUserResponse(mLoginResult.ToString());
			UserController.currentUser			= loginUser;
			authenticated 		= true;
			ScreensManager.instance.InitLanguage ();
			//language			= currentUser.Language;
		}
		else
		{
			authenticated = false;
			Debug.Log("WWW login error: "+ www.error);
			error = true;

			postLogInExecute(true, Constants.LOGIN_ERROR, www.error + " " + www.text);
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
