using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public delegate void FightWithFriend (int pFightNum);

public class FightOrDeleteDialog : MonoBehaviour {

	public GameObject 	AddPanelObject;
	public Text			UserName;
	public Button 		noButton;

	public Button 		fightRandomButton;
	public Button 		fight1Button;
	public Button 		fight2Button;
	public Button 		fight3Button;
	public Button 		fight4Button;
	public Button 		fight5Button;
	public Button 		fight6Button;
	public Button 		fight7Button;

	private FightWithFriend mFightDelegate;

	public void ShowDialog(string pUserName, UnityAction noEvent, FightWithFriend pFightDelegate){

		mFightDelegate = pFightDelegate;

		AddPanelObject.SetActive (true);
		UserName.text = pUserName;

		//Yes button
//		yesButton.onClick.RemoveAllListeners();
//		yesButton.onClick.AddListener (yesEvent);
//		yesButton.onClick.AddListener (ClosePanel);
//		yesButton.gameObject.SetActive (true);

		//No button
		noButton.onClick.RemoveAllListeners();
		noButton.onClick.AddListener (noEvent);
		noButton.onClick.AddListener (ClosePanel);
		noButton.gameObject.SetActive (true);

		//Random fight button
		fightRandomButton.onClick.RemoveAllListeners();
		fightRandomButton.onClick.AddListener (ClosePanel);
		fightRandomButton.onClick.AddListener ( () => FightButtonClick(0) );
		fightRandomButton.gameObject.SetActive (true);

		//1 fight button
		fight1Button.onClick.RemoveAllListeners();
		fight1Button.onClick.AddListener (ClosePanel);
		fight1Button.onClick.AddListener ( () => FightButtonClick(1) );
		fight1Button.gameObject.SetActive (true);

		//2 fight button
		fight2Button.onClick.RemoveAllListeners();
		fight2Button.onClick.AddListener (ClosePanel);
		fight2Button.onClick.AddListener ( () => FightButtonClick(2) );
		fight2Button.gameObject.SetActive (true);

		//3 fight button
		fight3Button.onClick.RemoveAllListeners();
		fight3Button.onClick.AddListener (ClosePanel);
		fight3Button.onClick.AddListener ( () => FightButtonClick(3) );
		fight3Button.gameObject.SetActive (true);

		//4 fight button
		fight4Button.onClick.RemoveAllListeners();
		fight4Button.onClick.AddListener (ClosePanel);
		fight4Button.onClick.AddListener ( () => FightButtonClick(4) );
		fight4Button.gameObject.SetActive (true);

		//5 fight button
		fight5Button.onClick.RemoveAllListeners();
		fight5Button.onClick.AddListener (ClosePanel);
		fight5Button.onClick.AddListener ( () => FightButtonClick(5) );
		fight5Button.gameObject.SetActive (true);

		//6 fight button
		fight6Button.onClick.RemoveAllListeners();
		fight6Button.onClick.AddListener (ClosePanel);
		fight6Button.onClick.AddListener ( () => FightButtonClick(6) );
		fight6Button.gameObject.SetActive (true);

		//7 fight button
		fight7Button.onClick.RemoveAllListeners();
		fight7Button.onClick.AddListener (ClosePanel);
		fight7Button.onClick.AddListener ( () => FightButtonClick(7) );
		fight7Button.gameObject.SetActive (true);

	}

	public void FightButtonClick(int pFightNum){
		mFightDelegate (pFightNum);
	}

	public void ClosePanel () {
		AddPanelObject.SetActive (false);
	}
}
