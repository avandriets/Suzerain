using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class SearchDialog : MonoBehaviour {

	public GameObject 	searchPanelObject;
	public Button 		okButton;

	List<Friend> 				mFriends;
	List<Friend> 				mExistenFriends;
	List<FriendTemplateButton> 	friendsButtons;

	public	GameObject				friendButton;
	public	Transform				ListFriendsPanel;
	public	AddToFriends 			addToFriendsDialog;

	private Friend	currentFriend;

	private WaitPanel			waitPanel  = null;
	private ScreensManager		screensManager	= null;

	private ErrorPanel	errorPanel = null;

	public void ShowDialog(List<Friend> pFriends, List<Friend> pExistenFriends, UnityAction okEvent){

		screensManager	= ScreensManager.instance;

		ClearList ();

		mExistenFriends = pExistenFriends;
		mFriends = pFriends;

		InitSearchList ();


		searchPanelObject.SetActive (true);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);

		okButton.gameObject.SetActive (true);
	}

	public void AddToFriendYes(){
		waitPanel = screensManager.ShowWaitDialog("Добавление друга");

		StartCoroutine (AddFriendServer());
	}

	IEnumerator AddFriendServer(){

		var postScoreURL = NetWorkUtils.buildRequestToAddFriends (currentFriend.UserId);

		var request = new WWW (postScoreURL);

		while (!request.isDone) {
			yield return null;
		}

		if(waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		if (request.error == null) {

			Debug.Log ("Add friends done ! " + request.text);

			DeleteFriendFromList(currentFriend);
			mExistenFriends.Add (currentFriend);

			errorPanel = screensManager.ShowErrorDialog("Друг был успешно добавлен", OnErrorButtonClick);

		} else {

			Debug.Log ("WWW Error: " + request.error);
			Debug.Log ("WWW Error: " + request.text);
			errorPanel = screensManager.ShowErrorDialog("Ошбика добавления друга", OnErrorButtonClick);
		}

	}

	public void OnErrorButtonClick(){
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
	}

	public void AddToFriendNo(){
	
	}

	private void InitSearchList(){

		friendsButtons			= new List<FriendTemplateButton>();

		//Bottom array
		foreach (var c in mFriends) {

			GameObject	newButtonItem = null;
			newButtonItem = Instantiate(friendButton) as GameObject;
			FriendTemplateButton button1 = newButtonItem.GetComponent<FriendTemplateButton>();

			button1.friend = c;

			button1.NameUser.text = c.UserName;

			button1.rankUser.text = ScreensManager.LMan.getString(Utility.getRunkByNumber (c.Rank));

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

		//TODO show dialog to choose friend
		currentFriend = item.friend;

		addToFriendsDialog.ShowDialog (item.friend.UserName, AddToFriendYes, AddToFriendNo);
		Debug.Log (item.friend.UserName);
	}

	public void ClosePanel () {
		ClearList ();
		searchPanelObject.SetActive (false);
	}

	protected void ClearList(){

		foreach (Transform child in ListFriendsPanel) {
			GameObject.Destroy(child.gameObject);
		}
	}

	protected void DeleteFriendFromList(Friend pF){

		foreach (Transform child in ListFriendsPanel) {

			FriendTemplateButton button = child.GetComponent<FriendTemplateButton> ();

			if (button.friend.UserId == pF.UserId) {
				GameObject.Destroy (child.gameObject);
			}
		}
	}

}
