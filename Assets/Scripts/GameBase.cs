using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;

public abstract class GameBase : MonoBehaviour {

	protected List<TaskAnswer> 		answerList 	= null;
	protected List<TestTask> 		tasksList 	= null;
	protected TestTask 				currentTask = null;

	protected FinishAnswerDelegate 		finishDelegate;
	protected RoundResultDelegate		rounfResultDelegate;
	protected RefreshGameScreen			refreshActionGameScreen;
	protected RoundResultDialog			roundResultDialog = null;

	protected ClockObject				clock;
	protected bool						inProgress = false;

	protected ScreensManager			screenManager;

	protected int 	Delay = 0;
	protected bool	isActiveForm = true;
	protected OnlineGame	ong;

	public void InitGameObject(List<TestTask> pTasksList, FinishAnswerDelegate pFinishDelegate, RoundResultDelegate roundRes,RefreshGameScreen refreshAction , ClockObject	pClock, int GameDuration){

		screenManager 	= ScreensManager.instance;
		ong				= OnlineGame.instance;

		answerList 		= null;
		tasksList		= pTasksList;

		finishDelegate			= pFinishDelegate;
		rounfResultDelegate		= roundRes;
		refreshActionGameScreen = refreshAction;

		currentTask = tasksList [0];
		clock = pClock;

		clock.stopTime = 0;
		clock.finishTime = GameDuration;
		clock.GameType = 0;

		inProgress = false;

		InitGameScreen ();
	}

	protected abstract void InitGameScreen ();

	public virtual void StartGame(){
		clock.StartTimer (onAnswer);
	}

	public void onAnswer(bool autoAnswer){

		if (inProgress || isActiveForm == false)
			return;

		inProgress = true;

		clock.StopTimer ();

		float currentTime = clock.stopTime;

		AddAnswerToList (currentTask, currentTime, autoAnswer);

		if (tasksList.IndexOf (currentTask) != tasksList.Count - 1) {
			currentTask = tasksList [tasksList.IndexOf (currentTask) + 1];
			SendRoundResult ();
		} else {
			SendRoundResult ();
		}
	}

	protected void SendRoundResult(){
		Debug.Log ("SendRoundResult");
		StartCoroutine (PauseBeforeRoundResult());
	}

	protected IEnumerator PauseBeforeRoundResult(){
		Debug.Log ("PauseBeforeRoundResult");
		if (Delay > 0) {
			isActiveForm = false;
			yield return new WaitForSeconds (Delay);
			isActiveForm = true;
		}

		rounfResultDelegate (answerList, NextRound);
	}

	protected void NextRound(List<Fight> fightResult){
		Debug.Log ("NextRound");
		StartCoroutine (StartNextRound(fightResult));
	}

	protected IEnumerator StartNextRound(List<Fight> fightResult){

		Debug.Log ("StartNextRound");
		refreshActionGameScreen ();

		//Show round result screen for more than one rounds
		if (tasksList.Count > 1) {
			roundResultDialog = screenManager.ShowResultDialog (fightResult, answerList, tasksList);
			yield return new WaitForSeconds (3);
			screenManager.CloseResultDialogPanel (roundResultDialog);
		}

		if (answerList.Count != tasksList.Count && ong.currentFight.FightState == -1){

			Debug.Log ("InitGameScreen");
			InitGameScreen ();
			StartGame ();

		} else {

			Debug.Log ("PrepareScreenBeforFinishCall");
			PrepareScreenBeforFinishCall ();

			finishDelegate (answerList);
		}

	}

	protected virtual void PrepareScreenBeforFinishCall(){
	}

	protected void AddAnswerToList(TestTask pTask, float time, bool autoAnswer){

		OnAnswerExecute ();

		if(answerList == null){
			answerList = new List<TaskAnswer>();			
		}

		TaskAnswer answer = new TaskAnswer();
		answer.FightId		= pTask.FightId;
		answer.QNum			= pTask.QNum;

		if(!autoAnswer){

			answer.Answer = GetAnswer();
			int getScoreMember = GetScore ();
			if (getScoreMember == -9999) {
				answer.Score = ((int)(time * 1000));
			} else {
				answer.Score = getScoreMember;
			}

		}else{
			int autoAnswerMember = GetAutoAnswer ();
			if (autoAnswerMember == -9999) {
				answer.Answer = -1;
			} else {
				answer.Answer = autoAnswerMember;
			}
			answer.Score		= 999999;
		}

		answerList.Add(answer);

	}

	protected virtual int GetAutoAnswer(){
		return -9999;
	}

	protected virtual void OnAnswerExecute(){
	}

	protected virtual int GetScore (){
		return -9999;
	}

	protected abstract int GetAnswer ();

}
