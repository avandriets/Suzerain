using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Game7TemplateButton : MonoBehaviour {

	public Button	button;

	public Image 	CardFace;
	public Image 	CardBack;
	public Text 	Number;

	[HideInInspector]
	public Game7Item		item;
}
