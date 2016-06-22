using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Purchasing;
using UnityEngine.UI;


public delegate void FailedPurchHandler(Product product, PurchaseFailureReason failureReason);
public delegate void SuccessPurchHandler(PurchaseEventArgs args);

public class TestPurch : MonoBehaviour, IStoreListener {

	public event FailedPurchHandler failedPurch;
	public event SuccessPurchHandler successPurch;

	private static TestPurch s_Instance = null;

	private void OnSuccessPurch(PurchaseEventArgs args){
		
		if (successPurch != null)
			successPurch(args);
	}

	private void OnFailedPurch(Product product, PurchaseFailureReason failureReason){

		if (failedPurch != null)
			failedPurch(product, failureReason);
	}

	public static TestPurch instance {
		get {
			if (s_Instance == null) {
				// This is where the magic happens.
				//  FindObjectOfType(...) returns the first AManager object in the scene.
				s_Instance =  FindObjectOfType(typeof (TestPurch)) as TestPurch;
			}

			// If it is still null, create a new instance
			if (s_Instance == null) {
				GameObject obj = new GameObject("PurchManager");
				s_Instance = obj.AddComponent(typeof (TestPurch)) as TestPurch;
				Debug.Log ("Could not locate an AManager object. \n ScreensManager was Generated Automaticly.");
			}

			return s_Instance;
		}
	}

	[HideInInspector]
	public static IStoreController m_StoreController;          // The Unity Purchasing system.
	[HideInInspector]
	public static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.


	//public static string NO_ADS_PRODUCT_ID = "android.test.purchased";
	#if UNITY_ANDROID
		public static string noADSProductId = "no_ads_product";
		public static string SUBSCRIPTION_MONTH			= "monthly_subscription";
		public static string SUBSCRIPTION_3_MONTH		= "3_month_subscription";
		public static string SUBSCRIPTION_6_MONTH		= "6_month_subscription";
		public static string SUBSCRIPTION_YEAR_MONTH	= "year_subscription";
	#elif UNITY_IPHONE
		public static string noADSProductId = "com.gamecore.suzerain.no_ads_product";

		public static string SUBSCRIPTION_MONTH			= "com.gamecore.suzerain.monthly_subscription";
		public static string SUBSCRIPTION_3_MONTH		= "com.gamecore.suzerain.3_month_subscription";
		public static string SUBSCRIPTION_6_MONTH		= "com.gamecore.suzerain.6_month_subscription";
		public static string SUBSCRIPTION_YEAR_MONTH	= "com.gamecore.suzerain.year_subscription";
	#else
		public static string NO_ADS_PRODUCT_ID = "com.gamecore.suzerain.no_ads_product";
	#endif


	//public static string kProductIDSubscription =  "subscription";

	void OnEnable()
	{
		// If we haven't set up the Unity Purchasing reference
		if (m_StoreController == null)
		{
			// Begin to configure our connection to Purchasing
			InitializePurchasing();
		}
	}

	public void InitializePurchasing() 
	{
		// If we have already connected to Purchasing ...
		if (IsInitialized())
		{
			// ... we are done here.
			return;
		}

		// Create a builder, first passing in a suite of Unity provided stores.
		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		// Add a product to sell / restore by way of its identifier, associating the general identifier
		// with its store-specific identifiers.
		//builder.AddProduct(kProductIDConsumable, ProductType.Consumable);
		// Continue adding the non-consumable product.
		builder.AddProduct(noADSProductId, ProductType.NonConsumable);

		// And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
		// if the Product ID was configured differently between Apple and Google stores. Also note that
		// one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
		// must only be referenced here.

		#if UNITY_ANDROID
			builder.AddProduct(SUBSCRIPTION_MONTH, ProductType.Subscription,
			new IDs(){	
			{ SUBSCRIPTION_MONTH,		GooglePlay.Name },
			}
			);

			builder.AddProduct(SUBSCRIPTION_3_MONTH, ProductType.Subscription,
			new IDs(){	
			{ SUBSCRIPTION_3_MONTH, 	GooglePlay.Name },
			}
			);

			builder.AddProduct(SUBSCRIPTION_6_MONTH, ProductType.Subscription,
			new IDs(){	
			{ SUBSCRIPTION_6_MONTH, 	GooglePlay.Name },
			}
			);

			builder.AddProduct(SUBSCRIPTION_YEAR_MONTH, ProductType.Subscription,
			new IDs(){	
			{ SUBSCRIPTION_YEAR_MONTH, 	GooglePlay.Name },
			}
			);
		#elif UNITY_IPHONE
			builder.AddProduct(SUBSCRIPTION_MONTH, ProductType.Subscription,
			new IDs(){	
				{ SUBSCRIPTION_MONTH,		AppleAppStore.Name },
			}
			);

			builder.AddProduct(SUBSCRIPTION_3_MONTH, ProductType.Subscription,
			new IDs(){	
				{ SUBSCRIPTION_3_MONTH, 	AppleAppStore.Name },
			}
			);

			builder.AddProduct(SUBSCRIPTION_6_MONTH, ProductType.Subscription,
			new IDs(){	
				{ SUBSCRIPTION_6_MONTH, 	AppleAppStore.Name },
			}
			);

			builder.AddProduct(SUBSCRIPTION_YEAR_MONTH, ProductType.Subscription,
			new IDs(){	
				{ SUBSCRIPTION_YEAR_MONTH, 	AppleAppStore.Name },
			}
			);
		#else

			builder.AddProduct(SUBSCRIPTION_MONTH, ProductType.Subscription,
			new IDs(){	
			{ SUBSCRIPTION_MONTH,		GooglePlay.Name },
			}
			);

			builder.AddProduct(SUBSCRIPTION_3_MONTH, ProductType.Subscription,
			new IDs(){	
			{ SUBSCRIPTION_3_MONTH, 	GooglePlay.Name },
			}
			);

			builder.AddProduct(SUBSCRIPTION_6_MONTH, ProductType.Subscription,
			new IDs(){	
			{ SUBSCRIPTION_6_MONTH, 	GooglePlay.Name },
			}
			);

			builder.AddProduct(SUBSCRIPTION_YEAR_MONTH, ProductType.Subscription,
			new IDs(){	
			{ SUBSCRIPTION_YEAR_MONTH, 	GooglePlay.Name },
			}
			);

		#endif


		// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
		// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
		UnityPurchasing.Initialize(this, builder);
	}

	// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
	// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
	public void RestorePurchases(PurchRestoreDelegare pDelegate)
	{
		// If Purchasing has not yet been set up ...
		if (!IsInitialized())
		{
			// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
			Debug.Log("RestorePurchases FAIL. Not initialized.");
			return;
		}

		// If we are running on an Apple device ... 
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			// ... begin restoring purchases
			Debug.Log("RestorePurchases started ...");

			// Fetch the Apple store-specific subsystem.
			var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			// Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
			// the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
			apple.RestoreTransactions((result) => {
			// The first phase of restoration. If no more responses are received on ProcessPurchase then 
			// no purchases are available to be restored.
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				pDelegate(result);
			});
		}
		// Otherwise ...
		else
		{
			pDelegate(false);
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}


	private bool IsInitialized()
	{
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{

		// Purchasing has succeeded initializing. Collect our Purchasing references.
		Debug.Log("OnInitialized: PASS");

		// Overall Purchasing system, configured with products for this application.
		m_StoreController = controller;
		// Store specific subsystem, for accessing device-specific store features.
		m_StoreExtensionProvider = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("Billing failed to initialize!");
		switch (error)
		{
		case InitializationFailureReason.AppNotKnown:
			Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
			break;
		case InitializationFailureReason.PurchasingUnavailable:
			// Ask the user if billing is disabled in device settings.
			Debug.Log("Billing disabled!");
			break;
		case InitializationFailureReason.NoProductsAvailable:
			// Developer configuration error; check product metadata.
			Debug.Log("No products available for purchase!");
			break;
		}
	}


	public void BuyProductID(string productId)
	{
		// If Purchasing has been initialized ...
		if (IsInitialized())
		{
			// ... look up the Product reference with the general product identifier and the Purchasing 
			// system's products collection.
			Product product = m_StoreController.products.WithID(productId);

			// If the look up found a product for this device's store and that product is ready to be sold ... 
			if (product != null && product.availableToPurchase)
			{
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
				// asynchronously.
				m_StoreController.InitiatePurchase(product);
			}
			// Otherwise ...
			else
			{
				// ... report the product look-up failure situation  
				Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		// Otherwise ...
		else
		{
			// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
			// retrying initiailization.
			Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
		// this reason with the user to guide their troubleshooting actions.
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

		//OnPurchaseFailed (product, failureReason);

		if (failedPurch != null)
			failedPurch(product, failureReason);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
	{

		// A consumable product has been purchased by this user.
		if (String.Equals(args.purchasedProduct.definition.id, SUBSCRIPTION_MONTH, StringComparison.Ordinal))
		{
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
		}
		else if (String.Equals(args.purchasedProduct.definition.id, SUBSCRIPTION_3_MONTH, StringComparison.Ordinal))
		{
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
		}
		else if (String.Equals(args.purchasedProduct.definition.id, SUBSCRIPTION_6_MONTH, StringComparison.Ordinal))
		{
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
		}
		else if (String.Equals(args.purchasedProduct.definition.id, SUBSCRIPTION_YEAR_MONTH, StringComparison.Ordinal))
		{
			Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
		}
		else 
		{
			Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
		}

		OnSuccessPurch (args);

		return PurchaseProcessingResult.Complete;
	}

	public static bool hasSubscriptionRequest(){

		bool hasSubscription = false;

		if (TestPurch.m_StoreController != null) {
			for (int t = 0; t < m_StoreController.products.all.Length; t++)
			{
				var item = m_StoreController.products.all [t];

				if (item.definition.id == SUBSCRIPTION_MONTH && item.hasReceipt) {
					hasSubscription = true;
				} else if (item.definition.id == SUBSCRIPTION_3_MONTH && item.hasReceipt) {
					hasSubscription = true;
				} else if (item.definition.id == SUBSCRIPTION_6_MONTH && item.hasReceipt) {
					hasSubscription = true;
				} else if (item.definition.id == SUBSCRIPTION_YEAR_MONTH && item.hasReceipt) {
					hasSubscription = true;
				}else if (item.definition.id == noADSProductId && item.hasReceipt) {
					hasSubscription = true;
				}
			}
		}

		return hasSubscription;
	}
}
