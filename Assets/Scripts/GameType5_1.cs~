using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameType5_1 : GameBase {

	public	GameObject				itemButton;
	public	GameObject				itemSlimButton;

	public	Transform				FoundCardPanel;
	public	Transform				CardsSetPanel;

	List<Game3TemplateButton> 		lettersList;
	List<Game3TemplateButton>		finishLettersList;

	public Text	mQuestionState;

	string[] rawKeys = new string[] {
		"А","Б","В","Г","Д","Е","Ё","Ж","З","И","Й","К","Л","М","Н","О","П","Р","С","Т","У","Ф","Х","Ц", 
		"Ч","Ш","Щ","Ъ","Ы","Ь","Э","Ю","Я"
	};

	protected override void InitGameScreen(){

		//mQuestionState.text = currentTask.TextQuestion;

		string answer = getRightWord();

		string[] arrLett = new string[answer.Length];

		//Create letters array
		for (int k = 0; k < arrLett.Length; k++) {
			if (k < answer.Length) {
				arrLett[k] = answer [k].ToString();
			} else {
				arrLett[k] = rawKeys[Random.Range(0, rawKeys.Length-1)];
			}
		}

		//shuffle letters array
		for (int ij = 0; ij < arrLett.Length; ij++) {
			string temp = arrLett[ij];
			int randomIndex = Random.Range(ij, arrLett.Length);
			arrLett[ij] = arrLett[randomIndex];
			arrLett[randomIndex] = temp;
		}

		PrepareScreenBeforFinishCall ();

		CreateGameObjects (arrLett);

		inProgress = false;

		clock.SetTime (clock.finishTime);
		StartCoroutine (WaitForReading ());
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

			newButtonItem.transform.SetParent(CardsSetPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);

			lettersList.Add (button1);
		}

		string answer = getRightWord();

		for (int i = 0; i < answer.Length; i++) {

			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(itemSlimButton) as GameObject;
			Game3TemplateButton button1 = newButtonItem.GetComponent<Game3TemplateButton>();

			button1.item = null; //new Game3Item(){answer[i].ToString()};

			button1.Letter.text = "";//button1.item.ItemName;// CardFace.gameObject.SetActive (true);
			button1.button.onClick.RemoveAllListeners();
			button1.button.onClick.AddListener( () => onCardFinishClick(button1) );

			newButtonItem.transform.SetParent(FoundCardPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);

			finishLettersList.Add (button1);
		}
	}

	public void onCardFromSetClick(Game3TemplateButton item){

		if (item.Letter.gameObject.activeSelf) {
			foreach (var c in finishLettersList) {
				if (c.item == null) {
					c.item = item.item;
					c.Letter.text = item.item.ItemName;

					c.relateButton = item;
					c.Letter.gameObject.SetActive (true);

					item.Letter.gameObject.SetActive (false);
					break;
				}
			}
		}

		Debug.Log (item.item.ItemName);
	}

	public void onCardFinishClick(Game3TemplateButton item){

		if (item.item != null) {
			item.item = null;
			item.Letter.gameObject.SetActive (false);
			item.relateButton.Letter.gameObject.SetActive (true);
		}

	}

	public void OnBackSpaceClick(){

		for (int i = finishLettersList.Count - 1; i >= 0; i--) {

			if (finishLettersList [i].item != null) {

				if (finishLettersList [i].Letter.gameObject.activeSelf) {
					finishLettersList [i].item = null;
					finishLettersList [i].Letter.gameObject.SetActive (false);
					finishLettersList [i].relateButton.Letter.gameObject.SetActive (true);
					break;
				}
			}
		}
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
