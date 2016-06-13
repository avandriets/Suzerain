using UnityEngine;
using System.Collections;

public class Friend
{
	public int UserId { get; set; }         //Идентификатор пользователя
	public string  UserName { get; set; }   //Ник пользователя
	public double Result { get; set; }      //процент побед к общему числу игр
	public int Rank { get; set; }           //звание
	public int Score { get; set; }          //баллы
	public int State { get; set; }         // Служебное
	public double SQ { get; set; }          //SQ
}
