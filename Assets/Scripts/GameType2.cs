using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameType2 : GameBase {

	public	GameObject				itemButton;

	public	Transform				FoundCardPanel;
	public	Transform				CardsSetPanel;

	List<Game3TemplateButton> 		lettersList;
	List<Game3TemplateButton>		finishLettersList;

	public Text	mQuestionState;

	protected override void InitGameScreen(){

		mQuestionState.text = currentTask.TextQuestion;

		//string answer = getRightWord();

		string[] arrLett = new string[10]{"1","2","3","4","5","6","7","8","9", "0"};

		PrepareScreenBeforFinishCall ();

		CreateGameObjects (arrLett);

		inProgress = false;
	}

	protected override void PrepareScreenBeforFinishCall(){

		foreach (Transform child in FoundCardPanel) {
			GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in CardsSetPanel) {
			GameObject.Destroy(child.gameObject);
		}

	}

	private void CreateGameObjects(string[] pLettersArray){

		lettersList			= new List<Game3TemplateButton>();
		finishLettersList	= new List<Game3TemplateButton>();

		//Bottom array
		foreach (var c in pLettersArray) {

			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(itemButton) as GameObject;
			Game3TemplateButton button1 = newButtonItem.GetComponent<Game3TemplateButton>();

			Game3Item gItem = new Game3Item ();
			gItem.ItemName = c;
			button1.item = gItem;

			button1.Letter.text = button1.item.ItemName;// CardFace.gameObject.SetActive (true);
			button1.button.onClick.RemoveAllListeners();
			button1.button.onClick.AddListener( () => onCardFromSetClick(button1) );
			//button1.button.onClick.AddListener( onButtonClickPlay );


			newButtonItem.transform.SetParent(CardsSetPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);

			lettersList.Add (button1);
		}

		string answer = getRightWord();

		for (int i = 0; i < answer.Length; i++) {

			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(itemButton) as GameObject;
			Game3TemplateButton button1 = newButtonItem.GetComponent<Game3TemplateButton>();

			button1.item = null; //new Game3Item(){answer[i].ToString()};

			button1.Letter.text = "";//button1.item.ItemName;// CardFace.gameObject.SetActive (true);
			button1.button.onClick.RemoveAllListeners();
			button1.button.onClick.AddListener( () => onCardFinishClick(button1) );
			//button1.button.onClick.AddListener( onButtonClickPlay );

			newButtonItem.transform.SetParent(FoundCardPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);

			finishLettersList.Add (button1);
		}
	}

	public void onCardFromSetClick(Game3TemplateButton item){


		//if (item.Letter.gameObject.activeSelf) {
			foreach (var c in finishLettersList) {
				if (c.item == null) {
					c.item = item.item;
					c.Letter.text = item.item.ItemName;

					c.relateButton = item;
					c.Letter.gameObject.SetActive (true);

					//item.Letter.gameObject.SetActive (false);
					break;
				}
			}
		//}

		//item.Letter.gameObject.SetActive (false);

		Debug.Log (item.item.ItemName);
	}

	public void onCardFinishClick(Game3TemplateButton item){

		if (item.item != null) {
			//item.HideImage ();
			item.item = null;
			item.Letter.gameObject.SetActive (false);
			item.relateButton.Letter.gameObject.SetActive (true);
		}

		Debug.Log (item.item.ItemName);
	}

	protected override int GetAnswer(){

		if (getRightWord () == GetAnswerWord ()) {
			return currentTask.TrueValue;
		} else {
			return (-1)*currentTask.TrueValue;
		}

	}

	private string getRightWord(){

		if (currentTask.TrueValue == 1) {
			return currentTask.Ans1.ToUpper().Trim();
		}else if (currentTask.TrueValue == 2) {
			return currentTask.Ans2.ToUpper().Trim();
		}else if (currentTask.TrueValue == 3) {
			return currentTask.Ans3.ToUpper().Trim();
		}else if (currentTask.TrueValue == 4) {
			return currentTask.Ans4.ToUpper().Trim();
		}

		return "";
	}

	private string GetAnswerWord(){
		string word = "";

		foreach(var cc in finishLettersList){
			if (cc.gameObject.activeSelf && cc.item != null) {
				word += cc.Letter.text;
			}
		}

		return word.ToUpper();
	}
}
