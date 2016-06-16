using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TrainingManager : BaseUIClass {

	int currentFight = 0;
	List<TestTask> tasksList = null;

	public Transform gameContainer;
	private GameBase currentGameTemplate;

	public ClockObject 			clock;

	private TaskAnswer userAnswer = null;

	public Text sqText;

	int rightAnswersCount = 0;
	int timeInGameSec = 0;

	public FinishTrainingDialog finishTrainingDialog;
	public EndTrainingRound		endTrainingRound;

	public Text gameTypeText;
	public GameObject ROGA;

	void OnEnable (){

		screensManager	= ScreensManager.instance;

		initComponents ();

		waitPanel = screensManager.ShowWaitDialog("Получение данных", false);

		clock.initClockImages ();
		clock.invertTimer = false;
		clock.showMinutes = true;

		clock.SetTime (0f);

		StartCoroutine (StartTraining());
	}

	private void initComponents(){
		//TODO init labels

		tasksList = null;
		userAnswer = null;
		timeInGameSec = 0;
		rightAnswersCount = 0;

		ROGA.SetActive (true);

		finishTrainingDialog.ClosePanel();
		endTrainingRound.ClosePanel();

		if (Rose.statList.Count > 0) {
			sqText.text = string.Format ("{0:N2}", Rose.statList [0].SQ);  
		}
	}

	IEnumerator StartTraining(){

		var postScoreURL = NetWorkUtils.buildRequestToGetTrainingQuestion (Utility.trainingFightsArray[currentFight]);

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "text/json");

		WWWForm form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (postScoreURL);

		while (!request.isDone) {
			yield return null;
		}

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		if (request.error == null) {

			Debug.Log ("buildRequestToGetTrainingQuestion done ! " + request.text);

			tasksList = Utility.GetListOfTasks (request.text, "GetTrainingQuestionResult");

			initGame ();

		} else {

			Debug.LogError ("WWW Error: " + request.error);
			Debug.LogError ("WWW Error: " + request.text);
			errorPanel = screensManager.ShowErrorDialog("Ошбика соединения с сервером", OnErrorGetTrainingClick);
		}
	}

	private void initGame(){

		currentGameTemplate = screensManager.GetGameByNumber (gameContainer, Utility.trainingFightsArray[currentFight]);
		currentGameTemplate.gameObject.SetActive (true);

		gameTypeText.text = ScreensManager.LMan.getString (Utility.FightTypeName (Utility.trainingFightsArray [currentFight]));

		if (userAnswer != null) {
			timeInGameSec = (int)clock.stopTime;
		}

		currentGameTemplate.InitGameObject (tasksList, OnFinishGame, RoundResult, showRoundsResults, clock, 9999);

		if (userAnswer != null) {
			clock.startTime = timeInGameSec;
		}

		currentGameTemplate.StartGame ();

	}

	private void showRoundsResults(){
		Debug.Log ("Time: " + System.DateTime.Now.ToString());
		Debug.Log ("showRoundsResults");
	}

	public void OnFinishGame(List<TaskAnswer> 	answerList){
		Debug.Log ("Time: " + System.DateTime.Now.ToString());
		Debug.Log ("OnFinishGame");
	}

	public void RoundResult(List<TaskAnswer>	answerList, StartNextRoundDelegate nextRoundDelegate){
		Debug.Log ("Time: " + System.DateTime.Now.ToString());
		Debug.Log ("RoundResult");

		userAnswer = answerList [0];

		GameObject.Destroy (currentGameTemplate.gameObject);

		currentGameTemplate = null;

		endTrainingRound.ShowDialog (Utility.trainingFightsArray[currentFight] ,userAnswer, tasksList [0], EndGame, ContinueFight);

		timeInGameSec = (int)clock.stopTime;

		if (userAnswer.Answer > 0) {
			rightAnswersCount++;
		}

		CalculateSQ ();

	}

	IEnumerator SendSQToServer(){
	
		var postScoreURL = NetWorkUtils.buildRequestTSendSQ (rightAnswersCount, timeInGameSec );

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "text/json");

		WWWForm form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (postScoreURL);

		while (!request.isDone) {
			yield return null;
		}

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		if (request.error == null) {

			Debug.Log ("buildRequestTSendSQ done ! " + request.text);

			//tasksList = Utility.GetListOfTasks (request.text, "GetTrainingQuestionResult");
			//initGame ();

		} else {

			Debug.LogError ("WWW Error: " + request.error);
			Debug.LogError ("WWW Error: " + request.text);
			errorPanel = screensManager.ShowErrorDialog("Ошбика соединения с сервером", OnErrorGetTrainingClick);
		}

	}

	private void OnErrorGetTrainingClick(){
	
		screensManager.ShowMainScreen ();
	}
		
	private void NextFight(){

		if (currentFight != Utility.trainingFightsArray.Length - 1) {
			currentFight++;
		} else {
			currentFight = 0;
		}
	}
		
	public void EndGame(){

		ROGA.SetActive (false);
		finishTrainingDialog.ShowDialog ((float)rightAnswersCount - timeInGameSec / 100f, rightAnswersCount, timeInGameSec, closeTraining);
	}

	public void ContinueFight(){
		NextFight ();
		StartCoroutine (StartTraining());
	}
		
	public void closeTraining(){

		if (userAnswer.Answer > 0 && rightAnswersCount > 0 && timeInGameSec >0) {
			StartCoroutine (SendSQToServer());
		}
		ROGA.SetActive (true);
		UserController.reNewStatistic = true;
		SoundManager.ChoosePlayMusic (0);
		screensManager.ShowMainScreen ();
	}

	public void CalculateSQ(){
		double sq = (float)rightAnswersCount - timeInGameSec / 100f;

		if (sq > 0)
			sqText.text = string.Format ("{0:N2}", sq);
		else
			sqText.text = "0";

	}
}
