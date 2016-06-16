using UnityEngine;
using System.Collections;
using System;
using System.IO;
using SimpleJSON;


public class NetWorkUtils : MonoBehaviour {

	public static string buildRequestPulse(){

		//Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "GetPulse";

		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" + userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestToGetTOP100(){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "GetTopTen";
		//var method = "GetTopSQ";

		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" + userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestToGetTOPSQ(){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "GetTopSQ";

		var userToken	= "token=";

		postScoreURL = postScoreURL + method + "?" + userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}



	public static string buildRequestGetNumPlayers(){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "GetNumPlayers";

		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" + userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestGetLocalTopTen(float pLongitude, float pLatitude){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "GetLocalTopTen";

		var userToken 	= "token=";
		var latitude	= "latitude=";
		var longitude	= "longitude=";

		postScoreURL = postScoreURL + method + "?" + userToken + UserController.currentUser.Token
			+ "&"+ latitude + pLatitude.ToString() 
			+ "&"+ longitude + pLongitude.ToString();

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestToDeleteFriends(int pfriendId){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "DeleteFriend";

		var friendId 		= "friendId=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" + friendId + pfriendId + "&"+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestToAddFriends(int pfriendId){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "AddFriend";

		var friendId 		= "friendId=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" + friendId + pfriendId + "&"+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestToSearchFriends(string pSearchString){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "FindUser";

		var userName 		= "userName=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" + userName + pSearchString + "&"+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestToGetFriends(){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "GetFriends";

		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" + userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestToGetTrainingQuestion(int pFightType){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "GetTrainingQuestion";

		var userToken 		= "token=";
		var fightType			= "fightType=";


		postScoreURL = postScoreURL + method + "?" + userToken + UserController.currentUser.Token + "&" + fightType + pFightType.ToString();

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;

	}

	public static string buildRequestToSendBalance (int pBalance){

		Debug.Log ("build SetAward URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "SetMoney";

		var money			= "money=";
		var userToken 		= "token=";


		postScoreURL = postScoreURL + method + "?" + money + pBalance.ToString() + "&" + userToken + UserController.currentUser.Token ;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;

	}

	public static string buildRequestTSendSQ (int rightAnswersCount,int timeInGameSec ){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "EndTraining";

		var num				= "num=";
		var time			= "time=";
		var userToken 		= "token=";


		postScoreURL = postScoreURL + method + "?" + num + rightAnswersCount.ToString() + "&" + time + timeInGameSec.ToString() + "&" + userToken + UserController.currentUser.Token ;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;

	}

	public static string buildLogEXInURL(int regtype){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "LoginEx";

		var authName 		= "authName=";
		var authToken 		= "authToken=";
		var ver 			= "ver=";

		postScoreURL = postScoreURL + method + "?" 
			+ authName		+ Utility.getRegistrationType(regtype) + "&"
			+ authToken		+ UserController.AccessToken + "&"
			+ ver			+ Constants.GAmeVersion.ToString();

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildLogInURL(){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;
		
		var method = Utility.LOGIN_URL;
		
		var userName 		= "UserName=";
		var userPassword 	= "password=";
		var ver 			= "ver=";
		
		postScoreURL = postScoreURL + method + "?" 
			+ userName		+ UserController.UserName + "&"
			+ userPassword	+ UserController.UserPassword + "&"
			+ ver			+ Constants.GAmeVersion.ToString();
		
		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}
		
	public static string buildRequestToFightWithFriendURL(int pFriendId, int pFihtType){

		Debug.Log ("build letsFight URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "LetsFightWithFriend";

		var friendId 		= "friendId=";
		var fightTypeId 	= "fightTypeId=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" 
			+ friendId + pFriendId + "&"
			+ fightTypeId + pFihtType + "&"
			+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}


	public static string buildRequestToFightURL(int pFightId){

		Debug.Log ("build letsFight URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = Utility.LETSFIGHT_URL;

		var fightType 		= "fightTypeid=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?"
		+ fightType + pFightId.ToString () + "&"
		+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}


	public static string buildAcceptFightURL(string currentFight_Id){

		Debug.Log ("build letsFight URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "AcceptFightWithFriend";

		var fightId 		= "fightId=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" 
			+ fightId + currentFight_Id + "&"
			+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestStateFightURL(string currentFight_Id){

		Debug.Log ("build letsFight URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = Utility.STATE_FIGHT_URL;

		var fightId 		= "fightId=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" 
			+ fightId + currentFight_Id + "&"
			+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildGetTaskFightURL(Fight currentFight){

		Debug.Log ("Get Task from server.");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = Utility.TASK_URL;

		var taskId 			= "taskId=";
		var fightId 		= "fightId=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" 
			+ fightId 	+ currentFight.Id.ToString() + "&"
			+ taskId 	+ currentFight.TaskId.ToString() + "&"
			+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildUserInfoURL(int pUserId){

		Debug.Log ("Get Task from server.");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = Utility.GET_USER_INFO_URL;

		var userId 			= "userId=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" 
			+ userId + pUserId.ToString () + "&"
			+ userToken + UserController.currentUser.Token;

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildGetOpponentInfo(Fight currentFight){
	
		Debug.Log ("Get Task from server.");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = Utility.GET_USER_INFO_URL;

		var userId 			= "userId=";
		var userToken 		= "token=";

		if (currentFight.OpponentId != UserController.currentUser.Id) {
			postScoreURL = postScoreURL + method + "?" 
				+ userId + currentFight.OpponentId.ToString () + "&"
				+ userToken + UserController.currentUser.Token;
		} else {
			postScoreURL = postScoreURL + method + "?" 
				+ userId + currentFight.InitiatorId.ToString () + "&"
				+ userToken + UserController.currentUser.Token;
		}

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildGetRoundInfoURL(Fight currentFight, int answerList_Count){

		Debug.Log ("buildGetRoundInfoURL.");

		var postScoreURL = Utility.SERVICE_BASE_URL;
		var method = Utility.GET_ROUND_STATE_URL;

		var fightId		= "fightId=";
		var numId 		= "numId=";
		var userToken 	= "token=";

		postScoreURL = postScoreURL + method +
			"?" + fightId	+ currentFight.Id +
			"&" + numId + answerList_Count +
			"&" + userToken + UserController.currentUser.Token;

		return postScoreURL;
	}

	public static string buildCancelFightDialog(Fight currentFight){

		Debug.Log ("buildGetRoundInfoURL.");

		var postScoreURL 	= Utility.SERVICE_BASE_URL;
		var method 			= Utility.FIGHT_REJECT_URL;

		var fightId		= "fightId=";
		var userToken 	= "token=";

		postScoreURL = postScoreURL + method +
			"?" + fightId	+ currentFight.Id +
			"&" + userToken + UserController.currentUser.Token;

		return postScoreURL;
	}

	public static string buildCancelFightWithFriendDialog(Fight currentFight){

		Debug.Log ("buildGetRoundInfoURL.");

		var postScoreURL 	= Utility.SERVICE_BASE_URL;
		var method 			= "CancelFightWithFriend";

		var fightId		= "fightId=";
		var userToken 	= "token=";

		postScoreURL = postScoreURL + method +
			"?" + fightId	+ currentFight.Id +
			"&" + userToken + UserController.currentUser.Token;

		return postScoreURL;
	}

	public static string buildStatURL(Fight currentFight){

		Debug.Log ("build stat URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = Utility.STATE_FIGTS_URL;

		var userId 		= "userId=";
		var userToken 		= "token=";

		if (currentFight.OpponentId != UserController.currentUser.Id) {
			
			postScoreURL = postScoreURL + method + "?" 
				+ userId		+ currentFight.OpponentId.ToString () + "&"
				+ userToken	+ UserController.currentUser.Token;
			
		} else {
			
			postScoreURL = postScoreURL + method + "?" 
				+ userId		+ currentFight.InitiatorId.ToString () + "&"
				+ userToken	+ UserController.currentUser.Token;
			
		}
			
		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}
		
	public static string buildMyStatURL(float pLongitiude, float pLatitude){

		Debug.Log ("build stat URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = "";

		method = Utility.STATE_LOCATION_FIGTS_URL;


//		var userId 		= "userId=";
		var userToken 	= "token=";
		var latitude	= "latitude=";
		var longitude	= "longitude=";

		if (pLongitiude == -1) {
//			postScoreURL = postScoreURL + method + "?" 
//				+ userToken	 +  UserController.currentUser.Token + "&"
//				+ latitude	 +  "null" + "&"
//				+ longitude	 +  "null";
			postScoreURL = postScoreURL + method + "?" 
				+ userToken	 +  UserController.currentUser.Token;
		}else{
			postScoreURL = postScoreURL + method + "?" 
				+ userToken	 +  UserController.currentUser.Token + "&"
				+ latitude	 +  pLatitude.ToString() + "&"
				+ longitude	 +  pLongitiude.ToString();
		}

		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildGETItemsURL(){

		Debug.Log ("Get items from server.");
		
		var postScoreURL = Utility.SERVICE_BASE_URL;
		
		var method = Utility.GET_USER_ITEMS_URL;
		
		var userId 			= "userId=";
		var languageId 		= "languageId=";
		var userToken 		= "token=";
		
		postScoreURL = postScoreURL + method + "?" 
			+ userId 		+ UserController.currentUser.Id + "&"
				+ languageId 	+ UserController.currentUser.LanguageId + "&"
				+ userToken 	+ UserController.currentUser.Token;
		
		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildDownloadImageURL(int ItemId){

		Debug.Log ("Get items from server.");
		
		var postScoreURL = Utility.SERVICE_BASE_URL;
		
		var method = Utility.GET_IMAGE_USER_ITEMS_URL;
		
		var itemId 			= "itemId=";
		var userToken 		= "token=";
		
		postScoreURL = postScoreURL + method + "?" 
			+ itemId	+ ItemId + "&"
				+ userToken + UserController.currentUser.Token;
		
		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildDownloadCrownImageURL(int randId)
	{
	
		Debug.Log ("Get crown from server.");
		
		var postScoreURL = Utility.SERVICE_BASE_URL;
		
		var method = Utility.GET_GETRANKIMAGE_URL;
		
		var rankId 			= "rankId=";
		var userToken 		= "token=";
		
		postScoreURL = postScoreURL + method + "?" 
			+ rankId 	+ randId + "&"
				+ userToken + UserController.currentUser.Token;
		
		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;	
	}

	public static Exception saveItemImageFromWWW(string stringImage, int ItemId){

		try{
			Debug.Log("Parse items image: " );			
			
			var N = JSON.Parse(stringImage);
			var mLoginResult = N["GetItemImageResult"].AsArray;
			
			if(mLoginResult != null){
				
				byte[] buffer = new byte[mLoginResult.Count];
				for(int i = 0 ; i < mLoginResult.Count ; i++)
				{
					buffer[i] =  Convert.ToByte(mLoginResult[i].Value);
				}

				File.WriteAllBytes(Utility.getDcumentsPath() + "/Resources/" + ItemId.ToString() + "_ItemImage" + ".png", buffer);
				
				Debug.Log("Convert was complete.");
				
			}else{
				Debug.Log("Image is null.");
			}
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
		}

		return null;
	}
	
	public static Exception saveCrownImageFromWWW(string stringImage, int ItemId){
		
		try{
			Debug.Log("Parse crown image: " );			
			
			var N = JSON.Parse(stringImage);
			var mLoginResult = N["GetRankImageResult"].AsArray;
			
			if(mLoginResult != null){
				
				byte[] buffer = new byte[mLoginResult.Count];
				for(int i = 0 ; i < mLoginResult.Count ; i++)
				{
					buffer[i] =  Convert.ToByte(mLoginResult[i].Value);
				}
				
				File.WriteAllBytes(Utility.getDcumentsPath() + "/Resources/" + ItemId.ToString() + "_CrownImage.png", buffer);
				
				Debug.Log("Convert was complete.");
				
			}else{
				Debug.Log("Image is null.");
			}
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
		}
		
		return null;
	}

}
