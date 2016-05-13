using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class MedalShowDialog : MonoBehaviour {

	public GameObject 	ShowMedalPanelObject;
	public Text			ShieldDescription;
	public Image		ShieldIcon;
	public Button 		okButton;

	//private ErrorPanel  errorPanel;

	public void InitDialog(User user, List<FightStat> fightStat, UnityAction okEvent){
		ShowMedalPanelObject.SetActive (true);

		ShieldDescription.text 	= System.Text.RegularExpressions.Regex.Unescape(Utility.getShieldDescription(user ,fightStat));

		Utility.setAvatar (ShieldIcon,  fightStat);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);		
		okButton.gameObject.SetActive (true);

	}

	public void ClosePanel () {
		ShowMedalPanelObject.SetActive (false);
	}

}