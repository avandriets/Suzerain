using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameType6 : GameBase {
	 
	public	GameObject				itemButtonTop;
	public	GameObject				itemButtonButtom;

	public	Transform				CardsSetPanel;
	public	Transform				FoundCardPanel;

	public static List<Game6Item>	set1List = new List<Game6Item>(){
		new Game6Item(){ItemName = "1",  FilePath = "Game6/1"},
		new Game6Item(){ItemName = "2",  FilePath = "Game6/2"},
		new Game6Item(){ItemName = "3",  FilePath = "Game6/3"},
		new Game6Item(){ItemName = "4",  FilePath = "Game6/4"},
		new Game6Item(){ItemName = "5",  FilePath = "Game6/5"},
		new Game6Item(){ItemName = "6",  FilePath = "Game6/6"},
		new Game6Item(){ItemName = "7",  FilePath = "Game6/7"}
	};

	public static List<Game6Item>	set2List = new List<Game6Item>(){
		new Game6Item(){ItemName = "8",  FilePath = "Game6/8"},
		new Game6Item(){ItemName = "9",  FilePath = "Game6/9"},
		new Game6Item(){ItemName = "10", FilePath = "Game6/10"},
		new Game6Item(){ItemName = "11", FilePath = "Game6/11"},
		new Game6Item(){ItemName = "12", FilePath = "Game6/12"},
		new Game6Item(){ItemName = "13", FilePath = "Game6/13"},
		new Game6Item(){ItemName = "14", FilePath = "Game6/14"}
	};

	public static List<Game6Item>	set3List = new List<Game6Item>(){
		new Game6Item(){ItemName = "21",  FilePath = "Game6/21"},
		new Game6Item(){ItemName = "22",  FilePath = "Game6/22"},
		new Game6Item(){ItemName = "23",  FilePath = "Game6/23"},
		new Game6Item(){ItemName = "24",  FilePath = "Game6/24"},
		new Game6Item(){ItemName = "25",  FilePath = "Game6/25"},
		new Game6Item(){ItemName = "26",  FilePath = "Game6/26"},
		new Game6Item(){ItemName = "27",  FilePath = "Game6/27"}
	};

	private List<Game6Item>				etalon = null;
	private List<Game6Item>				CardSet = null;

	private List<Game6TemplateButton>	answerListGameItem6 = null;

	protected override void PrepareScreenBeforFinishCall(){

		foreach (Transform child in FoundCardPanel) {
			GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in CardsSetPanel) {
			GameObject.Destroy(child.gameObject);
		}

	}

	protected override void InitGameScreen(){

		//Delay = 5;

		int type = Random.Range (0, 3);
		etalon	= new List<Game6Item> ();
		CardSet = new List<Game6Item> ();
		answerListGameItem6 = new List<Game6TemplateButton> ();

		if (type == 0) {
			foreach (var c in GameType6.set1List) {
				etalon.Add (c);
				CardSet.Add (c);
			}
		}else if(type == 1){
			foreach (var c in GameType6.set2List) {
				etalon.Add (c);
				CardSet.Add (c);
			}
		}else {
			foreach (var c in GameType6.set3List) {
				etalon.Add (c);
				CardSet.Add (c);
			}
		}

		for (int i = 0; i < etalon.Count; i++) {
			Game6Item temp = etalon[i];
			int randomIndex1				= Random.Range(i, etalon.Count);
			etalon[i]				= etalon[randomIndex1];
			etalon[randomIndex1]	= temp;
		}

		PrepareScreenBeforFinishCall ();

		clock.SetTime (clock.finishTime);

		FillLists ();

		inProgress = false;
	}

	public void FillLists(){
	
		foreach (var c in CardSet) {

			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(itemButtonButtom) as GameObject;
			Game6TemplateButton button1 = newButtonItem.GetComponent<Game6TemplateButton>();

			button1.item = c;
			Texture2D texture = Resources.Load(c.FilePath) as Texture2D;
			if(texture != null){
				Sprite spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
				button1.CardFace.sprite = spr;
			}

			button1.CardFace.gameObject.SetActive (true);
			button1.button.onClick.RemoveAllListeners();
			button1.button.onClick.AddListener( () => onCardFromSetClick(button1) );

			newButtonItem.transform.SetParent(CardsSetPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);
		}

		foreach (var cc in etalon) {
			
			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(itemButtonButtom) as GameObject;
			Game6TemplateButton button1 = newButtonItem.GetComponent<Game6TemplateButton>();

			button1.item = cc;
			Texture2D texture = Resources.Load(cc.FilePath) as Texture2D;
			if(texture != null){
				Sprite spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
				button1.CardFace.sprite = spr;
				button1.CardFace.gameObject.SetActive (true);
			}

			button1.button.onClick.RemoveAllListeners();
			button1.button.onClick.AddListener( () => onCardFromFoundClick(button1) );

			newButtonItem.transform.SetParent(FoundCardPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);

			answerListGameItem6.Add (button1);
		}

		StartCoroutine (FinishFill ());
	}

	IEnumerator FinishFill(){
	
		isActiveForm = false;
		yield return new WaitForSeconds(5);
		isActiveForm = true;

		foreach (var c in answerListGameItem6) {
			c.item = null;
			c.HideImage ();
		}

		clock.StartTimer (onAnswer);
	}

	public void onCardFromSetClick(Game6TemplateButton item){

		if (item.CardFace.gameObject.activeSelf && isActiveForm ) {
			foreach (var c in answerListGameItem6) {
				if (c.item == null) {
					c.item = item.item;
					c.InitImage ();
					c.relateButton = item;
					c.CardFace.gameObject.SetActive (true);

					item.CardFace.gameObject.SetActive (false);
					break;
				}
			}
		}

		Debug.Log ("Prived midved.");
	}

	public void onCardFromFoundClick(Game6TemplateButton item){

		if (item.item != null && isActiveForm) {
			item.HideImage ();
			item.CardFace.gameObject.SetActive (false);
			item.relateButton.CardFace.gameObject.SetActive (true);
		}

		Debug.Log ("Prived midved 2.");
	}

	public override void StartGame(){
	}

	private int CountFoundItems(){
		
		int count = 0;

		for (int i = 0; i < etalon.Count; i++) {
			if(answerListGameItem6[i].item != null){
				if (etalon [i].ItemName == answerListGameItem6 [i].item.ItemName) {
					count += 1;
					//break;
				}
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
