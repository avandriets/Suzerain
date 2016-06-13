using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Rose : MonoBehaviour {

	public List<RectTransform> RelationsTransformAnd;
	public List<Text> TextList;

	public static List<FightStat> statList = new List<FightStat> ();

	//private bool inProcess = false;

	public void toZeroPosition(){

		foreach (var c in RelationsTransformAnd) {
			c.transform.localScale = new Vector3(0,0,1);
		}

	}

	public void setDataToRose(List<FightStat> pStatList){	

		if (pStatList != null) {
			Rose.statList = pStatList;
		} else {
			List<FightStat> statList = new List<FightStat> ();

			for (int i = 0; i <= 7; i++) {
				FightStat fst = new FightStat ();
				fst.FightTypeId = i;
				fst.Result = Random.Range (0.0F, 100.0F);
				statList.Add (fst);
			}

			Rose.statList = statList;		
		}
	}

	public void ShowData(){

		//if (inProcess)
		//	return;

		float counter = 0;
		for (int i = 0; i < RelationsTransformAnd.Count; i++) {
			foreach (var c in statList) {
				if (c.FightTypeId == (i + 1) ) {
					//counter = 0.53F * ((float)c.Result / 100);
					counter = 0.53F * ((float)c.Result);
					//RelationsTransformAnd[i].transform.localScale = new Vector3(0.47F + counter,0.47F + counter,1); 
					TextList [i].text = ((int)(c.Result*100)).ToString () + " %";
					StartCoroutine(ShowSector(RelationsTransformAnd[i], 0.47F + counter));
				}
			}
		}
	}

	IEnumerator ShowSector(RectTransform sector, float maxValue) {

			float startingTime = 0.47F;

			while (startingTime <= maxValue) {

				sector.transform.localScale = new Vector3 (startingTime, startingTime, 1); 
				startingTime += 0.005F; 

				yield return null;
			}
	}
}
