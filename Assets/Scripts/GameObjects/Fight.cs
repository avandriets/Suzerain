using UnityEngine;
using System.Collections;

public class Fight
{
	public int Id { get; set; }
	public int InitiatorId { get; set; }
	public int FightState { get; set; }
	public int FightTypeId { get; set; }
	public int TaskId { get; set; }
	public int OpponentId { get; set; }
	public int InitiatorScore { get; set; }
	public int InitiatorAnswer { get; set; }
	public int OpponentScore { get; set; }
	public int OpponentAnswer { get; set; }
	public int Winner { get; set; }
	public int Looser { get; set; }
	public bool IsDraw { get; set; }
	public int RealInitiatorAnswer { get; set; }
	public int RealOpponentAnswer { get; set; }
}
