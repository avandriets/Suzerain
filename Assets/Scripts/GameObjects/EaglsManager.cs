using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EaglsManager {

	public static eaglsData[] eaglsArray = new eaglsData[]{
		new eaglsData(1, 50   ,59  ,"1", "Железный", 25), 
		new eaglsData(2, 60   ,69  ,"2", "Стальной", 50),
		new eaglsData(3, 70   ,79  ,"3", "Серебряный", 100),
		new eaglsData(4, 80   ,89  ,"4", "Золотой", 200),
		new eaglsData(5, 90   ,200  ,"5", "Кристальный", 500)
	};

	public static eaglsData getEagleNumByScore(double pScore){
	
		for (int i = 0; i < eaglsArray.Length; i++) {
			if (eaglsArray [i].startScore <= pScore && eaglsArray [i].endScore >= pScore) {
				return eaglsArray [i];
			}
		}

		return null;
	}

	public static eaglsData getEagl(List<FightStat> fightStat){
	
		foreach (var c in fightStat) {
			if (c.FightTypeId == 0) {
				return getEagleNumByScore (c.Result*100);
			}
		}

		return null;
	}
}
