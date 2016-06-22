using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Soomla.Store;

public delegate void ChangeSubscriptionDelegate();

public class PurchaseVisualObject : MonoBehaviour {

	public Button premiumButton;
	public Button buyCurrency;

	public Text currencyAmount;
	public Text	subscription_desc;

	ScreensManager screensManager;

	private BuySubscriptionDialog 	buySuscriptionDlg;
	private BuyTalantsDialog 		buyTalantsDlg;

	public event ChangeSubscriptionDelegate		changeSubscriptionDelegate;

	[HideInInspector]
	public static bool hasSubscription = false;

	public Image HasSubscription, NoSubscription, HasMoney, NoMoney;

	void Start(){
		
		premiumButton.onClick.RemoveAllListeners();
		premiumButton.onClick.AddListener (OnPremiumClick);
		premiumButton.gameObject.SetActive (true);

		buyCurrency.onClick.RemoveAllListeners();
		buyCurrency.onClick.AddListener (OnBuyCurrencyClick);
		buyCurrency.gameObject.SetActive (true);

		if (SoomlaStore.Initialized) {
		
			StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;

			if(StoreInventory.GetItemBalance (StoreInfo.Currencies [0].ItemId) > 0){
				HasMoney.gameObject.SetActive (true);
				NoMoney.gameObject.SetActive (false);
				currencyAmount.text = StoreInventory.GetItemBalance (StoreInfo.Currencies [0].ItemId).ToString ();
			}else{
				HasMoney.gameObject.SetActive (false);
				NoMoney.gameObject.SetActive (true);
				currencyAmount.text = "";
			}
		}
	}

	public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded) {		

		if (SoomlaStore.Initialized) {
			
			if (StoreInventory.GetItemBalance (StoreInfo.Currencies [0].ItemId) > 0) {
				HasMoney.gameObject.SetActive (true);
				NoMoney.gameObject.SetActive (false);

				currencyAmount.text = StoreInventory.GetItemBalance (StoreInfo.Currencies [0].ItemId).ToString ();
			} else {
				HasMoney.gameObject.SetActive (false);
				NoMoney.gameObject.SetActive (true);

				currencyAmount.text = "";
			}

		}
	}

	void OnEnable (){
		screensManager	= ScreensManager.instance;
	}

	public void OnPremiumClick(){

		if (hasSubscription)
			return;
		
		buySuscriptionDlg = screensManager.GetBuySubscriptionDlg ();
		buySuscriptionDlg.ShowDialog (OnChangeSubscription);
	}

	public void OnBuyCurrencyClick(){	

		buyTalantsDlg = screensManager.GetBuyTalantsDlg ();
		buyTalantsDlg.ShowDialog (OnChangeSubscription);
	}

	public void initSubAndCurrency(){	

		if (TestPurch.m_StoreController != null) {

			string subsLast = "";

			for (int t = 0; t < TestPurch.m_StoreController.products.all.Length; t++)
			{

				var item = TestPurch.m_StoreController.products.all [t];

				if (item.definition.id == TestPurch.SUBSCRIPTION_MONTH && item.hasReceipt) {
					subsLast = "30d";
					hasSubscription = true;
				} else if (item.definition.id == TestPurch.SUBSCRIPTION_3_MONTH && item.hasReceipt) {
					subsLast = "90d";
					hasSubscription = true;
				} else if (item.definition.id == TestPurch.SUBSCRIPTION_6_MONTH && item.hasReceipt) {
					subsLast = "180d";
					hasSubscription = true;
				} else if (item.definition.id == TestPurch.SUBSCRIPTION_YEAR_MONTH && item.hasReceipt) {
					subsLast = "360d";
					hasSubscription = true;
				}else if (item.definition.id == TestPurch.noADSProductId && item.hasReceipt) {
					subsLast = "9999d";
					hasSubscription = true;
				}

				subscription_desc.text = subsLast;
			}

			if (hasSubscription) {
				HasSubscription.gameObject.SetActive (true);
				NoSubscription.gameObject.SetActive (false);
			} else {
				HasSubscription.gameObject.SetActive (false);
				NoSubscription.gameObject.SetActive (true);
			}

		}
	}

	public void OnChangeSubscription(){
	
		if (buySuscriptionDlg != null) {
			GameObject.Destroy (buySuscriptionDlg.gameObject);
			buySuscriptionDlg = null;
		}

		if (buyTalantsDlg != null) {
			GameObject.Destroy (buyTalantsDlg.gameObject);
			buyTalantsDlg = null;
		}

		initSubAndCurrency ();

		if (changeSubscriptionDelegate != null) {
			changeSubscriptionDelegate ();
		}
	}

}
