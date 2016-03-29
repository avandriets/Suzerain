﻿using UnityEngine;
using System.Collections;
using System;
using System.IO;
using SimpleJSON;

public class NetWorkUtils : MonoBehaviour {

	public static string buildLogInURL(){

		Debug.Log ("build LogIn URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;
		
		var method = Utility.LOGIN_URL;
		
		var userName 		= "UserName=";
		var userPassword 	= "password=";
		
		postScoreURL = postScoreURL + method + "?" 
			+ userName		+ UserController.UserName + "&"
				+ userPassword	+ UserController.UserPassword;
		
		postScoreURL = System.Uri.EscapeUriString (postScoreURL);

		return postScoreURL;
	}

	public static string buildRequestToFightURL(){

		Debug.Log ("build letsFight URL");

		var postScoreURL = Utility.SERVICE_BASE_URL;

		var method = Utility.LETSFIGHT_URL;

		var fightType 		= "fightType=";
		var userToken 		= "token=";

		postScoreURL = postScoreURL + method + "?" 
			+ fightType + "1" + "&"
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