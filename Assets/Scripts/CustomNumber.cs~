using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CustomNumber : MonoBehaviour {

	private string[] numbersArray = new string[10]{"number/0","number/1", "number/2", "number/3", "number/4", "number/5", "number/6", "number/7", "number/8", "number/9"};

	public Image num1; 
	public Image num2; 
	public Image digit1; 
	public Image difit2; 

	public void SetNumber(float number){
	
		int sec1, sec2, milsec1, milsec2;
		float partAfterPoint;

		sec1 = (int)number / 10;
		sec2 = (int)number - sec1*10;
		partAfterPoint = (number - Mathf.Floor (number)) * 100;
		milsec1 = (int)partAfterPoint / 10;
		milsec2 = (int)partAfterPoint - milsec1 * 10;

		setImage (num1, sec1);
		setImage (num2, sec2);
		setImage (digit1, milsec1);
		setImage (difit2, milsec2);

	}

	private void setImage(Image targImg, int number){
		targImg.sprite = getResourceImage(numbersArray[number]);
	}

	private Sprite getResourceImage(string numberPath){
		
		Texture2D texture = Resources.Load(numberPath) as Texture2D;
		if(texture != null){
			Sprite spr = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),new Vector2(0.5f, 0.5f));
			return spr;
		}

		return null;
	}
}
