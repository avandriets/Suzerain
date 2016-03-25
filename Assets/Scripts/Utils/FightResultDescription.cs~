using UnityEngine;
using System.Collections;

public class FightResultDescription {

	public static int getMyAnswer(Fight currentFight){

		if (currentFight.InitiatorId == UserController.currentUser.Id) {
			return currentFight.RealInitiatorAnswer;
		} else {
			return currentFight.RealOpponentAnswer;
		}
	}

	public static int getOponentAnswer(Fight currentFight){

		if (currentFight.InitiatorId == UserController.currentUser.Id) {
			return currentFight.RealOpponentAnswer;
		} else {
			return currentFight.RealInitiatorAnswer;
		}
	}

	public static int getMyScore(Fight currentFight){

		if (currentFight.InitiatorId == UserController.currentUser.Id) {
			return currentFight.InitiatorScore;
		} else {
			return currentFight.OpponentScore;
		}
	}

	public static int getOponentScore(Fight currentFight){

		if (currentFight.InitiatorId == UserController.currentUser.Id) {
			return currentFight.OpponentScore;
		} else {
			return currentFight.InitiatorScore;
		}
	}

	public static string getExtendetWinDescription(Fight currentFight){

		// Answers & Score
		if (currentFight.FightTypeId == 1 || currentFight.FightTypeId == 2 || currentFight.FightTypeId == 3 ) {
			if (FightResultDescription.getOponentAnswer (currentFight) < 0 ) {
				switch (currentFight.FightTypeId) {
				case 1:
					return "Противник ответил неверно";
				case 2:
					return "Противник ответил неверно";
				case 3:
					return "Противник ответил неверно";
				}
			} else {
				switch (currentFight.FightTypeId) {
				case 1:
					return "Вы ответили быстрее";
				case 2:
					return "Вы ответили быстрее";
				case 3:
					return "Вы ответили быстрее";
				}
			}
		}// Score
		else if(currentFight.FightTypeId == 4){
			return "Вы были точнее";
//			if (FightResultDescription.getOponentScore (currentFight) > FightResultDescription.getMyScore (currentFight)) {
//				return "Вы были точнее";
//			} else {
//				return "Противник был точнее";
//			}
		}//Answers
		else if(currentFight.FightTypeId == 5 || currentFight.FightTypeId == 7 || currentFight.FightTypeId == 6){

			switch (currentFight.FightTypeId) {
				case 5:
				return "Вы были более удачливы";
				case 6:
				return "Ваша память оказалась крепче";
				case 7:
				return "Вы были более проницательны";
			}
		}
		return "";
	}

	public static string getExtendetLoseDescription(Fight currentFight){

		// Answers & Score
		if (currentFight.FightTypeId == 1 || currentFight.FightTypeId == 2 || currentFight.FightTypeId == 3 ) {
			if (FightResultDescription.getMyAnswer (currentFight) < 0 ) {
				switch (currentFight.FightTypeId) {
					case 1:
						return "Вы ответили неверно";
					case 2:
						return "Вы ответили неверно";
					case 3:
						return "Вы ответили неверно";
				}
			} else {
				switch (currentFight.FightTypeId) {
					case 1:
						return "Противник ответил быстрее";
					case 2:
						return "Противник ответил быстрее";
					case 3:
						return "Противник ответил быстрее";
				}
			}
		}// Score
		else if(currentFight.FightTypeId == 4){

			return "Противник был точнее";

//			if (FightResultDescription.getOponentScore (currentFight) > FightResultDescription.getMyScore (currentFight)) {
//				return "Вы были точнее";
//			} else {
//				return "Противник был точнее";
//			}
		}//Answers
		else if(currentFight.FightTypeId == 5 || currentFight.FightTypeId == 6 || currentFight.FightTypeId == 7){

			switch (currentFight.FightTypeId) {
				case 5:
				return "Противник был более удачлив";
				case 6:
				return "Память противника оказалась крепче";
				case 7:
				return "Противник был более проницателен";
			}
		}

		return "";
	}

	public static string getExtendetDraftDescription(Fight currentFight){

		if (currentFight.FightTypeId == 1 || currentFight.FightTypeId == 2 || currentFight.FightTypeId == 3 ) {
			return "Ничья";
		}// Score
		else if(currentFight.FightTypeId == 4){
			string msg = "";//"счет (" + (((float)FightResultDescription.getMyScore (currentFight))/1000).ToString("##.##") + " - "+ (((float)FightResultDescription.getOponentScore (currentFight))/1000).ToString("##.##") + ")";
			return "Ничья \n" + msg;
		}//Answers
		else if(currentFight.FightTypeId == 5 || currentFight.FightTypeId == 6 || currentFight.FightTypeId == 7){
			string msg = "";//"счет (" + FightResultDescription.getMyAnswer (currentFight).ToString() + "-"+ FightResultDescription.getOponentAnswer (currentFight).ToString() + ")";

			return "Ничья \n" + msg;
		}

		return "";
	}

	public static string getScoreBothPlayers(Fight currentFight){

		//System.Math.Round(((double)FightResultDescription.getMyScore (currentFight))/1000,2, System.MidpointRounding.
		//string msg = string.Format(
		if (currentFight.FightTypeId == 4) {
			float resultMy = 0;
			float taskScore = (float)currentFight.TaskId;
			float MyScore 	= ((float)FightResultDescription.getMyScore (currentFight)) / 1000f;
			resultMy 			= taskScore + MyScore;

			float resultOpp = 0;
			float OpponentScore = ((float)FightResultDescription.getOponentScore (currentFight)) / 1000f;
			resultOpp = taskScore + OpponentScore;

			string msg = resultMy.ToString ("00.00") + " - " + resultOpp.ToString ("00.00");
			return msg;
		} else if (currentFight.FightTypeId == 7) {
			int MyScore = FightResultDescription.getMyAnswer (currentFight);
			int OpponentScore = FightResultDescription.getOponentAnswer (currentFight);

			if (MyScore < 0) {
				MyScore = 0;
			}

			if (OpponentScore < 0) {
				OpponentScore = 0;
			}

			string msg = MyScore.ToString() + " - "+ OpponentScore.ToString();
			return msg;
		}
		//Answers
		else if(currentFight.FightTypeId == 5 || currentFight.FightTypeId == 6){
			string msg = FightResultDescription.getMyAnswer (currentFight).ToString() + " - "+ FightResultDescription.getOponentAnswer (currentFight).ToString();
			return msg;
		}
		return "";
	}
}
