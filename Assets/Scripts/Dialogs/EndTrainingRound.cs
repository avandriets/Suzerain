using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using Soomla.Store;

public class EndTrainingRound : MonoBehaviour {

	public GameObject 	mPanelObject;

	public Button 		endButton;
	public Button 		nextRoundButton;

	public GameObject	rightAnswerObject;
	public GameObject	wrongAnswerObject;
	public GameObject	buyButton;
	public Text			rightAnswer;

	public Text			askMoney;

	public Image oneTalant;
	public Image twoTalant;

	int fightType;

	ErrorPanel errorPanel;

	public void ShowDialog(int pFightType, TaskAnswer userAnswer, TestTask testTask, UnityAction endEvent, UnityAction nextRoundEvent){
		mPanelObject.SetActive (true);

		rightAnswer.text = testTask.GetRightAnswer ();

		fightType = pFightType;

		if (userAnswer.Answer < 0 && fightType != Utility.Iridizia) {

			if (fightType == Utility.Mudrost || fightType == Utility.Logic || fightType == Utility.Razum) {
				askMoney.text = string.Format ("открыть за {0} таланта", 2);
				twoTalant.gameObject.SetActive (true);
			} else {
				askMoney.text = string.Format ("открыть за {0} талант", 1);
				oneTalant.gameObject.SetActive (true);
			}

			buyButton.gameObject.SetActive (true);
			wrongAnswerObject.gameObject.SetActive (true);
		} else {
			rightAnswerObject.gameObject.SetActive (true);
		}

		endButton.onClick.RemoveAllListeners();
		endButton.onClick.AddListener (endEvent);
		endButton.onClick.AddListener (ClosePanel);
		endButton.gameObject.SetActive (true);

		nextRoundButton.onClick.RemoveAllListeners();
		nextRoundButton.onClick.AddListener (nextRoundEvent);
		nextRoundButton.onClick.AddListener (ClosePanel);
		nextRoundButton.gameObject.SetActive (true);
	}

	public void showRightAnswer(){

		if (Storage.GetMoneyBalance () > 0) {
			
			buyButton.gameObject.SetActive (false);
			rightAnswer.gameObject.SetActive (true);

			if (fightType == Utility.Mudrost || fightType == Utility.Logic || fightType == Utility.Razum) {
				Storage.TakeMoney (2);
			} else {
				Storage.TakeMoney (1);
			}
		} else {
			ScreensManager scr = ScreensManager.instance;
			errorPanel = scr.ShowErrorDialog ("На Вашем счету сейчас нет талантов. Пожалуйста, добудьте таланты в дуэлях или приобретите в магазине.", closeErrorPanel);
		}
	}

	private void closeErrorPanel(){

		GameObject.Destroy (errorPanel);
		errorPanel = null;

	}

	public void ClosePanel () {

		oneTalant.gameObject.SetActive (false);
		twoTalant.gameObject.SetActive (false);

		buyButton.gameObject.SetActive(true);
		rightAnswer.gameObject.SetActive (false);

		rightAnswerObject.gameObject.SetActive (false);
		wrongAnswerObject.gameObject.SetActive (false);
		mPanelObject.SetActive (false);
	}
}
