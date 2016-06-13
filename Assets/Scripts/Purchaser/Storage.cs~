using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Store;

public class Storage {

	private static string LEVEL_KEY = "tomi_level";

	public static int GetMoneyBalance(){
		return StoreInventory.GetItemBalance (BuyItems.TALENT_CURRENCY_ITEM_ID);
	}
	public static void GiveMoney(int value){
		StoreInventory.GiveItem (BuyItems.TALENT_CURRENCY_ITEM_ID, value);
	}

	public static void TakeMoney(int value){
		StoreInventory.TakeItem (BuyItems.TALENT_CURRENCY_ITEM_ID, value);
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
