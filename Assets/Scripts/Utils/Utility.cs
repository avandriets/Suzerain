using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System;
using System.Globalization;
using Soomla.Store;


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

public static class Utility {

	public static bool StopCoroutine = true;

	public static bool	TESTING_MODE = true;
	//public static string SERVICE_BASE_URL	= "http://suzerain.westeurope.cloudapp.azure.com:8062/SuzerainWcfService/SuzerainService/";
	public static string SERVICE_BASE_URL	= "http://suzerain.westeurope.cloudapp.azure.com:9062/SuzerainWcfService/SuzerainService/";
	public static string LOGIN_URL			= "Login";
	public static string LETSFIGHT_URL		= "LetsFight";
	public static string STATE_FIGTS_URL	= "GetUserStats";
	public static string STATE_LOCATION_FIGTS_URL	= "GetLocStats";
	public static string REGISTER_USER_URL	= "RegisterUser";
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

		return fightObj;
	}

	public static List<TestTask> GetListOfTasks(string pJsonStr){

		var N = JSON.Parse(pJsonStr);
		var mGetResult = N["GetTaskResult"].AsArray;
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

	public static bool ShieldIsOwned(string shieldNum){

		//PlayerPrefs.DeleteKey ("shields");

		if (PlayerPrefs.HasKey ("shields")) {

			string values = PlayerPrefs.GetString ("shields");

			JSONArray listNode = new JSONArray();
			listNode = JSON.Parse (values).AsArray;

			foreach (JSONData i in listNode) {
				if (i.Value == shieldNum) {
					return true;
				}
			}

			JSONNode jnod = new JSONNode();
			jnod.Add (shieldNum);

			listNode.Add (shieldNum);
			PlayerPrefs.SetString ("shields", listNode.ToString());

			return false;

		}else{
			JSONArray listNode = new JSONArray();

			JSONNode jnod = new JSONNode();
			jnod.Add (shieldNum);

			listNode.Add (shieldNum);

			PlayerPrefs.SetString ("shields", listNode.ToString());

			return true;
		}
			
	}

	public static string getShieldDescription(User user, List<FightStat> fightStat){
	
		string image = "1";
		foreach (var c in fightStat) {
			if (c.FightTypeId == 0 && c.Fights >= Constants.fightsCount) {
				if (c.Result * 100 <= 29) {
					image = "1";
				} else if (c.Result*100 <= 39 && c.Result*100 > 29) {
					image = "Вы побеждаете в более чем \n\n30% поединков.\n\nВы получаете \n\nСТАЛЬНОЙ ЩИТ.";
				}else if (c.Result*100 <= 49 && c.Result*100 > 39) {
					image = "Вы побеждаете в более чем \n\n40% поединков.\n\nВы получаете \n\nБРОНЗОВЫЙ ЩИТ.";
				}else if (c.Result*100 <= 59 && c.Result*100 > 49) {
					image = "Вы побеждаете в более чем \n\n50% поединков.\n\nВы получаете \n\nСЕРЕБРЯНЫЙ ЩИТ.";
				}else if (c.Result*100 <= 69 && c.Result*100 > 59) {
					image = "Вы побеждаете в более чем \n\n60% поединков.\n\nВы получаете \n\nЗОЛОТОЙ ЩИТ.";
				}else if (c.Result*100 <= 79 && c.Result*100 > 69) {
					image = "Вы побеждаете в более чем \n\n70% поединков.\n\nВы получаете \n\nПЛАТИНОВЫЙ ЩИТ.";
				}else if (c.Result*100 <= 100 && c.Result*100 > 79) {
					image = "Вы побеждаете в более чем \n\n80% поединков.\n\nВы получаете \n\nКРИСТАЛЬНЫЙ ЩИТ.";
				}
			}
		}

		return image;
	}

	public static string getNumberOfShield(User user, List<FightStat> fightStat){
		
		string image = "1";
		foreach (var c in fightStat) {
			if (c.FightTypeId == 0 && c.Fights >= Constants.fightsCount) {
				if (c.Result * 100 <= 29) {
					image = "1";
				} else if (c.Result*100 <= 39 && c.Result*100 > 29) {
					image = "2";
				}else if (c.Result*100 <= 49 && c.Result*100 > 39) {
					image = "3";
				}else if (c.Result*100 <= 59 && c.Result*100 > 49) {
					image = "4";
				}else if (c.Result*100 <= 69 && c.Result*100 > 59) {
					image = "5";
				}else if (c.Result*100 <= 79 && c.Result*100 > 69) {
					image = "6";
				}else if (c.Result*100 <= 100 && c.Result*100 > 79) {
					image = "7";
				}
			}
		}

		return image;
	}

	public static void setAvatar(Image imgAvatar, User user, List<FightStat> fightStat){
	
		string image = "1";
		foreach (var c in fightStat) {
			if (c.FightTypeId == 0 && c.Fights >= Constants.fightsCount) {
				if (c.Result * 100 <= 29) {
					image = "1";
				} else if (c.Result*100 <= 39 && c.Result*100 > 29) {
					image = "2";
				}else if (c.Result*100 <= 49 && c.Result*100 > 39) {
					image = "3";
				}else if (c.Result*100 <= 59 && c.Result*100 > 49) {
					image = "4";
				}else if (c.Result*100 <= 69 && c.Result*100 > 59) {
					image = "5";
				}else if (c.Result*100 <= 79 && c.Result*100 > 69) {
					image = "6";
				}else if (c.Result*100 <= 100 && c.Result*100 > 79) {
					image = "7";
				}
			}
		}

		Sprite spr = null;
		Texture2D tex = (Texture2D)Resources.Load(image);
		if (tex != null) {
			spr = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));

			imgAvatar.sprite = spr;
		}
	}

	public static void setAvatarByState(Image imgAvatar, double fightStat){

		string image = "1";

		if (fightStat * 100 <= 29) {
					image = "1";
		} else if (fightStat*100 <= 39 && fightStat*100 > 29) {
					image = "2";
		}else if (fightStat*100 <= 49 && fightStat*100 > 39) {
					image = "3";
		}else if (fightStat*100 <= 59 && fightStat*100 > 49) {
					image = "4";
		}else if (fightStat*100 <= 69 && fightStat*100 > 59) {
					image = "5";
		}else if (fightStat*100 <= 79 && fightStat*100 > 69) {
					image = "6";
		}else if (fightStat*100 <= 100 && fightStat*100 > 79) {
					image = "7";
		}

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

		foreach (var c in fightStat) {
			if (c.FightTypeId != 0 ) {
				if (c.Result > maxResult) {
					maxResult = c.Result;
					FightType = c.FightTypeId;
				}
			}
		}

		return getRunkByNumber (FightType);
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

//	public static double GetNumericPrice(this VirtualGood good)
//	{
//		return (good.PurchaseType as PurchaseWithMarket).MarketItem.Price;
//	}
}