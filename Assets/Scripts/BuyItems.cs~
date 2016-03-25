using UnityEngine;
using System.Collections;
using Soomla.Store;

public class BuyItems : IStoreAssets {

	public int GetVersion() {
		return 0;
	}

	// NOTE: Even if you have no use in one of these functions, you still need to
	// implement them all and just return an empty array.

	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{};
	}

	public VirtualGood[] GetGoods() {
		return new VirtualGood[] {NO_ADS_NONCONS};
	}

	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] {};
	}

	public VirtualCategory[] GetCategories() {
		
		return new VirtualCategory[]{};
	}
		
	//public static string NO_ADS_PRODUCT_ID = "android.test.purchased";
	#if UNITY_ANDROID
	public static string NO_ADS_PRODUCT_ID = "no_ads_product";
	#elif UNITY_IPHONE
	public static string NO_ADS_PRODUCT_ID = "com.gamecore.suzerain.no_ads_product";
	#else
	public static string NO_ADS_PRODUCT_ID = "com.gamecore.suzerain.no_ads_product";
	#endif

	public static VirtualGood NO_ADS_NONCONS = new LifetimeVG(
		"No Ads", 											// name
		"No More Ads!",				 						// description
		"remove_adds2",										// item id
		new PurchaseWithMarket(BuyItems.NO_ADS_PRODUCT_ID, 2.99));
}
