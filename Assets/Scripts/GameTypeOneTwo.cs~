using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;

public delegate void StartNextRoundDelegate(List<Fight> fight);
public delegate void FinishAnswerDelegate(List<TaskAnswer> 	answerList);
public delegate void RoundResultDelegate(List<TaskAnswer> 	answerList, StartNextRoundDelegate nextRoundDelegate);

public class GameTypeOneTwo : GameBase {

	public ToggleGroup 	mAnswerGroup;
	public Toggle 		mTogleAnwer1;
	public Toggle 		mTogleAnwer2;
	public Toggle 		mTogleAnwer3;
	public Toggle 		mTogleAnwer4;

	public ToggleGroup 	mAnswerGroupGame2;
	public Toggle 		mTogleAnwer1Game2;
	public Toggle 		mTogleAnwer2Game2;
	public Toggle 		mTogleAnwer3Game2;
	public Toggle 		mTogleAnwer4Game2;

	public ToggleGroup 	mAnswerGroupGame3;
	public Toggle 		mTogleAnwer1Game3;
	public Toggle 		mTogleAnwer2Game3;
	public Toggle 		mTogleAnwer3Game3;
	public Toggle 		mTogleAnwer4Game3;

	public Text	mQuestionState;
	public Text	LabelAnsw1;
	public Text	LabelAnsw2;
	public Text	LabelAnsw3;
	public Text	LabelAnsw4;

	public Text	mQuestionType2;
	public Image mQuestionStateGame2;
	public Text	LabelAnsw1Game2;
	public Text	LabelAnsw2Game2;
	public Text	LabelAnsw3Game2;
	public Text	LabelAnsw4Game2;

	public Text	mQuestionType3;
	public Image mQuestionStateGame3;
	public Image	LabelAnsw1Game3;
	public Image	LabelAnsw2Game3;
	public Image	LabelAnsw3Game3;
	public Image	LabelAnsw4Game3;


	public int GameType(){
		
		if (currentTask.PicQuestion == null && currentTask.Var1 == null) {
			return 1;
		} else if (currentTask.PicQuestion != null && currentTask.Var1 == null) {
			return 2;
		} else if (currentTask.PicQuestion != null && currentTask.Var1 != null) {
			return 3;
		} else {
			return -1;
		}
	}

	protected override void InitGameScreen(){

		inProgress = true;

		SetVisibility ();

		Texture2D tex = null;

		mAnswerGroup.SetAllTogglesOff ();
		mAnswerGroupGame2.SetAllTogglesOff ();
		mAnswerGroupGame3.SetAllTogglesOff ();

		if (GameType () == 1) {
			mQuestionState.text = System.Text.RegularExpressions.Regex.Unescape(currentTask.TextQuestion);
			LabelAnsw1.text = currentTask.Ans1;
			LabelAnsw2.text = currentTask.Ans2;
			LabelAnsw3.text = currentTask.Ans3;
			LabelAnsw4.text = currentTask.Ans4;

			mTogleAnwer1.enabled = true;
			mTogleAnwer2.enabled = true;
			mTogleAnwer3.enabled = true;
			mTogleAnwer4.enabled = true;

		} else if (GameType () == 2) {
		
			mQuestionType2.text = currentTask.TextQuestion;
			tex = new Texture2D(2, 2);
			tex.LoadImage(currentTask.PicQuestion);

			if (tex != null) {
				mQuestionStateGame2.sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			}

			LabelAnsw1Game2.text = currentTask.Ans1;
			LabelAnsw2Game2.text = currentTask.Ans2;
			LabelAnsw3Game2.text = currentTask.Ans3;
			LabelAnsw4Game2.text = currentTask.Ans4;

			mTogleAnwer1Game2.enabled = true;
			mTogleAnwer2Game2.enabled = true;
			mTogleAnwer3Game2.enabled = true;
			mTogleAnwer4Game2.enabled = true;

		} else if (GameType () == 3) {
		
			mQuestionType3.text = currentTask.TextQuestion;

			tex = new Texture2D(2, 2);
			tex.LoadImage(currentTask.PicQuestion);

			if (tex != null) {
				mQuestionStateGame3.sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			}

			//ans1
			tex = new Texture2D(2, 2);
			tex.LoadImage(currentTask.Var1);

			if (tex != null) {
				LabelAnsw1Game3.sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			}

			//ans2
			tex = new Texture2D(2, 2);
			tex.LoadImage(currentTask.Var2);

			if (tex != null) {
				LabelAnsw2Game3.sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			}

			//ans3
			tex = new Texture2D(2, 2);
			tex.LoadImage(currentTask.Var3);

			if (tex != null) {
				LabelAnsw3Game3.sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			}

			//ans4
			tex = new Texture2D(2, 2);
			tex.LoadImage(currentTask.Var4);

			if (tex != null) {
				LabelAnsw4Game3.sprite = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
			}

			mTogleAnwer1Game3.enabled = true;
			mTogleAnwer2Game3.enabled = true;
			mTogleAnwer3Game3.enabled = true;
			mTogleAnwer4Game3.enabled = true;

		}

		inProgress = false;

		clock.SetTime (clock.finishTime);
		StartCoroutine (WaitForReading ());
	}

	protected IEnumerator WaitForReading(){

		inProgress = true;
		isActiveForm = false;

		mTogleAnwer1.enabled = false;
		mTogleAnwer2.enabled = false;
		mTogleAnwer3.enabled = false;
		mTogleAnwer4.enabled = false;

		yield return new WaitForSeconds(3);

		isActiveForm = true;
		inProgress = false;

		mTogleAnwer1.enabled = true;
		mTogleAnwer2.enabled = true;
		mTogleAnwer3.enabled = true;
		mTogleAnwer4.enabled = true;

		clock.StartTimer (onAnswer);
	}

	protected override void PrepareScreenBeforFinishCall(){
		
		mAnswerGroup.SetAllTogglesOff ();
		mAnswerGroupGame2.SetAllTogglesOff ();
		mAnswerGroupGame3.SetAllTogglesOff ();

	}

	protected override int GetAnswer(){
		
		if(currentTask.TrueValue == GetNumAnswer ()){
			return GetNumAnswer ();
		}else{
			return (-1)*GetNumAnswer ();
		}
	}

	private int GetNumAnswer(){

		IEnumerable<UnityEngine.UI.Toggle>	active;
		string 								componentName;

		if (GameType () == 1) {

			active = mAnswerGroup.ActiveToggles ();
			componentName = "";

			foreach (UnityEngine.UI.Toggle i in active) {		
				componentName = i.name;
			}

			if (mTogleAnwer1.name == componentName) {
				return 1;
			} else if (mTogleAnwer2.name == componentName) {
				return 2;
			} else if (mTogleAnwer3.name == componentName) {
				return 3;
			} else if (mTogleAnwer4.name == componentName) {
				return 4;
			}

		} else if (GameType () == 2) {

			active = mAnswerGroupGame2.ActiveToggles ();
			componentName = "";

			foreach (UnityEngine.UI.Toggle i in active) {		
				componentName = i.name;
			}

			if (mTogleAnwer1Game2.name == componentName) {
				return 1;
			} else if (mTogleAnwer2Game2.name == componentName) {
				return 2;
			} else if (mTogleAnwer3Game2.name == componentName) {
				return 3;
			} else if (mTogleAnwer4Game2.name == componentName) {
				return 4;
			}

		} else if (GameType () == 3) {
		
			active = mAnswerGroupGame3.ActiveToggles ();
			componentName = "";

			foreach (UnityEngine.UI.Toggle i in active) {		
				componentName = i.name;
			}

			if (mTogleAnwer1Game3.name == componentName) {
				return 1;
			} else if (mTogleAnwer2Game3.name == componentName) {
				return 2;
			} else if (mTogleAnwer3Game3.name == componentName) {
				return 3;
			} else if (mTogleAnwer4Game3.name == componentName) {
				return 4;
			}

		}

		return 0;
	}

	public void SetVisibility(){
	
		if (GameType () == 1) {
			mAnswerGroup.gameObject.SetActive (true);
			mAnswerGroupGame2.gameObject.SetActive (false);
			mAnswerGroupGame3.gameObject.SetActive (false);
		} else if (GameType () == 2) {
			mAnswerGroup.gameObject.SetActive (false);
			mAnswerGroupGame2.gameObject.SetActive (true);
			mAnswerGroupGame3.gameObject.SetActive (false);
		} else if (GameType () == 3) {
			mAnswerGroup.gameObject.SetActive (false);
			mAnswerGroupGame2.gameObject.SetActive (false);
			mAnswerGroupGame3.gameObject.SetActive (true);
		}
	}
}
