using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CustomNumberText : MonoBehaviour {

	public Text num1; 
	public Text num2; 

	Color32 defaultColor = new Color32(227,	189, 141, 255);

	public void SetNumber(int number){

		int sec1, sec2;
		//float partAfterPoint;

		sec1 = (int)number / 10;
		sec2 = (int)number - sec1*10;

		num1.text = sec1.ToString ();
		num2.text = sec2.ToString ();
	}

	public void SetRedColor(){
		num1.color = Color.red;
		num2.color = Color.red;
	}


	public void SetDefaultColor(){
		
		num1.color = defaultColor;
		num2.color = defaultColor;

	}

}
