using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System.IO;

public class ScreensManager : MonoBehaviour {

	public GameObject mMainScreenCanvas;
	public GameObject mRegistrationScreenCanvas;
	public GameObject mProfileScreenCanvas;
	public GameObject mGameScreenCanvas;
	public GameObject mAchivmentScreenCanvas;
	public GameObject mFriendsScreenCanvas;
	public GameObject mTrainingScreenCanvas;

	[HideInInspector]
	public GameObject mStartScreenCanvas = null;

	public GameObject	currentScreenCanvas = null;

	public WaitPanel			waitPanel;
	public ErrorPanel			errorPanel;
	public ErrorPanelTwoButtons errorPanelTwoButtons;
	public WaitOpponentDialog	waitOpponentDialog;
	public RoundResultDialog	roundResultDialog;
	public InstructionDialog	instDialog;
	public AcceptFightWithFriend	fightWithFriendDialog;

	//Subscription
	public BuySubscriptionDialog 	buySuscriptionDlg;
	public BuyTalantsDialog 		buyTalantsDlg;

	public GameTypeOneTwo	game1;
	public GameType2		game2;
	public GameType3		game3;
	public GameType4		game4;
	public GameType5_1		game5;
	public GameType6_1		game6;
	public GameType7		game7;

	public static Lang 	LMan;

	private static ScreensManager s_Instance = null;

	public Dictionary<GameObject,Dictionary<Text,string>> mTextDict = new Dictionary<GameObject,Dictionary<Text,string>>();

	public BuyTalantsDialog GetBuyTalantsDlg(){

		BuyTalantsDialog NewWaitPanel = GameObject.Instantiate(buyTalantsDlg) as BuyTalantsDialog;

		NewWaitPanel.transform.SetParent(currentScreenCanvas.transform);
		NewWaitPanel.transform.localScale = new Vector3(1,1,1);

		RectTransform rctr = NewWaitPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);

		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewWaitPanel;
	}

	public BuySubscriptionDialog GetBuySubscriptionDlg(){

		BuySubscriptionDialog NewWaitPanel = GameObject.Instantiate(buySuscriptionDlg) as BuySubscriptionDialog;

		NewWaitPanel.transform.SetParent(currentScreenCanvas.transform);
		NewWaitPanel.transform.localScale = new Vector3(1,1,1);

		RectTransform rctr = NewWaitPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);

		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewWaitPanel;
	}

	public GameBase GetGameByNumber(Transform pGameContainer, int pGameNumner){

		GameBase NewWaitPanel = null;

		if (pGameNumner == Utility.Znaniya) {
			NewWaitPanel = GameObject.Instantiate (game1) as GameTypeOneTwo;
		}else if(pGameNumner == Utility.Logic){
			NewWaitPanel = GameObject.Instantiate (game2) as GameType2;
		}else if(pGameNumner == Utility.Mudrost){
			NewWaitPanel = GameObject.Instantiate (game3) as GameType3;
		}else if(pGameNumner == Utility.Reflex){
			NewWaitPanel = GameObject.Instantiate (game4) as GameType4;
		}else if(pGameNumner == Utility.Razum){
			NewWaitPanel = GameObject.Instantiate (game5) as GameType5_1;
		}else if(pGameNumner == Utility.Iridizia){
			NewWaitPanel = GameObject.Instantiate (game6) as GameType6_1;
		}else if(pGameNumner == Utility.Intuition){
			NewWaitPanel = GameObject.Instantiate (game7) as GameType7;
		}

		NewWaitPanel.transform.SetParent(pGameContainer);
		NewWaitPanel.transform.localScale = new Vector3(1,1,1);

		RectTransform rctr = NewWaitPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);

		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewWaitPanel;
	}


	public void InitTranslateList(){

		if (!mTextDict.ContainsKey (currentScreenCanvas.gameObject)) {

			var textComponents = currentScreenCanvas.gameObject.GetComponentsInChildren<Text>(true);

			Dictionary<Text,string> tt = new Dictionary<Text,string> ();

			foreach (Text c in textComponents) {
				if (c.text.Contains ("@")) {
					tt.Add (c,c.text);
					//c.text = ScreensManager.LMan.getString (c.text);
					Debug.Log (c.text);
				}
			}

			if (tt.Count > 0) {
				mTextDict.Add (currentScreenCanvas.gameObject, tt);
			}
		}
	}

	public void TranslateUI(){
		
		if (mTextDict.ContainsKey (currentScreenCanvas.gameObject)) {
			foreach (var c in mTextDict) {
				if (c.Key == currentScreenCanvas.gameObject) {
					foreach (var cc in c.Value) {
						//Text tcomp = c.Key.GetComponent<Text> ();
						var str = ScreensManager.LMan.getString (cc.Value);
						if (str != "")
							cc.Key.text = str;
					}
				}
			}
		}
	}

	public void InitLanguage(){
		if (ScreensManager.LMan == null) {

			//!!!!!!!!!!!!!!!!!!
			//РАСКОМЕТИЬРОВАТЬ КОГДА ПОНАДОБИТСЯ МУЛЬТИЯЗЫЧНАЯ ВЕРСИЯ
			ScreensManager.LMan = new Lang ("lang", "Russian", false, true);

//			if (UserController.currentUser != null) {
//				//ScreensManager.LMan = new Lang (Path.Combine (Application.dataPath, "lang.xml"), UserController.currentUser.Language, false);	
//				ScreensManager.LMan = new Lang ("lang", UserController.currentUser.Language, false, true);
//			} else {
//				if (Application.systemLanguage == SystemLanguage.Russian ||
//				   Application.systemLanguage == SystemLanguage.Ukrainian ||
//				   Application.systemLanguage == SystemLanguage.Belarusian) {
//					//ScreensManager.LMan = new Lang (Path.Combine (Application.dataPath, "lang.xml"), "Russian", false);	
//					ScreensManager.LMan = new Lang ("lang", "Russian", false, true);
//				} else {
//					//ScreensManager.LMan = new Lang (Path.Combine (Application.dataPath, "lang.xml"), "English", false);
//					ScreensManager.LMan = new Lang ("lang", "English", false, true);
//				}
//			}
		} else {
			
			//!!!!!!!!!!!!!!!!!!
			//РАСКОМЕТИЬРОВАТЬ КОГДА ПОНАДОБИТСЯ МУЛЬТИЯЗЫЧНАЯ ВЕРСИЯ
			ScreensManager.LMan.setLanguageFromRes ("lang", "Russian");

//			if (UserController.currentUser != null && UserController.currentUser.Language != Lang.currentLanguage) {
//				//ScreensManager.LMan.setLanguage (Path.Combine (Application.dataPath, "lang.xml"), UserController.currentUser.Language);
//				ScreensManager.LMan.setLanguageFromRes ("lang", UserController.currentUser.Language);
//			}
		}
	}

	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	public static ScreensManager instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance =  FindObjectOfType(typeof (ScreensManager)) as ScreensManager;
			}
			
			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("GameManager");
				s_Instance = obj.AddComponent(typeof (ScreensManager)) as ScreensManager;
				Debug.Log ("Could not locate an AManager object. \n ScreensManager was Generated Automaticly.");
			}

			return s_Instance;
		}
	}
	
	// Ensure that the instance is destroyed when the game is stopped in the editor.
	void OnApplicationQuit() {
		s_Instance = null;
	}

	public void ShowMainScreen()
	{
		if (currentScreenCanvas != null) {
			currentScreenCanvas.SetActive(false);
		}
			
		currentScreenCanvas = mMainScreenCanvas;

		if (!mMainScreenCanvas.activeSelf) {
			mMainScreenCanvas.SetActive (true);
		}
	}

	public void ShowFriendsScreen()
	{
		if (currentScreenCanvas != null) {
			currentScreenCanvas.SetActive(false);
		}

		currentScreenCanvas = mFriendsScreenCanvas;

		if (!mFriendsScreenCanvas.activeSelf) {
			mFriendsScreenCanvas.SetActive (true);
		}
	}

	public void ShowTrainingScreen()
	{
		if (currentScreenCanvas != null) {
			currentScreenCanvas.SetActive(false);
		}

		currentScreenCanvas = mTrainingScreenCanvas;

		if (!mTrainingScreenCanvas.activeSelf) {
			mTrainingScreenCanvas.SetActive (true);
		}
	}

	public void ShowRegistrationScreen()
	{
		if (currentScreenCanvas != null) {
			currentScreenCanvas.SetActive(false);
		}
	
		currentScreenCanvas = mRegistrationScreenCanvas;

		if (!mRegistrationScreenCanvas.activeSelf) {
			mRegistrationScreenCanvas.SetActive (true);
		}
	}

	public void ShowProfileScreen()
	{
		if (currentScreenCanvas != null) {
			currentScreenCanvas.SetActive(false);
		}
			
		currentScreenCanvas = mProfileScreenCanvas;

		if (!mProfileScreenCanvas.activeSelf) {
			mProfileScreenCanvas.SetActive (true);
		}
	}

	public void ShowGameScreen()
	{
		if (currentScreenCanvas != null) {
			currentScreenCanvas.SetActive(false);
		}
			
		currentScreenCanvas = mGameScreenCanvas;

		if (!mGameScreenCanvas.activeSelf) {
			mGameScreenCanvas.SetActive (true);
		}
	}

	public void ShowAchivmentsScreen()
	{
		if (currentScreenCanvas != null) {
			currentScreenCanvas.SetActive(false);
		}
			
		currentScreenCanvas = mAchivmentScreenCanvas;

		if (!mAchivmentScreenCanvas.activeSelf) {
			mAchivmentScreenCanvas.SetActive (true);
		}
	}
		
	public RoundResultDialog ShowResultDialog(List<Fight> message, List<TaskAnswer> answers, List<TestTask> 		tasksList){

		RoundResultDialog NewWaitPanel = GameObject.Instantiate(roundResultDialog) as RoundResultDialog;

		NewWaitPanel.transform.SetParent(currentScreenCanvas.transform);
		NewWaitPanel.transform.localScale = new Vector3(1,1,1);
		NewWaitPanel.SetText(message,  answers, tasksList);

		RectTransform rctr = NewWaitPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);

		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewWaitPanel;
	}

	public void CloseResultDialogPanel(RoundResultDialog NewWaitPanel){
		NewWaitPanel.ClosePanel();
		GameObject.Destroy(NewWaitPanel.gameObject);
		NewWaitPanel = null;
	}


	public WaitPanel ShowWaitDialog(string message, bool pShowCitates){
		
		WaitPanel NewWaitPanel = GameObject.Instantiate(waitPanel) as WaitPanel;
		
		NewWaitPanel.transform.SetParent(currentScreenCanvas.transform);
		NewWaitPanel.transform.localScale = new Vector3(1,1,1);
		NewWaitPanel.SetText (message, pShowCitates);
		
		RectTransform rctr = NewWaitPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);
		
		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewWaitPanel;
	}
	
	public void CloseWaitPanel(WaitPanel NewWaitPanel){
		if (NewWaitPanel != null) {
			NewWaitPanel.ClosePanel ();
			GameObject.Destroy (NewWaitPanel.gameObject);
			NewWaitPanel = null;
		}
	}

	public ErrorPanel ShowErrorDialog(string message, UnityAction clickButtonEvent){
		
		ErrorPanel NewErrorPanel = Instantiate(errorPanel) as ErrorPanel;
		
		NewErrorPanel.SetText(message, clickButtonEvent);
		
		NewErrorPanel.transform.SetParent(currentScreenCanvas.transform);
		NewErrorPanel.transform.localScale = new Vector3(1,1,1);
		
		RectTransform rctr = NewErrorPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);
		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewErrorPanel;
	}

	public ErrorPanelTwoButtons ShowErrorTwoButtonsDialog(string message, UnityAction clickOkButtonEvent, UnityAction clickNoButtonEvent){
		
		ErrorPanelTwoButtons NewErrorPanel = Instantiate(errorPanelTwoButtons) as ErrorPanelTwoButtons;

		NewErrorPanel.SetText(message, clickOkButtonEvent, clickNoButtonEvent);

		NewErrorPanel.transform.SetParent(currentScreenCanvas.transform);
		NewErrorPanel.transform.localScale = new Vector3(1,1,1);

		RectTransform rctr = NewErrorPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);
		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewErrorPanel;
	}

	public WaitOpponentDialog ShowWaitOpponentDialog(string message, UnityAction cancelButtonEvent){

		WaitOpponentDialog NewPanel = Instantiate(waitOpponentDialog) as WaitOpponentDialog;

		NewPanel.SetText(message, cancelButtonEvent);

		NewPanel.transform.SetParent(currentScreenCanvas.transform);
		NewPanel.transform.localScale = new Vector3(1,1,1);

		RectTransform rctr = NewPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);
		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewPanel;
	}

	public InstructionDialog ShowInstructionDialog(UnityAction closeButtonEvent){

		InstructionDialog NewWaitPanel = GameObject.Instantiate(instDialog) as InstructionDialog;

		NewWaitPanel.transform.SetParent(currentScreenCanvas.transform);
		NewWaitPanel.transform.localScale = new Vector3(1,1,1);
		NewWaitPanel.ShowDialog (closeButtonEvent);

		RectTransform rctr = NewWaitPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);

		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewWaitPanel;
	}

	public void CloseInstructionPanel(InstructionDialog NewWaitPanel){
		if (NewWaitPanel != null) {
			NewWaitPanel.ClosePanel ();
			GameObject.Destroy (NewWaitPanel.gameObject);
			NewWaitPanel = null;
		}
	}

	public AcceptFightWithFriend CreateFightWithFriendDialog(){

		AcceptFightWithFriend NewPanel = Instantiate(fightWithFriendDialog) as AcceptFightWithFriend;

		//NewPanel.SetText(message, cancelButtonEvent);

		NewPanel.transform.SetParent(currentScreenCanvas.transform);
		NewPanel.transform.localScale = new Vector3(1,1,1);

		RectTransform rctr = NewPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);
		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewPanel;
	}
}
