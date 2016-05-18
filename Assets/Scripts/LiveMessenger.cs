using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class LiveMessenger : MonoBehaviour {

	public bool pulseWasStarted = false;

	public 	MainScreenManager mainScreen;
	int		fightId = 0;

	ScreensManager	screensManager	= null;

	float startingTime = 0;


	public void StartPulseRequest(){

		screensManager	= ScreensManager.instance;

		if (!pulseWasStarted) {

			pulseWasStarted = true;
			//StartCoroutine (InitSearchList ());
		}
	}

	void Update(){
	
		startingTime += Time.deltaTime;

		if (pulseWasStarted && startingTime > 2) {
	
			StartCoroutine (GetPulse ());
			startingTime = 0;
		}
	}

	IEnumerator GetPulse(){
		
		var postScoreURL = NetWorkUtils.buildRequestPulse ();
		var request = new WWW (postScoreURL);

		while (!request.isDone) {
			yield return null;
		}

		if (request.error == null) {

			var N = JSON.Parse(request.text);
			var mGetResult = N["GetPulseResult"].AsInt;

			if (mGetResult != 0 && fightId != mGetResult && screensManager.mMainScreenCanvas == screensManager.currentScreenCanvas) {

				//screensManager.ShowMainScreen ();
				fightId = mGetResult;
				mainScreen.AskForFightFromFriend (mGetResult);

				Debug.Log ("You was asked to fight ! ");
			}

			Debug.Log ("Send puse done ! " + request.text);
		} else {

			Debug.LogError ("WWW Error: " + request.error);
			Debug.LogError ("WWW Error: " + request.text);
		}

	}

	IEnumerator InitSearchList(){
	
		while (true) {		

			if (UserController.authenticated && UserController.registered) {
				
				var postScoreURL = NetWorkUtils.buildRequestPulse ();

				var request = new WWW (postScoreURL);

				while (!request.isDone) {
					yield return null;
				}
					
				if (request.error == null) {

					var N = JSON.Parse(request.text);
					var mGetResult = N["GetPulseResult"].AsInt;

					if (mGetResult != 0 && fightId != mGetResult && screensManager.mMainScreenCanvas == screensManager.currentScreenCanvas) {

						fightId = mGetResult;
						mainScreen.AskForFightFromFriend (mGetResult);

						Debug.Log ("You was asked to fight ! ");
					}

					Debug.Log ("Send puse done ! " + request.text);
				} else {
					
					Debug.Log ("WWW Error: " + request.error);
					Debug.Log ("WWW Error: " + request.text);
				}
						
			}

			yield return new WaitForSeconds (5);
		}
	}

}
