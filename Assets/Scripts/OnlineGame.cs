using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public delegate void ShowGameResult ();

public class OnlineGame : MonoBehaviour
{

	[HideInInspector]
	public Fight currentFight = null;
	[HideInInspector]
	public User opponent = null;
	[HideInInspector]
	public List<TestTask> tasksList = null;
	bool fightCanceled = false;

	public static List<FightStat> statList = null;

	private static OnlineGame s_Instance = null;

	public List<TaskAnswer> answerList = null;
	public List<Fight> fightsList = null;

	WaitPanel waitP = null;
	ErrorPanel errP = null;

	ScreensManager screenManager;

	public ShowGameResult resultDelegate;
	public ShowGameResult errorRoundResult;

	public GameProtocol gameProtocol = new GameProtocol ();

	public static OnlineGame instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance = FindObjectOfType (typeof(OnlineGame)) as OnlineGame;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject ("OnlineGame");
				s_Instance = obj.AddComponent (typeof(OnlineGame)) as OnlineGame;
				Debug.Log ("Could not locate an AManager object. \n ScreensManager was Generated Automaticly.");
			}

			return s_Instance;
		}
	}

	public void AskForFight (CancelFightByServerDelegate cancelByServer, ReadyToFight readyToFight, ErrorToFight errordelegate)
	{
		
		Debug.Log ("LetsFight to server.");

		//SoundManager.ChoosePlayMusic (1);

		currentFight	= null;
		opponent = null;
		fightCanceled	= false;
		answerList = null;
		statList = null;
		fightsList = null;


		StartCoroutine (RequestForFight (cancelByServer, readyToFight, errordelegate));
	}

	IEnumerator RequestForFight (CancelFightByServerDelegate cancelByServer, ReadyToFight readyToFight, ErrorToFight errordelegate)
	{

		var postScoreURL = NetWorkUtils.buildRequestToFightURL ();

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "text/json");

		WWWForm form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (postScoreURL, form);

		while (!request.isDone) {
			yield return null;
		}

		if (request.error == null) {

			Debug.Log ("Lets fight waiting for fight ! " + request.text);

			currentFight = Utility.ParseFightResponse (request.text);

			Debug.Log ("TaskId =  " + currentFight.TaskId.ToString ());


			if (currentFight.FightState == -1) {
				StartCoroutine (GetTaskRequest (cancelByServer, readyToFight, errordelegate));

			} else if (currentFight.FightState == 0 || currentFight.FightState == -1) {
				
				StartCoroutine (StateRequest (cancelByServer, readyToFight, errordelegate));

			} else if (currentFight.FightState == 4) {
				cancelByServer ();
			}

		} else {
			errordelegate ();
			Debug.Log ("WWW Error: " + request.error);
			Debug.Log ("WWW Error: " + request.text);
		}

	}

	public void CancelFight ()
	{
		fightCanceled = true;

		StartCoroutine (RejectFight ());
	}

	public IEnumerator RejectFight ()
	{
	

		var form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (NetWorkUtils.buildCancelFightDialog (currentFight), form);

		while (!request.isDone) {
			yield return null;
		}
	}

	public IEnumerator GetTaskRequest (CancelFightByServerDelegate cancelByServer, ReadyToFight readyToFight, ErrorToFight errordelegate)
	{
	
		bool error = false;
		Debug.Log ("Get Fight state on server.");

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "text/json");

		WWWForm form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (NetWorkUtils.buildGetTaskFightURL (currentFight), form);

		while (!request.isDone) {
			yield return null;
		}

		if (request.error == null) {
			Debug.Log ("State fight ! " + request.text);
			tasksList = Utility.GetListOfTasks (request.text);

		} else {
			error = true;
			errordelegate ();
			Debug.Log ("WWW Error: " + request.error);
			Debug.Log ("WWW Error: " + request.text);
		}

		if (!error) {

			request = new WWW (NetWorkUtils.buildGetOpponentInfo (currentFight), form);

			while (!request.isDone) {
				yield return null;
			}

			if (request.error == null) {
				Debug.Log ("State fight ! " + request.text);

				var N1 = JSON.Parse (request.text);
				var mLoginResult1 = N1 ["GetUserInfoResult"];

				opponent = Utility.ParseGetUserResponse (mLoginResult1.ToString ());

			} else {
				error = true;
				errordelegate ();
				Debug.Log ("WWW Error: " + request.error);
				Debug.Log ("WWW Error: " + request.text);
			}
		}

		if (!error) {

			//POST
			request = new WWW (NetWorkUtils.buildStatURL (currentFight), null, dictHeader);

			while (!request.isDone) {
				yield return null;
			}

			if (request.error == null) {
				Debug.Log ("WWW get State Ok!: " + request.text);

				OnlineGame.statList = Utility.GetListOfFightsStatesUSER (request.text);

				readyToFight ();

			} else {

				error = true;
				errordelegate ();
				Debug.Log ("WWW Error: " + request.error);
				Debug.Log ("WWW Error: " + request.text);
			}
		}

	}

	IEnumerator StateRequest (CancelFightByServerDelegate cancelByServer, ReadyToFight readyToFight, ErrorToFight errordelegate)
	{
		while (currentFight != null && currentFight.FightState == 0 && fightCanceled == false) {
			Debug.Log ("timer request");
			StartCoroutine (StateFightRequest (cancelByServer, readyToFight, errordelegate));

			yield return new WaitForSeconds (5);
		}
	}

	public IEnumerator StateFightRequest (CancelFightByServerDelegate cancelByServer, ReadyToFight readyToFight, ErrorToFight errordelegate)
	{
	
		bool error = false;
		Debug.Log ("Get Fight state on server.");

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "text/json");

		WWWForm form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (NetWorkUtils.buildRequestStateFightURL (currentFight.Id.ToString ()), form);


		while (!request.isDone) {
			yield return null;
		}

		if (request.error == null) {

			Debug.Log ("Lets fight waiting for fight ! " + request.text);

			currentFight = Utility.ParseFightStateResponse (request.text);

			Debug.Log ("TaskId =  " + currentFight.TaskId.ToString ());

			if (currentFight.FightState == 4) {
				fightCanceled = true;
				cancelByServer ();
			}

		} else {
			error = true;
			errordelegate ();
			Debug.Log ("WWW Error: " + request.error);
			Debug.Log ("WWW Error: " + request.text);
		}


		if (currentFight.FightState == -1 && !error) {
			StartCoroutine (GetTaskRequest (cancelByServer, readyToFight, errordelegate));
		}
	}

	public IEnumerator SentAnswersInQueeThread (ShowGameResult pResultDelegate, ShowGameResult pErrorDelegate)
	{

		resultDelegate = pResultDelegate;

		screenManager = ScreensManager.instance;
		waitP = screenManager.ShowWaitDialog (ScreensManager.LMan.getString ("@find_result"));

		bool error = false;

		if (fightCanceled) {

			if (waitP != null)
				screenManager.CloseWaitPanel (waitP);

			errorRoundResult = pErrorDelegate;
			errP = screenManager.ShowErrorDialog (ScreensManager.LMan.getString ("@fight_canceled"), CancelFightWaitingForRoundResult);
		} else if (!error) {

			while (currentFight != null && currentFight.FightState == -1 && fightCanceled == false) {
				gameProtocol.AddMessage ("Time: " + System.DateTime.Now.ToString ());
				gameProtocol.AddMessage ("Ask 4 Global fight state");

				Debug.Log ("Time: " + System.DateTime.Now.ToString ());
				Debug.Log ("Ask 4 Global fight state");


				OnFightResultRequest ();

				yield return new WaitForSeconds (2);
			}
		}

	}

	public void CancelFightWaitingForResult ()
	{

		GameObject.Destroy (errP.gameObject);
		errP = null;
		screenManager.ShowMainScreen ();
	}

	public void CancelFightWaitingForRoundResult ()
	{

		GameObject.Destroy (errP.gameObject);
		errP = null;
		errorRoundResult ();
		//screenManager.ShowMainScreen ();
	}

	public void OnFightResultRequest ()
	{
		Debug.Log ("Get Fight state on server.");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = Utility.STATE_FIGHT_URL;

		var fightId = "fightId=";
		var userToken = "token=";

		postScoreURL = postScoreURL + method + "?"
		+ fightId + currentFight.Id.ToString () + "&"
		+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "text/json");

		WWWForm form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (postScoreURL, form);

		StartCoroutine (WaitForResultRequest (request));
	}

	IEnumerator WaitForResultRequest (WWW www)
	{
		
		while (!www.isDone) {
			waitP.SetDebugMessage (" Wait for global fight state");
			yield return null;
		}

		// check for errors
		if (www.error == null) {
			//Debug.Log("State fight ! " + www.text);
			//waitForOpponent.ClosePanel();

			currentFight = Utility.ParseFightStateResponse (www.text);

			gameProtocol.AddMessage ("Send request OK");
			gameProtocol.AddMessage ("Time: " + System.DateTime.Now.ToString ());
			gameProtocol.AddMessage ("request " + www.url);
			gameProtocol.AddMessage ("global fight state  " + currentFight.FightState.ToString ());

			Debug.Log ("Send request OK");
			Debug.Log ("Time: " + System.DateTime.Now.ToString ());
			Debug.Log ("request " + www.url);
			Debug.Log ("global fight state  " + currentFight.FightState.ToString ());

			waitP.SetDebugMessage (" Send request OK \n" +
			" Time: " + System.DateTime.Now.ToString () + "\n" +
			"global fight state  " + currentFight.FightState.ToString ()
			);

			if (currentFight.FightState == 2) {
				screenManager.CloseWaitPanel (waitP);
				//waitP.ClosePanel();
				resultDelegate ();
			} else if (currentFight.FightState == 4) {
				
				screenManager.CloseWaitPanel (waitP);
				errP = screenManager.ShowErrorDialog (ScreensManager.LMan.getString ("@fight_canceled"), CancelFightWaitingForResult);
				//errP.SetText("Бой расформирован",CancelFight);
			}

			Debug.Log ("TaskId =  " + currentFight.TaskId);

		} else {

			gameProtocol.AddMessage ("Send request ERROR");
			gameProtocol.AddMessage ("Time: " + System.DateTime.Now.ToString ());
			gameProtocol.AddMessage ("request " + www.url);
			gameProtocol.AddMessage ("ERROR  " + www.text + www.error);

			Debug.Log ("Send request ERROR");
			Debug.Log ("Time: " + System.DateTime.Now.ToString ());
			Debug.Log ("request " + www.url);
			Debug.Log ("ERROR  " + www.text + www.error);

			waitP.SetDebugMessage (" Send request ERROR \n" +
			" Time: " + System.DateTime.Now.ToString () + "\n" +
			"ERROR  " + www.text + www.error
			);

			screenManager.CloseWaitPanel (waitP);
			errP = screenManager.ShowErrorDialog (www.text, CancelFightWaitingForResult);
			//errP.SetText(www.text,CancelFight);			
			Debug.Log ("WWW Error: " + www.error);
		}
	}

	public int getMyAnswer ()
	{

		if (currentFight.InitiatorId == UserController.currentUser.Id) {
			return currentFight.InitiatorAnswer;
		} else {
			return currentFight.OpponentAnswer;
		}
	}

	public int getOponentAnswer ()
	{

		if (currentFight.InitiatorId == UserController.currentUser.Id) {
			return currentFight.OpponentAnswer;
		} else {
			return currentFight.InitiatorAnswer;
		}
	}

	public int getMyScore ()
	{

		if (currentFight.InitiatorId == UserController.currentUser.Id) {
			return currentFight.InitiatorScore;
		} else {
			return currentFight.OpponentScore;
		}
	}

	public int getOponentScore ()
	{

		if (currentFight.InitiatorId == UserController.currentUser.Id) {
			return currentFight.OpponentScore;
		} else {
			return currentFight.InitiatorScore;
		}
	}

	public string getExtendetWinDescription ()
	{

		if (getMyAnswer () > getOponentAnswer ())
			return ScreensManager.LMan.getString ("@wrong_opponent_answer");
		else
			return ScreensManager.LMan.getString ("@you_answered_faster");

	}

	public string getExtendetLoseDescription ()
	{

		if (getMyAnswer () < getOponentAnswer ())
			return ScreensManager.LMan.getString ("@you_answered_incorrect");
		else
			return ScreensManager.LMan.getString ("@opponent_answered_faster");

	}

	public string getExtendetDraftDescription ()
	{

		if (getMyAnswer () == 0)
			return ScreensManager.LMan.getString ("@both_answer_incottect");
		return "";

	}


	public IEnumerator SentSINGLEAnswersToServer (StartNextRoundDelegate pResultDelegate, ShowGameResult pErrorDelegate)
	{

		//Debug.Log("Send single answer to server.");			
		System.DateTime tm = System.DateTime.Now;
		gameProtocol.AddMessage ("Time:" + tm.ToString ()); gameProtocol.AddMessage ("Send single answer to server: method SetTestAnswer");
		Debug.Log (gameProtocol.GetLog ());

		bool 	receive_fight_result = false;
		string 	ErrorMessage = "";
		bool 	errorSendAnswer = false;
		bool 	errorAskRoundResult = false;
		bool 	errorFightState = false;

		float 	timer = 0; 
		float 	timeOut = 5;
		bool 	timeOutError = false;
		int 	Attempts = 6;
		int 	AttemptCount = 0;
		bool	reflexLeft = false;

		int roundNumber = 0;

		Fight localFightStae = null;
		errorRoundResult = pErrorDelegate;

		screenManager = ScreensManager.instance;

		if (fightsList == null) {
			fightsList = new List<Fight> ();

			gameProtocol.AddMessage ("round # 1");
			Debug.Log ("round # 1");
		} else {
			gameProtocol.AddMessage ("round # " + fightsList.Count.ToString ());
			Debug.Log ("round # " + fightsList.Count.ToString ());
		}

		roundNumber = fightsList.Count;

		//SHOW waiting screen
		waitP = screenManager.ShowWaitDialog (ScreensManager.LMan.getString ("@find_result"));

		bool error = false;

		var www = createFightResultRequest (out reflexLeft);

		AttemptCount = 0;
		while (AttemptCount < Attempts) {

			timeOutError = false;
			timer = 0;

			while (!www.isDone) {

				if (timer > timeOut) { 
					timeOutError = true; 
					break; 
				}

				timer += Time.deltaTime;
				yield return null;
			}

			if (timeOutError == false) {

				try {
					var Ntest = JSON.Parse (www.text);
					var fftest = Ntest ["SetTestAnswerResult"].AsInt;
					Debug.Log (fftest.ToString ());
					break;
				} catch (System.Exception ex) {
					Debug.Log (ex.Message);
				}

			}
			//yield return new WaitForSeconds (2);

			www = createFightResultRequest (out reflexLeft);

			AttemptCount++;
		}

		if (timeOutError == true) {
			error = true;
		}

		try {
			if (error == false && www.error == null) {

				//{"SetTestAnswerResult":0}
				try {
					var N = JSON.Parse (www.text);
					var mGetResult = N ["SetTestAnswerResult"].AsInt;
					Debug.Log (mGetResult.ToString ());
				} catch (System.Exception ex) {
					
					Debug.Log ("Data send ERROR" + www.text + " " + www.error);
					Debug.Log ("Time:" + tm.ToString ());
					Debug.Log ("Exception " + ex.Message);

					//waitP.SetDebugMessage (gameProtocol.GetLog ());

					error = true;
					ErrorMessage = www.text;
					errorSendAnswer = true;
				}
					
				Debug.Log ("Data send OK" + www.text);
				Debug.Log ("Time:" + tm.ToString ());
				//waitP.SetDebugMessage (gameProtocol.GetLog());

			} else {

				if (timeOutError) {
						
					Debug.Log ("Data send ERROR timeOut error");
					Debug.Log ("Time:" + tm.ToString ());
					//waitP.SetDebugMessage (gameProtocol.GetLog ());

					error = true;
					ErrorMessage = "Время ожидания истекло send result \n AttemptCount=" + AttemptCount.ToString ();
					//ErrorMessage = www.text;
					errorSendAnswer = true;

				} else {

					Debug.Log ("Data send ERROR" + www.text + " " + www.error);
					Debug.Log ("Time:" + tm.ToString ());
					//waitP.SetDebugMessage (gameProtocol.GetLog ());

					error = true;
					ErrorMessage = www.error;
					errorSendAnswer = true;
				}

			}
				
			Debug.Log ("Time:" + tm.ToString ());
			Debug.Log ("Wait 2 seconds for next request.");
			//waitP.SetDebugMessage (gameProtocol.GetLog());

		} catch (System.Exception ex) {
			
			error = true;
			ErrorMessage = ex.Message;

		}

		//yield return new WaitForSeconds (2);

		WWW request = null;
		WWW request1 = null;

		// Ask about opponent result
		if (!error) {

			int AttemptsCountForTakeResult = 5;

			localFightStae = new Fight ();
			localFightStae.FightState = 0;

			while (!error && !(localFightStae.OpponentScore > 0 && Mathf.Abs (localFightStae.InitiatorScore) > 0) && localFightStae.FightState != 2 && localFightStae.FightState != 4) {
				
				Debug.Log ("Wait foe users answer.");

				var form = new WWWForm ();
				form.AddField ("Content-Type", "text/json");

				Debug.Log ("Time:" + tm.ToString ()); Debug.Log ("Ask for round state " + NetWorkUtils.buildGetRoundInfoURL (currentFight, answerList.Count));

				request = new WWW (NetWorkUtils.buildGetRoundInfoURL (currentFight, answerList.Count), form);

				timer = 0;
				AttemptCount = 0;
				while (AttemptCount < Attempts) {

					timeOutError = false;
					timer = 0;
					while (!request.isDone) {

						if (timer > timeOut) { 
							timeOutError = true; 
							break; 
						}

						timer += Time.deltaTime;
						yield return null;
					}

					if (timeOutError == false) {
						break;
					}

					//yield return new WaitForSeconds (1);

					request = new WWW (NetWorkUtils.buildGetRoundInfoURL (currentFight, answerList.Count), form);

					AttemptCount++;
				}

				if (timeOutError == true) {
					error = true;
				}

				try {
					if (error == false && request.error == null) {

						localFightStae = Utility.ParseRoundFightResponse (request.text);

						if(fightsList.Count == roundNumber){
							fightsList.Add(localFightStae);
						}else{
							fightsList[roundNumber] = localFightStae;
						}

						if (localFightStae.OpponentScore > 0 && Mathf.Abs (localFightStae.InitiatorScore) > 0) {
							if (currentFight.FightTypeId == 4) {
								if (reflexLeft == true) {
									if (UserController.currentUser.Id == localFightStae.InitiatorId) {
										localFightStae.InitiatorScore = (-1) * localFightStae.InitiatorScore;
									} else {
										localFightStae.OpponentScore = (-1) * localFightStae.OpponentScore;
									}
								}
							}
						}
							
						Debug.Log ("Data send OK round state" + request.text); Debug.Log ("Time:" + tm.ToString ()); Debug.Log ("round# FightState " + localFightStae.FightState);

					} else {

						if (timeOutError) {

							Debug.Log ("Data send ERROR timeOut error round state"); Debug.Log ("Time:" + tm.ToString ());

							error = true;
							errorAskRoundResult = true; 
							ErrorMessage = "Время ожидания истекло ask for result";

						} else {

							Debug.Log ("Data send ERROR" + request.text + " " + request.error); Debug.Log ("Time:" + tm.ToString ());

							error = true;
							errorAskRoundResult = true;
							ErrorMessage = request.error;
						}

					}
						
					Debug.Log ("Time:" + tm.ToString ()); Debug.Log ("Wait 2 seconds for GlobalFiht request");

				} catch (System.Exception ex) {

					error = true;
					ErrorMessage = ex.Message;

				}

				//yield return new WaitForSeconds (2);

				//Ask for whole fight state
				if (!error) {
					request1 = getStateWWW ();

					timer = 0;
					AttemptCount = 0;
					while (AttemptCount < Attempts) {

						Debug.Log ("Time:" + tm.ToString ()); Debug.Log ("Ask for Global fight state ATTEMPT # " + AttemptCount.ToString ());

						timeOutError = false;
						timer = 0;

						while (!request1.isDone) {

							if (timer > timeOut) { 
								timeOutError = true; 
								break; 
							}

							timer += Time.deltaTime;
							yield return null;
						}

						if (timeOutError == false) {
							break;
						}

						//yield return new WaitForSeconds (1);
						request1 = getStateWWW ();

						AttemptCount++;
					}

					if (timeOutError == true) {
						error = true;
					}
						
					Debug.Log ("Time:" + tm.ToString ()); Debug.Log ("Ask for round state ");

					try {
						
						if (error == false && request1.error == null) {
							currentFight = Utility.ParseFightStateResponse (request1.text);
						
							Debug.Log ("Time:" + tm.ToString ()); Debug.Log ("Data send OK" + request1.text); Debug.Log ("GLOBAL FightState" + currentFight.FightState);

						} else {
							
							if (timeOutError) {
								
								error = true;
								errorFightState = true;
								ErrorMessage = "Время ожидания истекло ask global result";

								Debug.Log ("Data send ERROR timeout request"); Debug.Log ("Time:" + tm.ToString ());

							} else {
								
								error = true;
								errorFightState = true;
								ErrorMessage = request1.error;

								Debug.Log ("Data send ERROR" + request1.text + " " + request1.error); Debug.Log ("Time:" + tm.ToString ());
							}
						}
					} catch (System.Exception ex) {
						error = true;
						ErrorMessage = ex.Message;
					}
				}
					
				Debug.Log ("Time:" + tm.ToString ()); Debug.Log ("Wait 2 seconds for local&global fight request");

				request.Dispose ();
				request = null;

				//yield return new WaitForSeconds (2);
			}

		}

		if (fightsList [roundNumber].FightState == 4) {
			fightsList [roundNumber].Winner = UserController.currentUser.Id;
		}
		//fightsList.Add (localFightStae);
	
		screenManager.CloseWaitPanel (waitP);

		//error = true;

		if (error || timeOutError) {

			tm = System.DateTime.Now;
			gameProtocol.AddMessage ("Time:" + tm.ToString ());
			gameProtocol.AddMessage ("Error request");
			Debug.Log ("Time:" + tm.ToString ());
			Debug.Log ("Error request");
			//waitP.SetDebugMessage (gameProtocol.GetLog());

			//ErrorMessage = "Противник покинул поединок \n Вам засчитана победа";
			ErrorMessage = "Противник покинул поединок ";

			fightCanceled = true;

			if (errorSendAnswer) {
				errP = screenManager.ShowErrorDialog (ErrorMessage, CancelFightWaitingForRoundResult);
			} else if (errorAskRoundResult) {
				errP = screenManager.ShowErrorDialog (ErrorMessage, CancelFightWaitingForRoundResult);
			} else if (errorFightState) {
				errP = screenManager.ShowErrorDialog (ErrorMessage, CancelFightWaitingForRoundResult);
			} else {
				errP = screenManager.ShowErrorDialog ("Ошибка игрового сервера", CancelFightWaitingForRoundResult);
			}
		} else if ((currentFight.FightState == 4 || localFightStae.FightState == 4) && !(localFightStae.FightState == 2 || localFightStae.FightState == 4) ) {

			tm = System.DateTime.Now;
			gameProtocol.AddMessage ("Time:" + tm.ToString ());
			gameProtocol.AddMessage ("Error figt state 4");
			Debug.Log ("Time:" + tm.ToString ());
			Debug.Log ("Error figt state 4");

			fightCanceled = true;
			errP = screenManager.ShowErrorDialog ("Бой расформирован.", CancelFightWaitingForRoundResult);
			//pResultDelegate (fightsList);
		} else if (localFightStae.FightState == 2) {
			
			tm = System.DateTime.Now;
			gameProtocol.AddMessage ("Time:" + tm.ToString ()); gameProtocol.AddMessage ("Nex round"); 
			Debug.Log ("Time:" + tm.ToString ());Debug.Log ("Nex round");

			currentFight.FightState = -1;
			pResultDelegate (fightsList);

		} else if(localFightStae.FightState == 4){
			
			fightCanceled = true;
			errP = screenManager.ShowErrorDialog ("Ваш противник покинул поединок.\n\nБой расформирован.", CancelFightWaitingForRoundResult);
		}

	}

	public WWW createFightResultRequest (out bool reflexLeft)
	{

		reflexLeft = false;
		var postScoreURL = Utility.SERVICE_BASE_URL;		
		var method = Utility.SET_RANGE_ANSWER_URL;
		var userToken = "token=";

		//Send answer to server
		var answerItem = answerList [answerList.Count - 1];
		postScoreURL = Utility.SERVICE_BASE_URL;
		postScoreURL = postScoreURL + method + "?" + userToken + UserController.currentUser.Token;

		JSONClass rootNode = new JSONClass ();
		rootNode.Add ("FightId",	new JSONData (answerItem.FightId));
		rootNode.Add ("QNum", new JSONData (answerItem.QNum));
		rootNode.Add ("Answer", new JSONData (answerItem.Answer));

		if (currentFight.FightTypeId == 4) {
			
			if (answerItem.Score > 0) {
				reflexLeft = true;
			}

			if (answerItem.Score == 0) {
				rootNode.Add ("Score", new JSONData (Mathf.Abs (1)));
			} else {
				rootNode.Add ("Score", new JSONData (Mathf.Abs (answerItem.Score)));
			}

		} else {
			rootNode.Add ("Score", new JSONData (answerItem.Score));
		}

		rootNode.Add ("WasBought",	new JSONData (answerItem.WasBought));

		JSONClass sendElem = new JSONClass ();
		sendElem.Add ("taskAnswers", rootNode);

		var sendData = sendElem.ToString ();

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "application/json");
		dictHeader.Add ("Content-Length", sendData.Length.ToString ());

		var encoding = new System.Text.UTF8Encoding ();
		var pData = encoding.GetBytes (sendData.ToString ());

		//Send data
		gameProtocol.AddMessage ("URL " + postScoreURL);
		Debug.Log ("URL " + postScoreURL);
		gameProtocol.AddMessage ("Data " + sendData);
		Debug.Log ("Data " + sendData);
		//waitP.SetDebugMessage (gameProtocol.GetLog());

		var www = new WWW (postScoreURL, pData, dictHeader);

		return www;
	}

	public WWW getStateWWW ()
	{
		
		//bool error = false;
		Debug.Log ("Get Fight state on server.");

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "text/json");

		WWWForm form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (NetWorkUtils.buildRequestStateFightURL (currentFight.Id.ToString ()), form);

		return request;
	}
}
