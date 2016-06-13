using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;


public abstract class BaseDialog : MonoBehaviour {

	public GameObject 	mPanelObject;
	public Button 		internalButton;

	public ErrorPanel errorPanel;

	public void ShowDialog(UnityAction buttonEvent){
		mPanelObject.SetActive (true);

		InitDialog ();

		internalButton.onClick.RemoveAllListeners();
		internalButton.onClick.AddListener (buttonEvent);
		internalButton.onClick.AddListener (ClosePanel);
		internalButton.gameObject.SetActive (true);

	}

	protected abstract void InitDialog ();

	public void ClosePanel () {
		mPanelObject.SetActive (false);
	}

	public ErrorPanel ShowErrorDialog(string message, UnityAction clickButtonEvent){

		ErrorPanel NewErrorPanel = Instantiate(errorPanel) as ErrorPanel;

		NewErrorPanel.SetText(message, clickButtonEvent);

		NewErrorPanel.transform.SetParent(gameObject.transform);
		NewErrorPanel.transform.localScale = new Vector3(1,1,1);

		RectTransform rctr = NewErrorPanel.GetComponent<RectTransform>();
		rctr.offsetMax = new Vector2(0,0);
		rctr.offsetMin = new Vector2(0,0);
		rctr.anchoredPosition3D = new Vector3(0,0,0);

		return NewErrorPanel;
	}
}
