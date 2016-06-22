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
		return new VirtualCurrency[]{TALENT_CURRENCY};
	}

	public VirtualGood[] GetGoods() {
		return new VirtualGood[] {};
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] {TEN_TALENT_PACK, THIRTY_TALENT_PACK, FIFTY_TALENT_PACK, HUNDRED_TALENT_PACK};
	}

	public VirtualCategory[] GetCategories() {
		
		return new VirtualCategory[]{};
	}
		
	public const string TALENT_CURRENCY_ITEM_ID      		= "currency_talent";



	//public static string NO_ADS_PRODUCT_ID = "android.test.purchased";
	//public static string NO_ADS_PRODUCT_ID = "no_ads_product";

	public const string TEN_TALENT_PACK_PRODUCT_ID      	= "10_talents";
	public const string THIRTY_TALENT_PACK_PRODUCT_ID      	= "30_talents";
	public const string FIFTY_TALENT_PACK_PRODUCT_ID      	= "50_talents";
	public const string HUNDRED_TALENT_PACK_PRODUCT_ID      = "100_talents";

	/** Virtual Currencies **/

	public static VirtualCurrency TALENT_CURRENCY = new VirtualCurrency(
	"Talents",										// name
	"",												// description
	TALENT_CURRENCY_ITEM_ID							// item id
	);

	/** Virtual Currency Packs **/

	public static VirtualCurrencyPack TEN_TALENT_PACK = new VirtualCurrencyPack(
	"50 Talents",                                   // name
	"",                       // description
	TEN_TALENT_PACK_PRODUCT_ID,                                   // item id
	50,												// number of currencies in the pack
	TALENT_CURRENCY_ITEM_ID,                        // the currency associated with this pack
	new PurchaseWithMarket(TEN_TALENT_PACK_PRODUCT_ID, 0.99)
	);

	public static VirtualCurrencyPack THIRTY_TALENT_PACK = new VirtualCurrencyPack(
		"100 Talents",                                   // name
	"",                       // description
	THIRTY_TALENT_PACK_PRODUCT_ID,                                   // item id
	100,												// number of currencies in the pack
	TALENT_CURRENCY_ITEM_ID,                        // the currency associated with this pack
	new PurchaseWithMarket(THIRTY_TALENT_PACK_PRODUCT_ID, 0.99)
	);

	public static VirtualCurrencyPack FIFTY_TALENT_PACK = new VirtualCurrencyPack(
		"300 Talents",                                   // name
	"",                       // description
	FIFTY_TALENT_PACK_PRODUCT_ID,                                   // item id
	300,												// number of currencies in the pack
	TALENT_CURRENCY_ITEM_ID,                        // the currency associated with this pack
	new PurchaseWithMarket(FIFTY_TALENT_PACK_PRODUCT_ID, 0.99)
	);

	public static VirtualCurrencyPack HUNDRED_TALENT_PACK = new VirtualCurrencyPack(
		"500 Talents",                                   // name
	"",                       // description
	HUNDRED_TALENT_PACK_PRODUCT_ID,                                   // item id
	500,												// number of currencies in the pack
	TALENT_CURRENCY_ITEM_ID,                        // the currency associated with this pack
	new PurchaseWithMarket(HUNDRED_TALENT_PACK_PRODUCT_ID, 0.99)
	);


//	public static VirtualGood NO_ADS_NONCONS = new LifetimeVG(
//		"No Ads", 											// name
//		"No More Ads!",				 						// description
//		"remove_adds2",										// item id
//		new PurchaseWithMarket(BuyItems.NO_ADS_PRODUCT_ID, 2.99));

}
