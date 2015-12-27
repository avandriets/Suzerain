using UnityEngine;

	/// <summary>
	/// This script is attached to the player and it
	/// ensures that every players position, rotation, and scale,
	/// are kept up to date across the network.
	///
	/// This script is closely based on a script written by M2H,
	/// and edited by AnthonyB28 ( https://github.com/AnthonyB28/FPS_Unity/blob/master/MovementUpdate.cs )
	/// </summary>
    
public class PlayerSync : MonoBehaviour
{
	Vector3 lastPosition;
	Quaternion lastRotation;
  Vector3 lastSpineRotation;
  Transform myTransform;
  NetworkView networkView;
  Vector3 targetSpineRotation;
	[SerializeField] float rotThreshold = 5f;
  [SerializeField] private CharacterBase characterBase = null;  
  public bool IsMine = false;

  void Start ()
	{
    networkView = GetComponent<NetworkView>();
    myTransform = transform;
  }  
  	
	[RPC]
	void SetUsername (string name)
	{
		gameObject.name = name;
	}	
	
	void Update ()
	{
      if (IsMine)
	    {
		    SendMovement();
	    }
	    else
	    {
		    ApplyMovement();
	    }
	}
	
	void SendMovement()
	{
    if (Vector2.Angle(characterBase.SpineBoneJoystickAngle, lastSpineRotation) >= rotThreshold)
    {
      lastSpineRotation = characterBase.SpineBoneJoystickAngle;
      networkView.RPC("UpdateMovement", RPCMode.OthersBuffered, myTransform.position, myTransform.rotation, characterBase.SpineBoneJoystickAngle);      
    }
  }
	
	void ApplyMovement ()
	{
		characterBase.SpineBoneNetworkAngle = Vector3.Lerp(characterBase.SpineBoneJoystickAngle, targetSpineRotation, 0.5f);
  }
	
	[RPC]
	void UpdateMovement (Vector3 newPosition, Quaternion newRotation, Vector3 newSpineRotation)
	{
		targetSpineRotation = newSpineRotation;    
	}

  public void TryNetworkShoot(bool hasTarget, bool toHead)
  {
    networkView.RPC("NetworkShoot", RPCMode.OthersBuffered, hasTarget, toHead);
  }

  [RPC]
  private void NetworkShoot(bool hasTarget, bool toHead)
  {
    characterBase.NetworkTryShoot(hasTarget, toHead);
  }
}