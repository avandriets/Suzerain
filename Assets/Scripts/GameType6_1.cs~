using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.UI;

public class GameType6_1 : GameBase {

	public ToggleGroup 	mAnswerGroup;
	public Toggle 		mTogleAnwer1;
	public Toggle 		mTogleAnwer2;

	public Text	mQuestionState;
	public Text	LabelAnsw1;
	public Text	LabelAnsw2;

	protected override void InitGameScreen(){

		inProgress = true;

		SetVisibility ();

//		Texture2D tex = null;

		mAnswerGroup.SetAllTogglesOff ();

		mQuestionState.text = System.Text.RegularExpressions.Regex.Unescape(currentTask.TextQuestion);
		LabelAnsw1.text = currentTask.Ans1;
		LabelAnsw2.text = currentTask.Ans2;

		mTogleAnwer1.enabled = true;
		mTogleAnwer2.enabled = true;

		inProgress = false;

		clock.SetTime (clock.finishTime);
		StartCoroutine (WaitForReading ());

	}

	protected new IEnumerator WaitForReading(){

		inProgress = true;
		isActiveForm = false;

		mTogleAnwer1.enabled = false;
		mTogleAnwer2.enabled = false;

		yield return new WaitForSeconds(0);

		isActiveForm = true;
		inProgress = false;

		mTogleAnwer1.enabled = true;
		mTogleAnwer2.enabled = true;

		clock.StartTimer (onAnswer);
	}

	protected override void PrepareScreenBeforFinishCall(){

		mAnswerGroup.SetAllTogglesOff ();

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

		active = mAnswerGroup.ActiveToggles ();
		componentName = "";

		foreach (UnityEngine.UI.Toggle i in active) {		
			componentName = i.name;
		}

		if (mTogleAnwer1.name == componentName) {
			return 1;
		} else if (mTogleAnwer2.name == componentName) {
			return 2;
		}

		return 0;
	}

	public void SetVisibility(){

		mAnswerGroup.gameObject.SetActive (true);

	}
}
