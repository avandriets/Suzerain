using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public class BaseUIClass : MonoBehaviour {


	public PurchaseVisualObject purchaseVisualObject;
	protected WaitPanel				waitPanel				= null;
	protected ErrorPanel			errorPanel				= null;
	protected WaitOpponentDialog 	waitForOpponentPanel	= null;
//	protected InstructionDialog 	instDialog 				= null;

	protected static int currentFightId = -1;

	protected ScreensManager	screensManager	= null;

	public AcceptFightWithFriend mAcceptFightDialogWithFriend;

	public void AskForFightFromFriend (int fightId)
	{

		currentFightId = fightId;
		OnlineGame ing = OnlineGame.instance;

		ing.InitGameParameters (true, true);

		StartCoroutine (ing.StateFightRequestWithFriendByID (fightId, CancelFightByServer, ReadyToFightWithFriend, ErrorFightRequest));
	}

	public void CancelFightByServer (Fight pfight)
	{

		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		OnlineGame ing = OnlineGame.instance;
		ing.currentFight = null;

		errorPanel = screensManager.ShowErrorDialog (ScreensManager.LMan.getString ("@fight_finished_by_server"), ErrorCancelByServer);
		SoundManager.ChoosePlayMusic (0);
	}

	public void ReadyToFightWithFriend ()
	{
		
		mAcceptFightDialogWithFriend = screensManager.CreateFightWithFriendDialog ();

		mAcceptFightDialogWithFriend.ShowDialog (AcceptFightWithFriend, CancelFightWithFriend);		
	}

	public void AcceptFightWithFriend ()
	{
		if (mAcceptFightDialogWithFriend != null) {
			mAcceptFightDialogWithFriend.ClosePanel ();
			GameObject.Destroy (mAcceptFightDialogWithFriend.gameObject);
			mAcceptFightDialogWithFriend = null;
		}

		waitPanel = screensManager.ShowWaitDialog (ScreensManager.LMan.getString ("@connecting"), false);
		OnlineGame ing = OnlineGame.instance;
		StartCoroutine (ing.AcceptFightWithFriendByID (currentFightId, CancelFightByServer, IntoFight, ErrorFightRequest));		
	}

	public void CancelFightWithFriend ()
	{

		if (mAcceptFightDialogWithFriend != null) {
			mAcceptFightDialogWithFriend.ClosePanel ();
			GameObject.Destroy (mAcceptFightDialogWithFriend.gameObject);
			mAcceptFightDialogWithFriend = null;
		}

		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		OnlineGame ing = OnlineGame.instance;
		ing.CancelFightWithFriend ();
	}

	public void ErrorFightRequest ()
	{
		if (waitForOpponentPanel != null) {
			waitForOpponentPanel.ClosePanel ();
			GameObject.Destroy (waitForOpponentPanel.gameObject);
			waitForOpponentPanel = null;
		}

		OnlineGame ing = OnlineGame.instance;
		ing.currentFight = null;

		errorPanel = screensManager.ShowErrorDialog (ScreensManager.LMan.getString ("@server_side_error"), ErrorCancelByServer);
		SoundManager.ChoosePlayMusic (0);
	}

	public void ErrorCancelByServer ()
	{
		GameObject.Destroy (errorPanel.gameObject);
		errorPanel = null;
		SoundManager.ChoosePlayMusic (0);
	}

	public void IntoFight ()
	{
		if (waitPanel != null)
			screensManager.CloseWaitPanel (waitPanel);

		screensManager.ShowGameScreen ();
	}


}
