using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using UnityEngine.SocialPlatforms;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif


public class InitSocialNetworks : MonoBehaviour {


	private SucsessRegistrationDelegate		mSuccessDlg	= null;
	private FailRegistrationDelegate		mFailDlg		= null;

	[HideInInspector]
	public bool WasInit = false;

	// Use this for initialization
	void Start () {
	}

	public void InitNetworks(){

		int regType = -1;
		bool hasKey = PlayerPrefs.HasKey ("regType");

		if (hasKey) {
			regType = PlayerPrefs.GetInt ("regType");
		}

		if (regType == Constants.GOOGLE_PLAY) {
			InitGoogle ();
		} else if (regType == Constants.FACEBOOK) {
			InitFB ();
		} else {
			InitGoogle ();
			InitFB ();
		}

	}

	public void InitGoogle(){

		#if UNITY_ANDROID
			Debug.Log ("INIT InitGoogle START");
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ()            
				.RequireGooglePlus ()
				.Build ();

			Debug.Log ("INIT InitGoogle InitializeInstance");
			PlayGamesPlatform.InitializeInstance (config);
			// recommended for debugging:

			Debug.Log ("INIT InitGoogle DebugLogEnabled");
			PlayGamesPlatform.DebugLogEnabled = true;
			// Activate the Google Play Games platform

			Debug.Log ("INIT InitGoogle Activate");
			PlayGamesPlatform.Activate ();

			Debug.Log ("INIT InitGoogle Activate END"); 
			WasInit = true;
		#endif

	}

	private void InitFB(){
		
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init (InitCallback, null);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp ();
		}

		WasInit = true;

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

		#if UNITY_ANDROID

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

		#endif
	}

	public void RenewToken(SucsessRegistrationDelegate successDlg, FailRegistrationDelegate failDlg, int regType){

		Debug.Log ("INIT RenewToken"); 
		if (regType == Constants.GOOGLE_PLAY) {

			#if UNITY_ANDROID

			if (!Social.localUser.authenticated) {
				// Authenticate

				Social.localUser.Authenticate ((bool success) => {

					if (success) {

						Debug.Log ("INIT Constants.GOOGLE_PLAY"); 
						PlayGamesPlatform.Instance.GetServerAuthCode ((CommonStatusCodes status, string code) => {
							if (status == CommonStatusCodes.Success || status == CommonStatusCodes.SuccessCached) {
								Debug.Log ("INIT StartCoroutine (getGoogleToken (successDlg))"); 
								StartCoroutine (getGoogleToken (successDlg));
							} else {
								Debug.Log ("INIT failDlg (Constants.GOOGLE_PLAY) " + code + " status " + status.ToString ()); 
								failDlg (Constants.GOOGLE_PLAY);
							}
						}
						);
					}else{
						FBLogin (successDlg, failDlg);
					}
				});

			} else {
				
				Debug.Log ("INIT Constants.GOOGLE_PLAY"); 
				PlayGamesPlatform.Instance.GetServerAuthCode ((CommonStatusCodes status, string code) => {
					if (status == CommonStatusCodes.Success || status == CommonStatusCodes.SuccessCached) {
						Debug.Log ("INIT StartCoroutine (getGoogleToken (successDlg))"); 
						StartCoroutine (getGoogleToken (successDlg));
					} else {
						Debug.Log ("INIT failDlg (Constants.GOOGLE_PLAY) " + code + " status " + status.ToString ()); 
						failDlg (Constants.GOOGLE_PLAY);
					}
				}
				);

			}

			#endif
		}else {
			FBLogin (successDlg, failDlg);
		}
	}

	IEnumerator getGoogleToken(SucsessRegistrationDelegate successDlg){

		#if UNITY_ANDROID
			string mStatusText = "Welcome " + Social.localUser.userName;
			mStatusText += "\n Email " + ((PlayGamesLocalUser)Social.localUser).Email;
			mStatusText += "\n Token ID is " + GooglePlayGames.PlayGamesPlatform.Instance.GetToken();
			mStatusText += "\n AccessToken is " + GooglePlayGames.PlayGamesPlatform.Instance.GetAccessToken();

			yield return new WaitForSeconds (5);

			Dictionary<string,object> googleUserDetails = new Dictionary<string, object>();
			googleUserDetails.Add ("username", Social.localUser.userName);
			googleUserDetails.Add ("email", ((PlayGamesLocalUser)Social.localUser).Email);
			googleUserDetails.Add ("token", GooglePlayGames.PlayGamesPlatform.Instance.GetAccessToken());

			Debug.Log ("INIT TOKEN" + GooglePlayGames.PlayGamesPlatform.Instance.GetAccessToken()); 

			successDlg (Constants.GOOGLE_PLAY, googleUserDetails);
		#elif UNITY_IPHONE || UNITY_IOS
			yield return new WaitForSeconds (5);
		#endif
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
//			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
//			// Print current access token's User ID
//			Debug.Log(aToken.UserId);
//			Debug.Log("TOKEN is:"+ aToken.TokenString);
//
//			// Print current access token's granted permissions
//			foreach (string perm in aToken.Permissions) {
//				Debug.Log(perm);
//			}

			FetchFBProfile ();

		} else {

			mFailDlg (Constants.FACEBOOK);
			Debug.Log("User cancelled login");
		}
	}

	private void FetchFBProfile () {
		FB.API("/me?fields=first_name,last_name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string,string>(){});
	}

	private void FetchProfileCallback (IGraphResult result) {

		var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;

		Dictionary<string,object> FBUserDetails = (Dictionary<string,object>)result.ResultDictionary;

		FBUserDetails.Add ("token", aToken.TokenString);
		FBUserDetails.Add ("userId", aToken.UserId);

		Debug.Log ("Profile: first name: " + FBUserDetails["first_name"]);
		Debug.Log ("Profile: last name: " + FBUserDetails["last_name"]);
		Debug.Log ("Profile: id: " + FBUserDetails["id"]);
		Debug.Log ("Profile: email: " + FBUserDetails["email"]);
		Debug.Log ("userId:" + aToken.UserId);
		Debug.Log ("TOKEN is:"+ aToken.TokenString);

		mSuccessDlg (Constants.FACEBOOK, FBUserDetails);
	}
}
