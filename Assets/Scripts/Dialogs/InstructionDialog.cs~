using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class InstructionDialog : MonoBehaviour {

	public GameObject 	instructionPanelObject;
	public GameObject	buttonPrev;
	public GameObject	buttonNext;
	public Button		buttonOk;
	public Button		buttonClose;

	public List<GameObject> 	inagesList;
	int currentSlide;

	public void ShowDialog(UnityAction yesEvent){

		instructionPanelObject.SetActive (true);
		currentSlide = 0;

		inagesList [currentSlide].gameObject.SetActive (true);
				 
		buttonPrev.SetActive (false);
		buttonNext.SetActive (true);
		buttonOk.gameObject.SetActive (false);

		buttonOk.onClick.RemoveAllListeners();
		buttonOk.onClick.AddListener (yesEvent);
		buttonOk.onClick.AddListener (ClosePanel);

		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener (yesEvent);
		buttonClose.onClick.AddListener (ClosePanel);
		buttonClose.gameObject.SetActive (true);

	}

	public void InitButtons(){
		
		if (currentSlide == 0) {
			buttonPrev.SetActive (false);
			buttonOk.gameObject.SetActive (false);
		} else if (currentSlide == inagesList.Count - 1) {
			buttonNext.SetActive (false);
			buttonPrev.SetActive (true);
			buttonOk.gameObject.SetActive (true);
		} else {
			buttonNext.SetActive (true);
			buttonPrev.SetActive (true);
			buttonOk.gameObject.SetActive (false);
		}
	}

	public void GoPrevious(){
			inagesList [currentSlide].gameObject.SetActive (false);
			currentSlide--;
			inagesList [currentSlide].gameObject.SetActive (true);

			InitButtons ();
	}

	public void GoNext(){
			inagesList [currentSlide].gameObject.SetActive (false);
			currentSlide++;
			inagesList [currentSlide].gameObject.SetActive (true);

			InitButtons ();
	}

	public void ClosePanel () {
		inagesList [currentSlide].gameObject.SetActive (false);
		instructionPanelObject.SetActive (false);
	}

}
