using UnityEngine;
using System.Collections;

public class eaglsData {

	public int 		position;
	public int		startScore;
	public int		endScore;
	public string	eagleNumber;
	public string	description;
	public int		reward;

	public eaglsData(int pPosition, int	pStartScore, int pEndScore, string pEagleNumber, string pDescription, int pReward){	
		position		= pPosition;
		startScore		= pStartScore;
		endScore		= pEndScore;
		eagleNumber		= pEagleNumber;
		description		= pDescription;
		reward = pReward;
	}
}
