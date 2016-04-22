using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


public class FriendsManager : MonoBehaviour {

	private WaitPanel	waitPanel  = null;
	private ErrorPanel	errorPanel = null;

	ScreensManager	screensManager	= null;

	public	GameObject				friendButton;
	public	Transform				ListFriendsPanel;

	List<Friend> friendsList = null;
	List<FriendTemplateButton> friendsButtons;

	public SearchDialog			searchDialog;
	public FightOrDeleteDialog	fightOrDeleteDialog;

	public Text	searchField;

	private Friend currentFriend;
	private WaitOpponentDialog		waitForOpponentPanel	= null;

	public LiveMessenger	messengerLive;

	public void onSearchClick(){

		waitPanel = screensManager.ShowWaitDialog("Получение данных");

		if (searchField.text.Length > 0) {
			StartCoroutine (SerchFriendsOnServer (searchField.text));

		} else {
			screensManager.CloseWaitPanel (waitPanel);
		}

	}

	IEnumerator SerchFriendsOnServer(string pFriendName){

		var postScoreURL = NetWorkUtils.buildRequestToSearchFriends (pFriendName);

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "text/json");

		WWWForm form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (postScoreURL);

		while (!request.isDone) {
			yield return null;
		}

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		if (request.error == null) {

			Debug.Log ("Get friends done ! " + request.text);

			List<Friend> foundUsers = Utility.ParseFriendsJsonList (request.text, "FindUserResult");

			if (foundUsers != null && foundUsers.Count > 0) {
				searchDialog.ShowDialog (foundUsers, friendsList, closeSearchDialog);
			} else {
				errorPanel = screensManager.ShowErrorDialog("Пользователей с таким именем не найдено.", OnErrorButtonClick);
			}

		} else {

			Debug.Log ("WWW Error: " + request.error);
			Debug.Log ("WWW Error: " + request.text);
			errorPanel = screensManager.ShowErrorDialog("Ошбика соединения с сервером", OnErrorButtonClick);
		}

	}

	public void closeSearchDialog(){

		PrepareList ();
		InflateList ();
	}

	// Use this for initialization
	void OnEnable() {
	
		screensManager	= ScreensManager.instance;

		waitPanel = screensManager.ShowWaitDialog("Получение данных");

		PrepareList ();

		StartCoroutine (GetFriendsFromServer());

		//messengerLive.pulseWasStarted = false;
	}

	IEnumerator GetFriendsFromServer(){
	
		var postScoreURL = NetWorkUtils.buildRequestToGetFriends ();

		var dictHeader = new Dictionary<string, string> ();
		dictHeader.Add ("Content-Type", "text/json");

		WWWForm form = new WWWForm ();
		form.AddField ("Content-Type", "text/json");

		var request = new WWW (postScoreURL);

		while (!request.isDone) {
			yield return null;
		}

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);
		
		if (request.error == null) {

			Debug.Log ("Get friends done ! " + request.text);

			friendsList = Utility.GetListOfFriends (request.text);
			InflateList ();

		} else {
			
			Debug.Log ("WWW Error: " + request.error);
			Debug.Log ("WWW Error: " + request.text);
			errorPanel = screensManager.ShowErrorDialog("Ошбика соединения с сервером", OnErrorButtonClick);
		}
	}

	public void OnErrorButtonClick(){
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
	}
		 
	protected void PrepareList(){

		foreach (Transform child in ListFriendsPanel) {
			GameObject.Destroy(child.gameObject);
		}
	}

	protected void DeleteFriendFromList(Friend pF){

		foreach (Transform child in ListFriendsPanel) {

			FriendTemplateButton button = child.GetComponent<FriendTemplateButton> ();

			if (button.friend.UserId == pF.UserId) {
				friendsList.Remove (pF);
				GameObject.Destroy (child.gameObject);
			}
		}
	}

	private void InflateList(){

		friendsButtons			= new List<FriendTemplateButton>();

		//Bottom array
		foreach (var c in friendsList) {

			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(friendButton) as GameObject;
			FriendTemplateButton button1 = newButtonItem.GetComponent<FriendTemplateButton>();

			button1.friend = c;

			button1.NameUser.text = c.UserName;

			if (c.Rank == -1) {
				button1.rankUser.text = "";
			} else {
				button1.rankUser.text = ScreensManager.LMan.getString(Utility.getRunkByNumber (c.Rank));
			}

			if (c.Rank != -1) {
				Utility.setAvatarByState (button1.shieldImage, c.Result);
			}
			else
				Utility.setAvatarByState (button1.shieldImage, -1);

			button1.button.onClick.RemoveAllListeners();
			button1.button.onClick.AddListener( () => onCardFromSetClick(button1) );

			newButtonItem.transform.SetParent(ListFriendsPanel);
			newButtonItem.transform.localScale = new Vector3(1,1,1);

			friendsButtons.Add (button1);
		}
	}

	public void onCardFromSetClick(FriendTemplateButton item){

		//errorPanel = screensManager.ShowErrorDialog(item.friend.UserName, OnErrorButtonClick);

		currentFriend = item.friend;

		fightOrDeleteDialog.ShowDialog(item.friend.UserName, DeleteUser, CancelDeleteUser, FightButton);

		Debug.Log (item.friend.UserName);
	}

	public void DeleteUser(){
	
		waitPanel = screensManager.ShowWaitDialog("Добавление друга");

		StartCoroutine (DeleteFriendServer());
	}

	public void CancelDeleteUser(){
	
	}

	public void FightButton(int pFightNum){
	
		waitForOpponentPanel = screensManager.ShowWaitOpponentDialog("Вызываю на дуэль",CancelFightByUser);

		OnlineGame ing = OnlineGame.instance;
		ing.AskForFightWithFriend (CancelFightByServer, ReadyToFight, ErrorFightRequest, currentFriend.UserId, pFightNum);

	}

	public void CancelFightByUser(){

		OnlineGame ing = OnlineGame.instance;
		ing.CancelFightWithFriend ();
		SoundManager.ChoosePlayMusic (0);
	}

	public void CancelFightByServer(Fight pFight){

		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		if (pFight.Id == 0) {
			errorPanel = screensManager.ShowErrorDialog ("Отказано в поединке", ErrorCancelByServer);
		}else if(pFight.Id == -1){
			errorPanel = screensManager.ShowErrorDialog ("Офлайн", ErrorCancelByServer);
		}else if(pFight.Id == -2){
			errorPanel = screensManager.ShowErrorDialog ("Вызван на поединок", ErrorCancelByServer);
		}
		SoundManager.ChoosePlayMusic (0);
	}

	public void ErrorCancelByServer(){
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
		SoundManager.ChoosePlayMusic (0);
	}

	public void ReadyToFight(){

		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		screensManager.ShowGameScreen ();
	}

	public void ErrorFightRequest(){
		
		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		errorPanel = screensManager.ShowErrorDialog(ScreensManager.LMan.getString ("@server_side_error"), ErrorCancelByServer);
		SoundManager.ChoosePlayMusic (0);
	}

	IEnumerator DeleteFriendServer(){

		var postScoreURL = NetWorkUtils.buildRequestToDeleteFriends (currentFriend.UserId);
		var request = new WWW (postScoreURL);

		while (!request.isDone) {
			yield return null;
		}

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		if (request.error == null) {

			Debug.Log ("Add friends done ! " + request.text);

			DeleteFriendFromList (currentFriend);
			errorPanel = screensManager.ShowErrorDialog("Друг был успешно удален", OnErrorButtonClick);

		} else {

			Debug.Log ("WWW Error: " + request.error);
			Debug.Log ("WWW Error: " + request.text);
			errorPanel = screensManager.ShowErrorDialog("Ошбика удаления друга", OnErrorButtonClick);
		}
	}
}
