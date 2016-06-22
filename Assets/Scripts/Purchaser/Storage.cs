using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Store;

public class Storage {

	private static string LEVEL_KEY = "tomi_level";

	public static int GetMoneyBalance(){
		if (SoomlaStore.Initialized) {
			return StoreInventory.GetItemBalance (BuyItems.TALENT_CURRENCY_ITEM_ID);
		} else {
			return 0;
		}
	}
	public static void GiveMoney(int value){
		if (SoomlaStore.Initialized) {
			StoreInventory.GiveItem (BuyItems.TALENT_CURRENCY_ITEM_ID, value);
		}
	}

	public static void TakeMoney(int value){
		if (SoomlaStore.Initialized) {
			StoreInventory.TakeItem (BuyItems.TALENT_CURRENCY_ITEM_ID, value);
		}
	}

	public static void SetLevel(int value){
		KeyValueStorage.SetValue (LEVEL_KEY, value.ToString());
	}

	public static int GetLevel(){
		string key = KeyValueStorage.GetValue (LEVEL_KEY);
		if (key.Length == 0) {
			return -1;
		}
		return int.Parse (key);
	}
}
