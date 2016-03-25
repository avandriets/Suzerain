using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameType7 : GameBase {

//	public	Text 					targetNumberTEXT;
//	public	Text 					MyNumberTEXT;

	public CustomNumberInt		TargNumCOMP;
	public CustomNumberText 	MyNumbCOMP;

	public	GameObject				itemButton;
	public	Transform				CardsSetPanel;

	private List<Game7Item> randomItemsList = null;

	private Game7Item 		targetItem		= null;
	private int 			MyNumber;

	protected override void PrepareScreenBeforFinishCall(){

		foreach (Transform child in CardsSetPanel) {
			GameObject.Destroy(child.gameObject);
		}
	}

	protected override void InitGameScreen(){

		randomItemsList 		= new List<Game7Item> ();
		targetItem				= new Game7Item ();
		targetItem.ItemName		= GetTargetNumber ();

		MyNumber				= 0;


		TargNumCOMP.SetNumber (targetItem.ItemName);

		//targetNumberTEXT.text	= targetItem.ItemName.ToString();
		//MyNumberTEXT.text		= "0";

		MyNumbCOMP.SetNumber (0);
		MyNumbCOMP.SetDefaultColor ();

		//Create number list
		for (int i = 1; i <= 9; i++) {

			Game7Item gi = new Game7Item ();
			gi.ItemName = i;


			randomItemsList.Add (gi);
		}

		//shuffle list
		for (int i = 0; i < randomItemsList.Count; i++) {
			Game7Item temp					= randomItemsList[i];
			int randomIndex1				= Random.Range(i, randomItemsList.Count);
			randomItemsList[i]				= randomItemsList[randomIndex1];
			randomItemsList[randomIndex1]	= temp;
		}

		PrepareScreenBeforFinishCall ();

		FillCardSet ();

		inProgress = false;
	}

	private  void FillCardSet(){

		foreach (var c in randomItemsList) {

			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(itemButton) as GameObject;
			Game7TemplateButton button1 = newButtonItem.GetComponent<Game7TemplateButton>();

			button1.item = c;
			button1.Number.text = c.ItemName.ToString ();

			button1.button.onClick.RemoveAllListeners();
			button1.button.onClick.AddListener( () => onCardFromSetClick(button1) );

			newButtonItem.transform.SetParent(CardsSetPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);
		}

	}

	public void onCardFromSetClick(Game7TemplateButton item){

		if (item.CardBack.gameObject.activeSelf) {			
			item.CardBack.gameObject.SetActive (false);
			item.Number.gameObject.SetActive (true);
			AddItemToFound (item.item);

			Debug.Log ("Prived medved.");
		}

	}

	private void AddItemToFound(Game7Item item){
		
		MyNumber += item.ItemName;

		MyNumbCOMP.SetNumber (MyNumber);

		if (MyNumber > targetItem.ItemName)
			MyNumbCOMP.SetRedColor ();
		//MyNumberTEXT.text = MyNumber.ToString ();

	}

	protected override int GetAnswer(){
		return GetGameResult();
	}

	protected override int GetAutoAnswer(){
		return GetGameResult();
	}

	private int GetGameResult(){
		if (targetItem.ItemName >= MyNumber) {
			return MyNumber;
		} else {
			return -1;
		}
	} 

	private int GetTargetNumber(){

		return currentTask.TaskId;
	}
}
