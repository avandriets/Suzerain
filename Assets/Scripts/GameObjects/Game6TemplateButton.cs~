using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game6TemplateButton : MonoBehaviour {

	public Button	button;

	public Image 	CardFace;
	public Image 	CardBack;

	public Game6Item		item;

	public Game6TemplateButton relateButton;

	public void InitImage(){
		
		Texture2D texture = Resources.Load(item.FilePath) as Texture2D;
		if(texture != null){
			Sprite spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
			CardFace.sprite = spr;
			CardFace.gameObject.SetActive (true);
		}

	}

	public void HideImage(){
		item = null;
		CardFace.gameObject.SetActive (false);
	}
}
