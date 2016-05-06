using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class InitSocialNetworks : MonoBehaviour {


	private SucsessRegistrationDelegate		mSuccessDlg	= null;
	private FailRegistrationDelegate		mFailDlg		= null;

	// Use this for initialization
	void Start () {
	}

	public void InitNetworks(){

		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()            

			// require access to a player's Google+ social graph to sign in
			.RequireGooglePlus()
			.Build();

		PlayGamesPlatform.InitializeInstance(config);
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();

		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init (InitCallback, null);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp ();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp ();

		} else {
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}
	}

	public void GoogleGetToken(SucsessRegistrationDelegate successDlg, FailRegistrationDelegate failDlg){

		if (!Social.localUser.authenticated) {
			// Authenticate


			Social.localUser.Authenticate((bool success) => {

				if (success) {

					PlayGamesPlatform.Instance.GetServerAuthCode((CommonStatusCodes status,string code) => 
						{
							if(status == CommonStatusCodes.Success || status == CommonStatusCodes.SuccessCached){
								StartCoroutine(getGoogleToken(successDlg));
							}else{
								failDlg(Constants.GOOGLE_PLAY);
							}
						}
					);

				} else {					
					failDlg(Constants.GOOGLE_PLAY);
				}
			});
		} 
	}

	IEnumerator getGoogleToken(SucsessRegistrationDelegate successDlg){

		string mStatusText = "Welcome " + Social.localUser.userName;
		mStatusText += "\n Email " + ((PlayGamesLocalUser)Social.localUser).Email;
		mStatusText += "\n Token ID is " + GooglePlayGames.PlayGamesPlatform.Instance.GetToken();
		mStatusText += "\n AccessToken is " + GooglePlayGames.PlayGamesPlatform.Instance.GetAccessToken();

		yield return new WaitForSeconds (5);

		mStatusText = "Welcome " + Social.localUser.userName;

		mStatusText += "\n Email " + ((PlayGamesLocalUser)Social.localUser).Email;
		mStatusText += "\n Token ID is " + GooglePlayGames.PlayGamesPlatform.Instance.GetToken();
		mStatusText += "\n AccessToken is " + GooglePlayGames.PlayGamesPlatform.Instance.GetAccessToken();

		successDlg (Constants.GOOGLE_PLAY);
	}

	public void FBLogin(SucsessRegistrationDelegate successDlg, FailRegistrationDelegate failDlg){
		mSuccessDlg = successDlg;
		mFailDlg = failDlg;

		var perms = new List<string>(){"public_profile", "email", "user_friends"};
		FB.LogInWithReadPermissions(perms, AuthCallback);
	}

	private void AuthCallback (ILoginResult result) {

		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			// Print current access token's granted permissions
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}

			mSuccessDlg (Constants.FACEBOOK);

		} else {

			mFailDlg (Constants.FACEBOOK);
			Debug.Log("User cancelled login");
		}
	}

	private void FetchFBProfile () {
		FB.API("/me?fields=first_name,last_name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string,string>(){});
	}

	private void FetchProfileCallback (IGraphResult result) {

		Debug.Log (result.RawResult);

		Dictionary<string,object> FBUserDetails = (Dictionary<string,object>)result.ResultDictionary;

		Debug.Log ("Profile: first name: " + FBUserDetails["first_name"]);
		Debug.Log ("Profile: last name: " + FBUserDetails["last_name"]);
		Debug.Log ("Profile: id: " + FBUserDetails["id"]);
		Debug.Log ("Profile: email: " + FBUserDetails["email"]);

	}
}
