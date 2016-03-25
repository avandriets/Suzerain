using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameType5 : GameBase {

	public	Image 					targetImage;
	public	GameObject				itemButton;
	public	Transform				CardsSetPanel;
	public	Transform				FoundCardPanel;
	public	Text 					Question;

	public static List<Game5Item>	gameItemsList = new List<Game5Item>(){
		new Game5Item(){ItemName = "1",  FilePath = "Game5/1"},
		new Game5Item(){ItemName = "2",  FilePath = "Game5/2"},
		new Game5Item(){ItemName = "3",  FilePath = "Game5/3"},
		new Game5Item(){ItemName = "4",  FilePath = "Game5/4"},
		new Game5Item(){ItemName = "5",  FilePath = "Game5/5"},
		new Game5Item(){ItemName = "6",  FilePath = "Game5/6"},
		new Game5Item(){ItemName = "7",  FilePath = "Game5/7"},
		new Game5Item(){ItemName = "8",  FilePath = "Game5/8"},
		new Game5Item(){ItemName = "9",  FilePath = "Game5/9"},
		new Game5Item(){ItemName = "10", FilePath = "Game5/10"},
		new Game5Item(){ItemName = "11", FilePath = "Game5/11"},
		new Game5Item(){ItemName = "12", FilePath = "Game5/12"},
		new Game5Item(){ItemName = "13", FilePath = "Game5/13"},
		new Game5Item(){ItemName = "14", FilePath = "Game5/14"}
	};

	private List<Game5Item> randomItemsList = null;
	private List<Game5Item> finalItemsList	= null;
	private Game5Item 		targetItem		= null;
	private int 			foundItemsCount = 0;
	private int 			limitItems = 0;

	protected override void PrepareScreenBeforFinishCall(){

		foreach (Transform child in FoundCardPanel) {
			GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in CardsSetPanel) {
			GameObject.Destroy(child.gameObject);
		}

	}

	protected override void InitGameScreen(){

		Delay = 1;
		foundItemsCount = 0;
		int randomIndex = -1;
		targetItem = GameType5.gameItemsList [Random.Range (0, 13)];

		limitItems = currentTask.TaskId;

		finalItemsList = new List<Game5Item> ();
		randomItemsList = new List<Game5Item> ();

		for (int i = 0; i < 14; i++) {
			if (i < 5) {
				randomItemsList.Add (targetItem);
			} else {

				randomIndex = Random.Range (0, 13);
				while (randomIndex == GameType5.gameItemsList.IndexOf (targetItem)) {
					randomIndex = Random.Range (0, 13);
				}
				randomItemsList.Add (GameType5.gameItemsList[randomIndex]);
			}
		}

		for (int i = 0; i < randomItemsList.Count; i++) {
			Game5Item temp = randomItemsList[i];
			int randomIndex1				= Random.Range(i, randomItemsList.Count);
			randomItemsList[i]				= randomItemsList[randomIndex1];
			randomItemsList[randomIndex1]	= temp;
		}

		Texture2D texture = Resources.Load(targetItem.FilePath) as Texture2D;
		if(texture != null){
			Sprite spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
			targetImage.sprite = spr;
		}

		Question.text = "За " + limitItems.ToString() + " попыток найдите как можно большее количество следующих символов";		 

		PrepareScreenBeforFinishCall ();

		FillCardList ();


		inProgress = false;
	}

	private  void FillCardList(){

		foreach (var c in randomItemsList) {
			
			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(itemButton) as GameObject;
			Game5TemplateButton button1 = newButtonItem.GetComponent<Game5TemplateButton>();

			button1.item = c;
			Texture2D texture = Resources.Load(c.FilePath) as Texture2D;
			if(texture != null){
				Sprite spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
				button1.CardItem.sprite = spr;
			}

			button1.button.onClick.RemoveAllListeners();
			button1.button.onClick.AddListener( () => onCardFromSetClick(button1) );


			newButtonItem.transform.SetParent(CardsSetPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);
		}

	}

	private void AddItemToFound(Game5Item item){

		if (foundItemsCount < limitItems) {
			
			GameObject	newButtonItem = null;
			newButtonItem = Instantiate (itemButton) as GameObject;
			Game5TemplateButton button1 = newButtonItem.GetComponent<Game5TemplateButton> ();

			button1.item = item;
			Texture2D texture = Resources.Load (item.FilePath) as Texture2D;
			if (texture != null) {
				Sprite spr = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
				button1.CardItem.sprite = spr;
			}

			if (item.ItemName == targetItem.ItemName) {
				button1.CardBack.gameObject.SetActive (false);
				button1.CardItem.gameObject.SetActive (true);
			}

			button1.button.onClick.RemoveAllListeners ();
			button1.button.onClick.AddListener (() => onCardFoundClick (button1));

			newButtonItem.transform.SetParent (FoundCardPanel);
			newButtonItem.transform.localScale = new Vector3 (1, 1, 1);

			finalItemsList.Add (item);

			foundItemsCount++;

			if (foundItemsCount == limitItems) {
				onAnswer (false);
			}
		} 

	}

	public void onCardFromSetClick(Game5TemplateButton item){

		if (foundItemsCount < limitItems && item.CardBack.gameObject.activeSelf) {
			
			item.CardBack.gameObject.SetActive (false);
			item.CardItem.gameObject.SetActive (true);

			AddItemToFound (item.item);

			Debug.Log ("Prived medved.");
		}
	}

	public void onCardFoundClick(Game5TemplateButton item){

		Debug.Log ("Prived medved 2.");
	}

	private int CountFoundItems(){
		int count = 0;
		foreach (var c in finalItemsList) {
			if (c.ItemName == targetItem.ItemName) {
				count++;
			}
		}

		return count;
	}

	protected override int GetAnswer(){
		return CountFoundItems ();
	}

	protected override int GetAutoAnswer(){
		return CountFoundItems ();
	}
}
