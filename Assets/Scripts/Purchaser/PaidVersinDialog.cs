using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using Soomla;
using Soomla.Store;

public class PaidVersinDialog : MonoBehaviour {

	public GameObject 	paidVersionPanelObject;
	public Button 		yesButton;
	public Button 		cancelButton;
	public Text			itemPrice;
	public Text			promoText;

	private PaidVersinDialog errorPanel;

	public void SetText(UnityAction yesEvent){
		paidVersionPanelObject.SetActive (true);

		yesButton.onClick.RemoveAllListeners();
		yesButton.onClick.AddListener (yesEvent);
		yesButton.onClick.AddListener (ClosePanel);
		yesButton.gameObject.SetActive (true);


		cancelButton.onClick.RemoveAllListeners();
		cancelButton.onClick.AddListener (ClosePanel);
		cancelButton.gameObject.SetActive (true);

		//set price
//		if (vg.PurchaseType is PurchaseWithVirtualItem) {
//			GUI.Label(new Rect(Screen.width/2f,y+productSize*2/3f,Screen.width,productSize/3f),"price:" + ((PurchaseWithVirtualItem)vg.PurchaseType).Amount);
//		}
//		else {
//			string price = ((PurchaseWithMarket)vg.PurchaseType).MarketItem.MarketPriceAndCurrency;
//			if (string.IsNullOrEmpty(price)) {
//				price = ((PurchaseWithMarket)vg.PurchaseType).MarketItem.Price.ToString("0.00");
//			}
//			GUI.Label(new Rect(Screen.width/2f,y+productSize*2/3f,Screen.width,productSize/3f),"price: " + price);
//		}
//		GUI.Label(new Rect(Screen.width*3/4f,y+productSize*2/3f,Screen.width,productSize/3f), "Balance:" + StoreInventory.GetItemBalance(vg.ItemId));
		string items = "";
		foreach (var itemFromStore in StoreInfo.Goods) {
			items += " " + itemFromStore.ItemId + " " + itemFromStore.Name + " \n";
		}

		VirtualGood vg = StoreInfo.Goods [0];

//		string message = "";
//		if (! vg.CanAfford()) {
//			message = "Cannot afford this item ";
//		}

		string price = "";
		if (vg.PurchaseType is PurchaseWithVirtualItem) {
			price = "price:" + ((PurchaseWithVirtualItem)vg.PurchaseType).Amount;
		}
		else {
			price = ((PurchaseWithMarket)vg.PurchaseType).MarketItem.MarketPriceAndCurrency;
			if (string.IsNullOrEmpty(price)) {
				price = ((PurchaseWithMarket)vg.PurchaseType).MarketItem.Price.ToString("0.00");
			}
			//GUI.Label(new Rect(Screen.width/2f,y+productSize*2/3f,Screen.width,productSize/3f),"price: " + price);
		}
		//string balance = "Balance:" + StoreInventory.GetItemBalance(vg.ItemId);
		//promoText.text = message + " \n " + price + " \n " + balance;
		//promoText.text = items;

		itemPrice.text = price;
	}

	public void ClosePanel () {
		paidVersionPanelObject.SetActive (false);
	}

}
