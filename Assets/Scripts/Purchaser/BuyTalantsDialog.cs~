using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using Soomla;
using Soomla.Store;


public class BuyTalantsDialog : BaseDialog {

	public Text textTalants_10;
	public Text textTalants_30;
	public Text textTalants_50;
	public Text textTalants_100;

	public Button buttonTalants_10;
	public Button buttonTalants_30;
	public Button buttonTalants_50;
	public Button buttonTalants_100;


	protected override void InitDialog (){

		buttonTalants_10.onClick.RemoveAllListeners();
		buttonTalants_10.onClick.AddListener (()=> {
			BuyProduct(BuyItems.TEN_TALENT_PACK.ItemId);
		} );

		buttonTalants_30.onClick.RemoveAllListeners();
		buttonTalants_30.onClick.AddListener (()=> {
			BuyProduct(BuyItems.THIRTY_TALENT_PACK.ItemId);
		} );

		buttonTalants_50.onClick.RemoveAllListeners();
		buttonTalants_50.onClick.AddListener (()=> {
			BuyProduct(BuyItems.FIFTY_TALENT_PACK.ItemId);
		} );

		buttonTalants_100.onClick.RemoveAllListeners();
		buttonTalants_100.onClick.AddListener (()=> {
			BuyProduct(BuyItems.HUNDRED_TALENT_PACK.ItemId);
		} );

		string price = "";
		foreach (var itemFromStore in StoreInfo.CurrencyPacks) {

			price = ((PurchaseWithMarket)itemFromStore.PurchaseType).MarketItem.MarketPriceAndCurrency;
			if (string.IsNullOrEmpty(price)) {
				price = ((PurchaseWithMarket)itemFromStore.PurchaseType).MarketItem.Price.ToString("0.00");
			}

			if (itemFromStore.ItemId == BuyItems.TEN_TALENT_PACK.ItemId) {
				textTalants_10.text = price;
			} else if (itemFromStore.ItemId == BuyItems.THIRTY_TALENT_PACK.ItemId) {
				textTalants_30.text = price;
			}else if (itemFromStore.ItemId == BuyItems.FIFTY_TALENT_PACK.ItemId) {
				textTalants_50.text = price;
			}else if (itemFromStore.ItemId == BuyItems.HUNDRED_TALENT_PACK.ItemId) {
				textTalants_100.text = price;
			}
		}

	}

	public void BuyProduct(string itemId){

		try{
			
			StoreInventory.BuyItem (itemId);
			StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;
			StoreEvents.OnItemPurchased += onMarketPurchase;
			StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;

		}catch(System.Exception ex){
			Debug.Log ("SOOMLA BUY ERROR " + ex.Message);
		}

	}

	public void onMarketPurchaseCancelled(PurchasableVirtualItem pvi) {

		StoreEvents.OnMarketPurchaseCancelled -= onMarketPurchaseCancelled;
		StoreEvents.OnItemPurchased -= onMarketPurchase;
		StoreEvents.OnUnexpectedStoreError -= onUnexpectedStoreError;

		ShowErrorDialog ("Покупка отменена.", ErrorDialogReaction);
	}

	public void onMarketPurchase(PurchasableVirtualItem pvi, string payload) {

		StoreEvents.OnMarketPurchaseCancelled -= onMarketPurchaseCancelled;
		StoreEvents.OnItemPurchased -= onMarketPurchase;
		StoreEvents.OnUnexpectedStoreError -= onUnexpectedStoreError;

		ShowErrorDialog ("Успешная покупка игровой валюты.", ErrorDialogReaction);
	}

	public void onUnexpectedStoreError(int errorCode) {

		StoreEvents.OnMarketPurchaseCancelled -= onMarketPurchaseCancelled;
		StoreEvents.OnItemPurchased -= onMarketPurchase;
		StoreEvents.OnUnexpectedStoreError -= onUnexpectedStoreError;
		ShowErrorDialog ("Ошибка при покупке.", ErrorDialogReaction);
		SoomlaUtils.LogError ("ExampleEventHandler", "error with code: " + errorCode);
	}

	private void ErrorDialogReaction(){
		internalButton.onClick.Invoke ();
	}
}
