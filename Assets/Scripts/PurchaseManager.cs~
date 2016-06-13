using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class PurchaseManager : MonoBehaviour {

	private ErrorPanel		errorPanel = null;
//	private ScreensManager	screensManager	= null;
	private static PurchaseManager s_Instance = null;

	// This defines a static instance property that attempts to find the manager object in the scene and
	// returns it to the caller.
	public static PurchaseManager instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance =  FindObjectOfType(typeof (PurchaseManager)) as PurchaseManager;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("SoomlaPurchManager");
				s_Instance = obj.AddComponent(typeof (PurchaseManager)) as PurchaseManager;
				Debug.Log ("Could not locate an AManager object. \n ScreensManager was Generated Automaticly.");
			}
			return s_Instance;
		}
	}

	void Start(){

		StoreEvents.OnMarketPurchase += onMarketPurchase;
		StoreEvents.OnMarketRefund += onMarketRefund;
		StoreEvents.OnItemPurchased += onItemPurchased;
		StoreEvents.OnMarketPurchaseStarted += onMarketPurchaseStarted;
		StoreEvents.OnItemPurchaseStarted += onItemPurchaseStarted;
		StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
		StoreEvents.OnGoodBalanceChanged += onGoodBalanceChanged;
		StoreEvents.OnRestoreTransactionsStarted += onRestoreTransactionsStarted;
		StoreEvents.OnRestoreTransactionsFinished += onRestoreTransactionsFinished;

		StoreEvents.OnSoomlaStoreInitialized += OnSoomlaStoreInitialized;
		StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;
		Soomla.Store.SoomlaStore.Initialize (new BuyItems ());

	}

	/// <summary>
	/// Handles a market purchase event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	/// <param name="purchaseToken">Purchase token.</param>
	public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra) {
		Debug.Log ("onMarketPurchase");

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("Покупка прошла успешно.", ErrorEvent);
	}

	/// <summary>
	/// Handles a market refund event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onMarketRefund(PurchasableVirtualItem pvi) {
		Debug.Log ("onMarketRefund");

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("Покупка отменена.", ErrorEvent);
	}

	/// <summary>
	/// Handles an item purchase event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onItemPurchased(PurchasableVirtualItem pvi, string payload) {
		Debug.Log ("onItemPurchased");

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("onItemPurchased", ErrorEvent);
	}

	/// <summary>
	/// Handles a market purchase started event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onMarketPurchaseStarted(PurchasableVirtualItem pvi) {
		Debug.Log ("onMarketPurchaseStarted");

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("onMarketPurchaseStarted", ErrorEvent);
	}

	/// <summary>
	/// Handles an item purchase started event.
	/// </summary>
	/// <param name="pvi">Purchasable virtual item.</param>
	public void onItemPurchaseStarted(PurchasableVirtualItem pvi) {
		Debug.Log ("onItemPurchaseStarted");

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("onItemPurchaseStarted", ErrorEvent);
	}

	/// <summary>
	/// Handles a currency balance changed event.
	/// </summary>
	/// <param name="virtualCurrency">Virtual currency whose balance has changed.</param>
	/// <param name="balance">Balance of the given virtual currency.</param>
	/// <param name="amountAdded">Amount added to the balance.</param>
	public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded) {
		Debug.Log ("onCurrencyBalanceChanged");

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("onCurrencyBalanceChanged", ErrorEvent);
	}

	/// <summary>
	/// Handles a good balance changed event.
	/// </summary>
	/// <param name="good">Virtual good whose balance has changed.</param>
	/// <param name="balance">Balance.</param>
	/// <param name="amountAdded">Amount added.</param>
	public void onGoodBalanceChanged(VirtualGood good, int balance, int amountAdded) {
		Debug.Log ("onGoodBalanceChanged");

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("onGoodBalanceChanged", ErrorEvent);
	}

	/// <summary>
	/// Handles a restore Transactions process started event.
	/// </summary>
	public void onRestoreTransactionsStarted() {
		Debug.Log ("onRestoreTransactionsStarted");

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("onRestoreTransactionsStarted", ErrorEvent);
	}

	/// <summary>
	/// Handles a restore transactions process finished event.
	/// </summary>
	/// <param name="success">If set to <c>true</c> success.</param>
	public void onRestoreTransactionsFinished(bool success) {
		Debug.Log ("onRestoreTransactionsFinished");

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("onRestoreTransactionsFinished", ErrorEvent);
	}

	public void OnSoomlaStoreInitialized(){
		Debug.Log("SOOMLA INITIALIZED");
	}

	public void onUnexpectedStoreError(int errorCode) {
		
		Debug.Log ("SOOMLA ExampleEventHandler error with code: " + errorCode);

		//screensManager	= ScreensManager.instance;
		//errorPanel = screensManager.ShowErrorDialog("Ошибка подключения к магазину приложений: " + errorCode, ErrorEvent);
	}

	public void ReleaseLiveObject(){
		StoreInventory.TakeItem(StoreInfo.Goods[0].ItemId, 1);
	}

	public void ErrorEvent(){
		GameObject.Destroy(errorPanel.gameObject);
		errorPanel = null;
	}

	public void BuyLifeTimeItem(){
	
	}
}
