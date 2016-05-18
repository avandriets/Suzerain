using UnityEngine;
using System.Collections;

public class shieldDataObj {
	public int 		position;
	public int		startScore;
	public int		endScore;
	public string	shieldNumber;
	public string	description;

	public shieldDataObj(int pPosition, int	pStartScore, int pEndScore, string pShieldNumber, string pDescription){	
		position		= pPosition;
		startScore		= pStartScore;
		endScore		= pEndScore;
		shieldNumber	= pShieldNumber;
		description		= pDescription;
	}
}
