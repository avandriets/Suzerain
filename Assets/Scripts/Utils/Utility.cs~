using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System;
using System.Globalization;


// Declare a delegate type for processing a book:
public delegate void ProcessLogInDelegate(bool error, string source_error, string error_text);
public delegate void ProcessDownloadCrownDelegate(bool error, string source_error, string error_text);
public delegate void ProcessBuyItemDelegate();
public delegate void ProcessSelectItemDelegate();
public delegate void ParseFightsDelegate(string fightsList);
public delegate void TimerStopDelegate();

public delegate void CancelFightByServerDelegate();
public delegate void CancelFightDelegate(Fight pFight);
public delegate void CancelFughtByUserDelegate();
public delegate void ReadyToFight();
public delegate void ErrorToFight();

public delegate void SucsessRegistrationDelegate(int typeRed, Dictionary<string,object> logInDetails);
public delegate void FailRegistrationDelegate(int typeRed);

public delegate void AskForFightDelegate(int fightId);

public static class Utility {

	//Statuses
	public static int STATUS_GLOBAL = 0;
	public static int STATUS_LOCAL = 1;
	public static int STATUS_SQ = 2;

	//Game types
	public static int Znaniya	= 1;
	public static int Logic		= 2;
	public static int Mudrost 	= 3;
	public static int Reflex	= 4;
	public static int Razum		= 5;
	public static int Iridizia	= 6;
	public static int Intuition = 7;

	public static int[] gameDurationArray = new int[] {20, 60, 60, 20, 30, 20, 20};

	public static int[] trainingFightsArray = new int[] {Znaniya, Logic, Mudrost, Razum, Iridizia};

	public static bool	TESTING_MODE	= false;
	public static bool	ShowRightAnswer = true;

	//public static string SERVICE_BASE_URL	= "http://suzerain.westeurope.cloudapp.azure.com:8062/SuzerainWcfService/SuzerainService/";
	public static string SERVICE_BASE_URL	= "http://suzerain.westeurope.cloudapp.azure.com:9064/SuzerainWcfService/SuzerainService/";
	public static string LOGIN_URL			= "Login";
	public static string LETSFIGHT_URL		= "LetsFight";
	public static string STATE_FIGTS_URL	= "GetUserStats";
	public static string STATE_LOCATION_FIGTS_URL	= "GetLocStats";
	public static string REGISTER_USER_URL	= "RegisterUserEx";
	public static string STATE_FIGHT_URL	= "GetFightState";
	public static string TASK_URL			= "GetTask";
	public static string SET_ANSWER_URL		= "SetAnswer";
	public static string EDIT_USER_URL		= "EditUser";
	public static string GET_USER_INFO_URL	= "GetUserInfo";
	public static string GET_USER_ITEMS_URL	= "GetUserItems";
	public static string GET_IMAGE_USER_ITEMS_URL	= "GetItemImage";
	public static string BUY_ITEM_URL				= "BuyItem";
	public static string SET_ACTIVE_ITEM_URL		= "SetActiveItem";
	public static string GET_LAST_FIGTS_URL			= "GetLastFights";

	public static string GET_LOCAL_STATUS_URL	= "GetLocalStatus";
	public static string GET_GLOBAL_STATUS_URL	= "GetGlobalStatus";
	public static string GET_GETRANKIMAGE_URL	= "GetRankImage";
	public static string SET_ANSWERS_URL		= "SetTestAnswers";
	public static string SET_RANGE_ANSWER_URL	= "SetTestAnswer";
	public static string GET_ROUND_STATE_URL	= "GetRoundState";
	public static string FIGHT_REJECT_URL	= "RejectFight";

	public static shieldDataObj[] shieldsArray = new shieldDataObj[] {
		new shieldDataObj(1, 0   ,29  ,"1", "ЖЕЛЕЗНЫЙ ЩИТ", 10), 
		new shieldDataObj(2, 30 ,99 ,"2", "СТАЛЬНОЙ ЩИТ", 15), 
		new shieldDataObj(3, 100 ,199 ,"3", "БРОНЗОВЫЙ ЩИТ", 20),
		new shieldDataObj(4, 200 ,499,"4", "СЕРЕБРЯНЫЙ ЩИТ", 25),
		new shieldDataObj(5, 500,999,"5", "ЗОЛОТОЙ ЩИТ", 30),
		new shieldDataObj(6, 1000,2999,"6", "ПЛАТИНОВЫЙ ЩИТ", 50),
		new shieldDataObj(7, 3000,10000,"7", "КРИСТАЛЬНЫЙ ЩИТ", 100)
	};
				
	public static string getRegistrationType(int regType){

		string authType;

		if (regType == Constants.GOOGLE_PLAY) {
			authType = "GP";
		} else if (regType == Constants.FACEBOOK) {
			authType = "FB";
		} else {
			authType = "LOC";
		}

		return authType;
	}

	public static List<Friend> ParseFriendsJsonList(string pJsonStr, string nameOfCollection){

		var N = JSON.Parse(pJsonStr);
		var mGetResult = N[nameOfCollection].AsArray;
		List<Friend> newObj = new List<Friend> ();

		foreach (var item in mGetResult) {
			Friend newItem = ParseGetFriendResponse(item.ToString());
			newObj.Add(newItem);
		}

		return newObj;
	}

	public static List<Friend> GetListOfFriends(string pJsonStr){

		return ParseFriendsJsonList(pJsonStr,"GetFriendsResult" );
	}

	public static Friend ParseGetFriendResponse(string pJsonStr){

		var mGetResult = JSON.Parse(pJsonStr);

		Friend newObj = new Friend ();

		newObj.UserId		= mGetResult ["UserId"].AsInt;
		newObj.UserName		= mGetResult ["UserName"];
		newObj.Result		= mGetResult ["Result"].AsDouble;
		newObj.State 		= mGetResult ["State"].AsInt;
		newObj.Rank 		= mGetResult ["Rank"].AsInt;
		newObj.Score 		= mGetResult ["Score"].AsInt;
		newObj.SQ 		= mGetResult ["SQ"].AsDouble;

		return newObj;
	}

	public static List<FightStat> GetListOfFightsStates(string pJsonStr){

		var N = JSON.Parse(pJsonStr);
		var mGetResult = N["GetLocStatsResult"].AsArray;
		List<FightStat> newObj = new List<FightStat> ();

		foreach (var item in mGetResult) {
			FightStat newItem = ParseGetFigtResponse(item.ToString());
			newObj.Add(newItem);
		}

		return newObj;
	}

	public static List<FightStat> GetListOfFightsStatesUSER(string pJsonStr){

		var N = JSON.Parse(pJsonStr);
		var mGetResult = N["GetUserStatsResult"].AsArray;
		List<FightStat> newObj = new List<FightStat> ();

		foreach (var item in mGetResult) {
			FightStat newItem = ParseGetFigtResponse(item.ToString());
			newObj.Add(newItem);
		}

		return newObj;
	}

	public static FightStat ParseGetFigtResponse(string pJsonStr){

		var mGetResult = JSON.Parse(pJsonStr);

		FightStat newObj = new FightStat ();

		newObj.UserId		= mGetResult ["UserId"].AsInt;
		newObj.FightTypeId	= mGetResult ["FightTypeId"].AsInt;
		newObj.Fights		= mGetResult ["Fights"].AsInt;
		newObj.Wins 		= mGetResult ["Wins"].AsInt;
		newObj.Draws 		= mGetResult ["Draws"].AsInt;
		newObj.RowWins 		= mGetResult ["RowWins"].AsInt;
		newObj.Result 		= mGetResult ["Result"].AsDouble; 
		newObj.LocalStatus 	= mGetResult ["LocalStatus"].AsInt;
		newObj.GlobalStatus = mGetResult ["GlobalStatus"].AsInt;
		newObj.Score = mGetResult ["Score"].AsInt;
		newObj.SQ = mGetResult ["SQ"].AsDouble;
	
		return newObj;
	}

	public static Sprite getResourceImage(string filePath){
	
		if (File.Exists (filePath)) {
			byte[] image = File.ReadAllBytes (filePath);
			Texture2D tex = new Texture2D (2, 2);
			tex.LoadImage (image);

			if (tex != null) {
				Sprite spr = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
				return spr;
			}

		} 

		return null;
	}

	public static string getDcumentsPath(){

		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{ 
			Debug.Log("IT IS IPHONE !!!");

			if( !Directory.Exists(Application.persistentDataPath + "/Resources/")){
				var t = new DirectoryInfo(Application.persistentDataPath);
				t.CreateSubdirectory("Resources");
			}

			return Application.persistentDataPath;

		}else if(Application.platform == RuntimePlatform.Android){

			Debug.Log("IT IS ANDROID !!!");

			if( !Directory.Exists(Application.persistentDataPath + "/Resources/")){
				var t = new DirectoryInfo(Application.persistentDataPath);
				t.CreateSubdirectory("Resources");
			}
			
			return Application.persistentDataPath;
		}else{

			Debug.Log("OTHER ENIMAL !!!");

			return Application.dataPath;
		}

	}

	public static string getResourceFolder(){

		return Utility.getDcumentsPath () + "/Resources/";
	}

	public static User ParseGetUserResponse(string pJsonStr){
		
		var N = JSON.Parse(pJsonStr);
		//var mGetResult = N["GetLoginResult"];
		
		User newObj = new User ();
		
		newObj.Id 			= N ["Id"].AsInt;
		newObj.UserName  	= N ["UserName"].Value;
		newObj.EMail  		= N ["EMail"].Value;
		newObj.Sex 	 		= N ["Sex"].AsInt;
		newObj.IsOnline  	= N ["IsOnline"].AsInt;
		//newObj.BirthDate = null; //N ["BirthDate"].Value;
		newObj.Motto 	 	= N ["Motto"].Value;
		//newObj.Photo 	 	= N ["Photo"].AsArray;
		newObj.State 	 	= N ["State"].AsInt;
		//newObj.RegDate = null;//N ["RegDate"].Value;
		newObj.LanguageId  	= N ["LanguageId"].AsInt;
		newObj.CountryId  	= N ["CountryId"].AsInt;
		//newObj.LastSync  	= N ["LastSync"].Value;
		newObj.Flag 	 	= N ["Flag"].AsInt;
		//newObj.GetRankDate = null;//N ["GetRankDate"].Value;
		newObj.Fights 	 	= N ["Fights"].AsInt;
		newObj.Wins 	 	= N ["Wins"].AsInt;
		newObj.Draws 	 	= N ["Draws"].AsInt;
		newObj.CurrentScore = N ["CurrentScore"].AsInt;
		newObj.LocalStatus 	= N ["LocalStatus"].AsInt;
		newObj.Country  	= N ["Country"].Value;
		newObj.Language  	= N ["Language"].Value;
		newObj.RankName  	= N ["RankName"].Value;
		newObj.SignName  	= N ["SignName"].Value;
		newObj.AddressTo  	= N ["AddressTo"].Value;
		//newObj.BindingSign 	= N ["BindingSign"].AsArray;
		newObj.Token 	 	= N ["Token"].Value;
		newObj.GameStatus  	= N ["GameStatus"].AsInt;
		newObj.RankId  		= N ["RankId"].AsInt;
		newObj.SysMessage	= N ["SysMessage"].Value;
		
		return newObj;
	}


	public static Fight ParseFightWthFriendResponse(string pJsonStr){

		var N = JSON.Parse(pJsonStr);
		var mLetsFightResult = N["LetsFightWithFriendResult"];

		Fight fightObj = new Fight ();

		fightObj.Id					= mLetsFightResult ["Id"].AsInt;
		fightObj.InitiatorId		= mLetsFightResult ["InitiatorId"].AsInt;
		fightObj.FightState			= mLetsFightResult ["FightState"].AsInt;
		fightObj.FightTypeId		= mLetsFightResult ["FightTypeId"].AsInt;
		fightObj.TaskId				= mLetsFightResult ["TaskId"].AsInt;
		fightObj.OpponentId			= mLetsFightResult ["OpponentId"].AsInt;
		fightObj.InitiatorScore		= mLetsFightResult ["InitiatorScore"].AsInt;
		fightObj.InitiatorAnswer	= mLetsFightResult ["InitiatorAnswer"].AsInt; 
		fightObj.OpponentScore 		= mLetsFightResult ["OpponentScore"].AsInt;
		fightObj.OpponentAnswer		= mLetsFightResult ["OpponentAnswer"].AsInt;
		fightObj.Winner 			= mLetsFightResult ["Winner"].AsInt;
		fightObj.Looser 			= mLetsFightResult ["Looser"].AsInt;
		fightObj.IsDraw				= mLetsFightResult ["IsDraw"].AsBool;

		fightObj.RealInitiatorAnswer	= mLetsFightResult ["RealInitiatorAnswer"].AsInt;
		fightObj.RealOpponentAnswer		= mLetsFightResult ["RealOpponentAnswer"].AsInt;
		fightObj.WinnerScore	= mLetsFightResult ["WinnerScore"].AsInt;
		fightObj.LooserScore		= mLetsFightResult ["LooserScore"].AsInt;

		return fightObj;
	}


	public static Fight ParseFightResponse(string pJsonStr){

		var N = JSON.Parse(pJsonStr);
		var mLetsFightResult = N["LetsFightResult"];

		Fight fightObj = new Fight ();

		fightObj.Id					= mLetsFightResult ["Id"].AsInt;
		fightObj.InitiatorId		= mLetsFightResult ["InitiatorId"].AsInt;
		fightObj.FightState			= mLetsFightResult ["FightState"].AsInt;
		fightObj.FightTypeId		= mLetsFightResult ["FightTypeId"].AsInt;
		fightObj.TaskId				= mLetsFightResult ["TaskId"].AsInt;
		fightObj.OpponentId			= mLetsFightResult ["OpponentId"].AsInt;
		fightObj.InitiatorScore		= mLetsFightResult ["InitiatorScore"].AsInt;
		fightObj.InitiatorAnswer	= mLetsFightResult ["InitiatorAnswer"].AsInt; 
		fightObj.OpponentScore 		= mLetsFightResult ["OpponentScore"].AsInt;
		fightObj.OpponentAnswer		= mLetsFightResult ["OpponentAnswer"].AsInt;
		fightObj.Winner 			= mLetsFightResult ["Winner"].AsInt;
		fightObj.Looser 			= mLetsFightResult ["Looser"].AsInt;
		fightObj.IsDraw				= mLetsFightResult ["IsDraw"].AsBool;

		fightObj.RealInitiatorAnswer	= mLetsFightResult ["RealInitiatorAnswer"].AsInt;
		fightObj.RealOpponentAnswer		= mLetsFightResult ["RealOpponentAnswer"].AsInt;
		fightObj.WinnerScore	= mLetsFightResult ["WinnerScore"].AsInt;
		fightObj.LooserScore		= mLetsFightResult ["LooserScore"].AsInt;

		return fightObj;
	}

	public static Fight ParseRoundFightResponse(string pJsonStr){

		var N = JSON.Parse(pJsonStr);
		var mLetsFightResult = N["GetRoundStateResult"];

		Fight fightObj = new Fight ();

		fightObj.Id					= mLetsFightResult ["Id"].AsInt;
		fightObj.InitiatorId		= mLetsFightResult ["InitiatorId"].AsInt;
		fightObj.FightState			= mLetsFightResult ["FightState"].AsInt;
		fightObj.FightTypeId		= mLetsFightResult ["FightTypeId"].AsInt;
		fightObj.TaskId				= mLetsFightResult ["TaskId"].AsInt;
		fightObj.OpponentId			= mLetsFightResult ["OpponentId"].AsInt;
		fightObj.InitiatorScore		= mLetsFightResult ["InitiatorScore"].AsInt;
		fightObj.InitiatorAnswer	= mLetsFightResult ["InitiatorAnswer"].AsInt; 
		fightObj.OpponentScore 		= mLetsFightResult ["OpponentScore"].AsInt;
		fightObj.OpponentAnswer		= mLetsFightResult ["OpponentAnswer"].AsInt;
		fightObj.Winner 			= mLetsFightResult ["Winner"].AsInt;
		fightObj.Looser 			= mLetsFightResult ["Looser"].AsInt;
		fightObj.IsDraw				= mLetsFightResult ["IsDraw"].AsBool;

		fightObj.RealInitiatorAnswer	= mLetsFightResult ["RealInitiatorAnswer"].AsInt;
		fightObj.RealOpponentAnswer		= mLetsFightResult ["RealOpponentAnswer"].AsInt;
		fightObj.WinnerScore	= mLetsFightResult ["WinnerScore"].AsInt;
		fightObj.LooserScore		= mLetsFightResult ["LooserScore"].AsInt;

		return fightObj;
	}

	public static Fight ParseFightStateResponse(string pJsonStr){

		var N = JSON.Parse(pJsonStr);
		var mLetsFightResult = N["GetFightStateResult"];

		Fight fightObj = new Fight ();

		fightObj.Id					= mLetsFightResult ["Id"].AsInt;
		fightObj.InitiatorId		= mLetsFightResult ["InitiatorId"].AsInt;
		fightObj.FightState			= mLetsFightResult ["FightState"].AsInt;
		fightObj.FightTypeId		= mLetsFightResult ["FightTypeId"].AsInt;
		fightObj.TaskId				= mLetsFightResult ["TaskId"].AsInt;
		fightObj.OpponentId			= mLetsFightResult ["OpponentId"].AsInt;
		fightObj.InitiatorScore		= mLetsFightResult ["InitiatorScore"].AsInt;
		fightObj.InitiatorAnswer	= mLetsFightResult ["InitiatorAnswer"].AsInt; 
		fightObj.OpponentScore 		= mLetsFightResult ["OpponentScore"].AsInt;
		fightObj.OpponentAnswer		= mLetsFightResult ["OpponentAnswer"].AsInt;
		fightObj.Winner 			= mLetsFightResult ["Winner"].AsInt;
		fightObj.Looser 			= mLetsFightResult ["Looser"].AsInt;
		fightObj.IsDraw				= mLetsFightResult ["IsDraw"].AsBool;

		fightObj.RealInitiatorAnswer	= mLetsFightResult ["RealInitiatorAnswer"].AsInt;
		fightObj.RealOpponentAnswer		= mLetsFightResult ["RealOpponentAnswer"].AsInt;
		fightObj.WinnerScore	= mLetsFightResult ["WinnerScore"].AsInt;
		fightObj.LooserScore		= mLetsFightResult ["LooserScore"].AsInt;

		return fightObj;
	}

	public static Fight ParseFightStateResponseByNode(string pJsonStr, string nodeName){

		var N = JSON.Parse(pJsonStr);
		var mLetsFightResult = N[nodeName];

		Fight fightObj = new Fight ();

		fightObj.Id					= mLetsFightResult ["Id"].AsInt;
		fightObj.InitiatorId		= mLetsFightResult ["InitiatorId"].AsInt;
		fightObj.FightState			= mLetsFightResult ["FightState"].AsInt;
		fightObj.FightTypeId		= mLetsFightResult ["FightTypeId"].AsInt;
		fightObj.TaskId				= mLetsFightResult ["TaskId"].AsInt;
		fightObj.OpponentId			= mLetsFightResult ["OpponentId"].AsInt;
		fightObj.InitiatorScore		= mLetsFightResult ["InitiatorScore"].AsInt;
		fightObj.InitiatorAnswer	= mLetsFightResult ["InitiatorAnswer"].AsInt; 
		fightObj.OpponentScore 		= mLetsFightResult ["OpponentScore"].AsInt;
		fightObj.OpponentAnswer		= mLetsFightResult ["OpponentAnswer"].AsInt;
		fightObj.Winner 			= mLetsFightResult ["Winner"].AsInt;
		fightObj.Looser 			= mLetsFightResult ["Looser"].AsInt;
		fightObj.IsDraw				= mLetsFightResult ["IsDraw"].AsBool;

		fightObj.RealInitiatorAnswer	= mLetsFightResult ["RealInitiatorAnswer"].AsInt;
		fightObj.RealOpponentAnswer		= mLetsFightResult ["RealOpponentAnswer"].AsInt;
		fightObj.WinnerScore	= mLetsFightResult ["WinnerScore"].AsInt;
		fightObj.LooserScore		= mLetsFightResult ["LooserScore"].AsInt;

		return fightObj;
	}

	public static List<TestTask> GetListOfTasks(string pJsonStr, string typeOfmethod){

		var N = JSON.Parse(pJsonStr);
		var mGetResult = N[typeOfmethod].AsArray;
		List<TestTask> newObj = new List<TestTask> ();

		foreach (var item in mGetResult) {
			TestTask newItem = ParseGetTaskResponse(item.ToString());
			newObj.Add(newItem);
		}

		return newObj;
	}

	public static TestTask ParseGetTaskResponse(string pJsonStr){

		var mGetResult = JSON.Parse(pJsonStr);
		//var mGetResult = N["GetTaskResult"];

		TestTask newObj = new TestTask ();

		newObj.FightId		= mGetResult ["FightId"].AsInt;
		newObj.QNum			= mGetResult ["QNum"].AsInt;
		newObj.TaskId		= mGetResult ["TaskId"].AsInt;
		newObj.TrueValue 	= mGetResult ["TrueValue"].AsInt;
		newObj.WasBought 	= mGetResult ["WasBought"].AsInt;

		newObj.TextQuestion = mGetResult ["TextQuestion"].Value;
		newObj.Ans1 = mGetResult ["Ans1"].Value; 
		newObj.Ans2 = mGetResult ["Ans2"].Value;
		newObj.Ans3 = mGetResult ["Ans3"].Value;
		newObj.Ans4 = mGetResult ["Ans4"].Value;

		newObj.PicQuestion = Utility.getByteArray (mGetResult, "PicQuestion");//mGetResult ["PicQuestion"].AsArray;
		newObj.Var1 = Utility.getByteArray (mGetResult, "Var1");//mGetResult ["Var1"].AsArray; 
		newObj.Var2 = Utility.getByteArray (mGetResult, "Var2");//mGetResult ["Var2"].AsArray; 
		newObj.Var3 = Utility.getByteArray (mGetResult, "Var3");//mGetResult ["Var3"].AsArray;
		newObj.Var4 = Utility.getByteArray (mGetResult, "Var4");//mGetResult ["Var4"].AsArray;

		return newObj;
	}

	public static byte[] getByteArray(JSONNode jsClass, string parameterName){

		byte[] buffer = null;

		try{

			var PicQuestion = jsClass[parameterName].AsArray;

			if(PicQuestion != null){

				buffer = new byte[PicQuestion.Count];
				for(int i = 0 ; i < PicQuestion.Count ; i++)
				{
					buffer[i] =  Convert.ToByte(PicQuestion[i].Value);
				}

				//File.WriteAllBytes(Utility.getDcumentsPath() + "/Resources/" + ItemId.ToString() + "_ItemImage" + ".png", buffer);
				Debug.Log("Convert was complete.");
			}

		}catch(Exception ex){
			Debug.Log (ex.Message);
			return null;
		}

		return buffer;
	}
		
	public static bool ItemsIsOwned(string shieldNum, string key, MainScreenManager profile){

		string values = UserController.currentUser.Motto;

//		if (key == "eagles") {
//			values = UserController.currentUser.Motto;
//		} else if (key == "difference") {
//			values = UserController.currentUser.AddressTo;
//		}else if (key == "shields") {
//			values = UserController.currentUser.SignName;
//		}

		//profile.SaveUser ();

		if (values != null && values.Length > 0) {
		
			var jsonStr = JSON.Parse (values);//.AsArray;

			//JSONArray listNode = new JSONArray();


			var listNode = jsonStr [key].AsArray;

			if (listNode != null) {
				foreach (JSONData i in listNode) {
					if (i.Value == shieldNum) {
						return true;
					}
				}

				JSONNode jnod = new JSONNode();
				jnod.Add (shieldNum);

				listNode.Add (shieldNum);

				jsonStr [key] = listNode;

			} else {
			

				JSONArray listNode1 = new JSONArray();

				JSONNode jnod1 = new JSONNode();
				jnod1.Add (shieldNum);

				listNode1.Add (shieldNum);

				jsonStr.Add (key,	listNode1);
			}



			UserController.currentUser.Motto = jsonStr.ToString();

//			if (key == "eagles") {
//				UserController.currentUser.Motto = listNode.ToString();
//			} else if (key == "difference") {
//				UserController.currentUser.AddressTo = listNode.ToString();
//			}else if (key == "shields") {
//				UserController.currentUser.SignName = listNode.ToString();
//			}

			profile.SaveUser ();
			//PlayerPrefs.SetString (key, listNode.ToString());

			return false;
		} else {
		
			JSONArray listNode = new JSONArray();

			JSONNode jnod = new JSONNode();
			jnod.Add (shieldNum);

			listNode.Add (shieldNum);


			JSONClass rootNode = new JSONClass ();
			rootNode.Add (key,	listNode);

			UserController.currentUser.Motto = rootNode.ToString();

//			if (key == "eagles") {
//				UserController.currentUser.Motto = listNode.ToString();
//			} else if (key == "difference") {
//				UserController.currentUser.AddressTo = listNode.ToString();
//			}else if (key == "shields") {
//				UserController.currentUser.SignName = listNode.ToString();
//			}

			profile.SaveUser ();
			//PlayerPrefs.SetString (key, listNode.ToString());

			return false;
		}
			
//		if (PlayerPrefs.HasKey (key)) {
//
//			string values = PlayerPrefs.GetString (key);
//
//			JSONArray listNode = new JSONArray();
//			listNode = JSON.Parse (values).AsArray;
//
//			foreach (JSONData i in listNode) {
//				if (i.Value == shieldNum) {
//					return true;
//				}
//			}
//
//			JSONNode jnod = new JSONNode();
//			jnod.Add (shieldNum);
//
//			listNode.Add (shieldNum);
//			PlayerPrefs.SetString (key, listNode.ToString());
//
//			return false;
//
//		}else{
//			JSONArray listNode = new JSONArray();
//
//			JSONNode jnod = new JSONNode();
//			jnod.Add (shieldNum);
//
//			listNode.Add (shieldNum);
//
//			PlayerPrefs.SetString (key, listNode.ToString());
//
//			return true;
//		}

	}
		
	public static string getEagleDescription(User user, List<FightStat> fightStat){

		string temlate = "Вы побеждаете в более чем {0}% поединков.\n\n Вы получаете {1} Орлел Интеллектуальной лиги.";
		var eagle = EaglsManager.getEagl (fightStat);

		if (eagle != null) {
			//Storage.GiveMoney (eagle.reward);
			return string.Format (temlate, eagle.startScore, eagle.description);
		}

		return "";
	}

	public static string getDifferenceDescription(User user, List<FightStat> fightStat){

		string temlate = "Лучше всего Вы проявили себя в поединках {0}.\n\nВы получаете титул \"{1}\".\n\nПоздравляем!";
		double maxResult = -1;
		int FightType = -1;

		if (getNumberOfShield ( fightStat).shieldNumber != "1") {
			foreach (var c in fightStat) {
				if (c.FightTypeId != 0) {
					if (c.Result > maxResult) {
						maxResult = c.Result;
						FightType = c.FightTypeId;
					}
				}
			}

			//return getRunkByNumber (FightType);
		} else {
			return "";
		}

		return string.Format(temlate, ScreensManager.LMan.getString(FightTypeName(FightType)), ScreensManager.LMan.getString(getRunkByNumber (FightType)));
	}
		
	public static string getShieldDescription(User user, List<FightStat> fightStat){
	
		string temlate = "Вы одержали {0} побед\n\nВы заработали {1} баллов.\n\nВы получаете {2}.\n\nВы получаете {3} талантов\n\nПоздравляем!";

		foreach (var c in fightStat) {
			if (c.FightTypeId == 0 && c.Fights >= Constants.fightsCount) {

				var image = getShieldNumByScore (c.Score);

				if (image.shieldNumber != "1") {
					temlate = string.Format (temlate, c.RowWins, c.Score, image.description, image.reward);
					Storage.GiveMoney (image.reward);
				}
				else {
					temlate = "Вы вошли в Интеллектуальную лигу.\n\nВы получаете ЖЕЛЕЗНЫЙ ЩИТ.\n\nВы получаете 20 талантов\n\nПоздравляем и желаем хороших дуэлей!";
					Storage.GiveMoney (image.reward);
				}
			}
		}

		return temlate;
	}

	public static shieldDataObj getShieldNumByScore(double fightStat){

		for (int i = 0; i < shieldsArray.Length; i++) {
			if (shieldsArray [i].startScore <= fightStat && shieldsArray [i].endScore >= fightStat) {
				return shieldsArray [i];
			}
		}

		return shieldsArray [0];
	}

	public static shieldDataObj getNumberOfShield(List<FightStat> fightStat){
		
		foreach (var c in fightStat) {
			if (c.FightTypeId == 0 && c.Fights >= Constants.fightsCount) {

				return getShieldNumByScore (c.Score);

			}
		}

		return shieldsArray[0];
	}

	public static void setAvatar(Image imgAvatar, List<FightStat> fightStat){
	
		string image = getNumberOfShield(fightStat).shieldNumber;

		Sprite spr = null;
		Texture2D tex = (Texture2D)Resources.Load(image);
		if (tex != null) {
			spr = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));

			imgAvatar.sprite = spr;
		}
	}

	public static void setAvatarByState(Image imgAvatar, double fightStat){

		string image = getShieldNumByScore (fightStat).shieldNumber;

		Sprite spr = null;
		Texture2D tex = (Texture2D)Resources.Load(image);
		if (tex != null) {
			spr = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));

			imgAvatar.sprite = spr;
		}
	}

	public static string GetDifference(User user, List<FightStat> fightStat){

		double maxResult = -1;
		int FightType = -1;

		if (getNumberOfShield ( fightStat).shieldNumber != "1") {
			foreach (var c in fightStat) {
				if (c.FightTypeId != 0) {
					if (c.Result > maxResult) {
						maxResult = c.Result;
						FightType = c.FightTypeId;
					}
				}
			}

			return getRunkByNumber (FightType);
		} else {
			return "";
		}
			
	}

	public static string getRunkByNumber(int FightType){

		switch (FightType) {
		case 1:
			return "@expert";
		case 2:
			return "@scientist";
		case 3:
			return "@wizard";
		case 4:
			return "@predtor";
		case 5:
			return "@lucky";
		case 6:
			return "@keeper";
		case 7:
			return "@prophet";
		default:
			return "";
		}

	}

	public static string FightTypeName(int id){

		switch (id) {
		case 1:
			return "@knowledge";
		case 2:
			return "@intelligence";
		case 3:
			return "@wisdom";
		case 4:
			return "@reflex";
		case 5:
			return "@fortune";
		case 6:
			return "@memory";
		case 7:
			return "@intuition";
		default:
			return "";
		}

	}

	public static Color Parse(string hexstring)
	{
		if (hexstring.StartsWith("#"))
		{
			hexstring = hexstring.Substring(1);
		}

		if (hexstring.StartsWith("0x"))
		{
			hexstring = hexstring.Substring(2);
		}

		if (hexstring.Length != 6) 
		{
			throw new Exception(string.Format("{0} is not a valid color string.", hexstring));
		}

		byte r = byte.Parse(hexstring.Substring(0, 2), NumberStyles.HexNumber);
		byte g = byte.Parse(hexstring.Substring(2, 2), NumberStyles.HexNumber);
		byte b = byte.Parse(hexstring.Substring(4, 2), NumberStyles.HexNumber);

		return new Color32(r, g, b, 1);
	}

	public static bool hasSubscription(){

		return TestPurch.hasSubscriptionRequest ();
	}
		
	public static void setEagle(Image imgAvatar, List<FightStat> fightStat){

		string image = "";

		eaglsData eagle = EaglsManager.getEagl (fightStat);

		if (eagle == null) {
			return;
		}
			
		image = "eagle" + eagle.eagleNumber;

		Sprite spr = null;
		Texture2D tex = (Texture2D)Resources.Load(image);
		if (tex != null) {
			spr = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));

			imgAvatar.sprite = spr;
		} else {
			imgAvatar.gameObject.SetActive (false);
		}
	}
}