using UnityEngine;
using System.Collections;

public class shieldDataObj {
	public int 		position;
	public int		startScore;
	public int		endScore;
	public string	shieldNumber;
	public string	description;
	public int reward;

	public shieldDataObj(int pPosition, int	pStartScore, int pEndScore, string pShieldNumber, string pDescription, int pReward){	
		position		= pPosition;
		startScore		= pStartScore;
		endScore		= pEndScore;
		shieldNumber	= pShieldNumber;
		description		= pDescription;
		reward = pReward;
	}
}
