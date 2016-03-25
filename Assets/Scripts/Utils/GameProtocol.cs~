using UnityEngine;
using System.Collections;

public class GameProtocol {

	string log;
	public string AddMessage(string msg){
		log += msg + "\n";
		return log;
	}

	public string GetLog(){
		return log;
	}

	public string SetValue(string msg){
		log = msg;
		return log;
	}

	public void SaveToFile(string filename){

		//System.IO.File.WriteAllText(Utility.getDcumentsPath()+ "/" + filename + ".txt", log);
		//File.WriteAllBytes(Utility.getDcumentsPath()+ "/" + filename + ".txt", buffer);
	}
		
	public IEnumerator SendMessageToSrv(string message){

		var postScoreURL = Utility.SERVICE_BASE_URL;		
		var method = "SendMsgToGame";
		var userToken = "token=";
		var msg = "msg=";

		WWW www;

		int messageParts = message.Length / 3000;
		string buffer = "";
		for (int i = 0; i <= messageParts; i++) {

			if ((message.Length - (i * 3000)) < 3000) {
				buffer = message.Substring (i * 3000, (message.Length - (i * 3000)));
			} else {
				buffer = message.Substring (i * 3000, 3000);
			}

			//Send answer to server
			postScoreURL = Utility.SERVICE_BASE_URL;
			postScoreURL = postScoreURL + method + "?" + userToken + UserController.currentUser.Token + "&" + msg + System.Uri.EscapeUriString(buffer);

			www = new WWW (postScoreURL);

			while (!www.isDone) {
				yield return null;
			}

			if (www.error == null) {
				Debug.Log ("OK sent data to server: buffer len = " + buffer.Length.ToString());
			} else {
				Debug.Log ("Error send data to server:");
			}
		}

	}
}
