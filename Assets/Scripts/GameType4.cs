using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameType4 : GameBase {

	int targetNumber = 0;

	public CustomNumber			targetNum;
	public CustomNumberTexFloat finalNumber;

	protected override void InitGameScreen (){
	
		inProgress		= true;
		Delay = 3;

		targetNumber = currentTask.TaskId;

		targetNum.SetNumber (targetNumber);
		finalNumber.SetDefaultColor();
		finalNumber.SetNumber (0);

		inProgress		= false;
	}

	public override void StartGame(){
		clock.StartTimer (onAnswer);
	}

	protected override int GetAnswer (){
		return 0;
	}

	protected override int GetScore (){
		int res = (int)(targetNumber*1000f - clock.stopTime4GameType4);
		return res;
	}

	protected override void OnAnswerExecute(){
		finalNumber.gameObject.SetActive (true);
		finalNumber.SetNumber (clock.finishTime - clock.stopTime);
	}

}
