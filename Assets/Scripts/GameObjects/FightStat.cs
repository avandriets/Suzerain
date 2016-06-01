using UnityEngine;
using System.Collections;

public class FightStat
{
	public int UserId { get; set; }         //Идентификатор польхователя
	public int FightTypeId { get; set; }    //Тип боя. Если значение 0 - общий результат игрока
	public int Fights { get; set; }         //Количество боев.
	public int Wins { get; set; }           //Количество побед
	public int Draws { get; set; }          //Количество ничией
	public int RowWins { get; set; }        //количество побед, одержанных подряд в данном типе игры
	public int Score { get; set; }          //счет игрока
	public double Result { get; set; }      //процент побед к общему числу игр
	public int LocalStatus { get; set; }    //локальный статус (место в локальном рейтинге)
	public int GlobalStatus { get; set; }   //глобальный статус (место в глобальном рейтинге)
}
