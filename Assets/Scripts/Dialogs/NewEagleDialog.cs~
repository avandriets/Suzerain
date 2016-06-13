using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;


public class NewEagleDialog : MonoBehaviour {

	public GameObject 	mPanelObject;
	public Button 		okButton;

	public Image 		eagle;
	public Text 		eagleDescription;


	public void InitDialog(User user, List<FightStat> fightStat, UnityAction okEvent){
		mPanelObject.SetActive (true);

		//"Вы побеждаете в более чем {0}% поединков. Вы награждаетесь {1} Орлом Интеллектуальной лиги. Вы получаете {} талантов).
		eagleDescription.text 	= System.Text.RegularExpressions.Regex.Unescape(Utility.getEagleDescription(user ,fightStat));

		Utility.setEagle (eagle, fightStat);
		//rankText.text = Utility.GetDifference (null, fightStat);

		okButton.onClick.RemoveAllListeners();
		okButton.onClick.AddListener (okEvent);
		okButton.onClick.AddListener (ClosePanel);		
		okButton.gameObject.SetActive (true);
	}

	public void ClosePanel () {
		mPanelObject.SetActive (false);
	}
}
