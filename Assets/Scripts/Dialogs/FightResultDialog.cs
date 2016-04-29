﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Soomla.Store;


public class FightResultDialog : MonoBehaviour
{

	public GameObject fightResultPanelObject;
	public Text actionDescription;
	public Button okButton;
	public Text scoreUser;

	public GameObject imageWin;
	public GameObject imageLose;
	public GameObject imageDraft;

	public GameObject	rightAnswerObject;
	public Text			answerDescription;

	public List<Image>	fightsImageList;

	BannerView bannerView = null;

	public void SetText (string text, int fightState, UnityAction okEvent, List<Fight> fight, string scrUs, string pRightAnswer)
	{

		if (StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) == 0 && !Utility.hide_ad) {
			RequestBanner ();
		}

		Sprite spriteDraft		= null;
		Sprite spriteWin		= null;
		Sprite spriteLose		= null;
		Sprite spriteDefault	= null;

		Texture2D texture = Resources.Load ("draft_sign") as Texture2D;
		if (texture != null) {
			spriteDraft = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
		}

		texture = Resources.Load ("win_sign") as Texture2D;
		if (texture != null) {
			spriteWin = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
		}

		texture = Resources.Load ("lose_sign") as Texture2D;
		if (texture != null) {
			spriteLose = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
		}

		if (spriteDefault == null) {
			texture = Resources.Load ("default_sign") as Texture2D;
			if (texture != null) {
				spriteDefault = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
			}
		}

		foreach (var cc in fightsImageList) {
			cc.sprite = spriteDefault;
		}

		fightResultPanelObject.SetActive (true);

		foreach (var currentFight in fight) {
			if (currentFight.IsDraw == true) {
				fightsImageList [fight.IndexOf (currentFight)].sprite = spriteDraft;
			} else if (currentFight.Winner == UserController.currentUser.Id) {
				fightsImageList [fight.IndexOf (currentFight)].sprite = spriteWin;
			} else {
				fightsImageList [fight.IndexOf (currentFight)].sprite = spriteLose;
			}
		}

		actionDescription.text = System.Text.RegularExpressions.Regex.Unescape (text);
		scoreUser.text = scrUs;

		if (fightState == -1) {
			imageLose.SetActive (true);
		} else if (fightState == 0) {
			imageDraft.SetActive (true);
		} else {
			imageWin.SetActive (true);
		}

		okButton.onClick.RemoveAllListeners ();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);
		
		okButton.gameObject.SetActive (true);

		//Show right answer
		if(StoreInventory.GetItemBalance (BuyItems.NO_ADS_NONCONS.ItemId) != 0 || Utility.TESTING_MODE && fight.Count == 1 && 
			(fight[fight.Count-1].FightTypeId == 1 || fight[fight.Count-1].FightTypeId == 2 || fight[fight.Count-1].FightTypeId == 3 
				|| fight[fight.Count-1].FightTypeId == 5)){

			rightAnswerObject.SetActive (true);
			answerDescription.text = pRightAnswer;
		}
	}

	public void ClosePanel ()
	{
		rightAnswerObject.SetActive (false);
		imageLose.SetActive (false);
		imageDraft.SetActive (false);
		imageWin.SetActive (false);

		fightResultPanelObject.SetActive (false);

		if (bannerView != null) {
			bannerView.Hide ();
			bannerView.Destroy ();
		}
	}

	private void RequestBanner ()
	{
		#if UNITY_ANDROID
		string adUnitId = Constants.BANNER_ID_KEY_ANDROID;
		#elif UNITY_IPHONE
		string adUnitId = Constants.BANNER_ID_KEY_IOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif


		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Top);

		bannerView.OnAdLoaded += HandleAdLoaded;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder ()
		.TagForChildDirectedTreatment (true)
		.Build ();

		// Load the banner with the request.
		bannerView.LoadAd (request);
	}

	public void HandleAdLoaded (object sender, System.EventArgs args)
	{
		bannerView.Show ();
	}
}
