using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Purchasing;


public class BuySubscriptionDialog : BaseDialog {
	
	public Text month;
	public Text three_month;
	public Text six_month;
	public Text year;

	public Button buttonMonth;
	public Button buttonThreeMonth;
	public Button buttonSixMonth;
	public Button buttonYearMonth;

	TestPurch purchObject;

	public PremiumDialog premiumDlg;

	string currentProduct = "";

	protected override void InitDialog (){

		purchObject = TestPurch.instance;

		buttonMonth.onClick.RemoveAllListeners();
		buttonMonth.onClick.AddListener (()=> {
			BuyProduct(TestPurch.SUBSCRIPTION_MONTH);
		} );

		buttonThreeMonth.onClick.RemoveAllListeners();
		buttonThreeMonth.onClick.AddListener (()=> {
			BuyProduct(TestPurch.SUBSCRIPTION_3_MONTH);
		} );

		buttonSixMonth.onClick.RemoveAllListeners();
		buttonSixMonth.onClick.AddListener (()=> {
			BuyProduct(TestPurch.SUBSCRIPTION_6_MONTH);
		} );

		buttonYearMonth.onClick.RemoveAllListeners();
		buttonYearMonth.onClick.AddListener (()=> {
			BuyProduct(TestPurch.SUBSCRIPTION_YEAR_MONTH);
		} );

		if (TestPurch.m_StoreController != null) {

			for (int t = 0; t < TestPurch.m_StoreController.products.all.Length; t++)
			{
				var item = TestPurch.m_StoreController.products.all [t];

				if (item.definition.id == TestPurch.SUBSCRIPTION_MONTH ) {
					month.text = item.metadata.localizedPriceString;
				} else if (item.definition.id == TestPurch.SUBSCRIPTION_3_MONTH ) {
					three_month.text = item.metadata.localizedPriceString;
				} else if (item.definition.id == TestPurch.SUBSCRIPTION_6_MONTH ) {
					six_month.text = item.metadata.localizedPriceString;
				} else if (item.definition.id == TestPurch.SUBSCRIPTION_YEAR_MONTH ) {
					year.text = item.metadata.localizedPriceString;
				}
					
				var description = string.Format("{0} - {1}", item.definition.id, item.definition.type);
				Debug.Log (description);
			}
		}
	}

	private void BuyProduct(string prod_id){

		currentProduct = prod_id;

		premiumDlg.ShowDialog (buyEvent, cancelEvent);

	}

	private void buyEvent(){

		purchObject.failedPurch += OnFailedPurch;
		purchObject.successPurch += OnSuccessPurch;

		purchObject.BuyProductID (currentProduct);

	}

	private void cancelEvent(){

		currentProduct = "";

	}

	private void OnSuccessPurch(PurchaseEventArgs args){
		Debug.Log ("PURCH SUCCESS !!!");

		purchObject.failedPurch -= OnFailedPurch;
		purchObject.successPurch -= OnSuccessPurch;
	
		ShowErrorDialog ("Поздарвяем с приобритением подписки.", ErrorDialogReaction);
	}

	private void OnFailedPurch(Product product, PurchaseFailureReason failureReason){
		
		Debug.Log ("PURCH FAIL !!!");

		purchObject.failedPurch -= OnFailedPurch;
		purchObject.successPurch -= OnSuccessPurch;

		ShowErrorDialog ("Не удачная попытка приобритения подписки.", ErrorDialogReaction);
	}

	private void ErrorDialogReaction(){
		internalButton.onClick.Invoke ();
	}
}
