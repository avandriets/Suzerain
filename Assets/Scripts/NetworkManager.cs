using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	[SerializeField] public ServerSettings advancedSettings;
	
	[System.Serializable]
	public class ServerSettings
	{
		public string gameName = "SUNKExample";		// When RefreshHostList() is called, only servers with this exact string will be displayed CTRL-F GNINFO FOR IMPORTANT INFO
		public string serverName = "Example name";	// This is the name that will be displayed when returned by RefreshHostList()
		public string serverPort = "25000";			// This is the port you'd like to use; useful to know/change if you need to port forward
		public string username = "";				// What you want your username to be
		public string password = "";				// The password for the server; needed to join a passworded server, or to create a server
		public string maxPlayers = "4";				// Maximum number of players allowed in the server; CTRL-F MPINFO FOR IMPORTANT INFO
		public bool passwordProtected = false;		// If true, require a password on server join
		public bool privateServer = false;			// If this is true, then the server will become password-protected, and only those with the password can join
		public bool dedicated = false;				// If true, the server host won't spawn as a player, but will still be joinable. A separate instance is required for the server host to join and play
		public bool showAdminMenu = false;			// If dedicated is true, this will be shown on screen; customize it to display appropriate admin functions
	}

	HostData[] hostList;							// This is where the list of servers will be stored later
	[SerializeField] private GameObject playerObject;	
  [SerializeField] Text errorText;
  [SerializeField] Text infoText;
  [SerializeField] Transform serverPosition = null;
  [SerializeField] Transform clientPosition = null;
  [SerializeField] GameObject serverPanel = null;
  [HideInInspector] public bool HasInternet = false;
  [SerializeField] Text testMessage;
  [HideInInspector] public bool IsServer = false;
	string testMessageText = "Test in progress.";
	string shouldEnableNatMessage = "";
	public bool doneTesting = false;
	bool probingPublicIP = false;
	ConnectionTesterStatus connectionTestResult = ConnectionTesterStatus.Undetermined;
	bool useNat = false;
  private bool offlineMode = false;
	//===========

	void Awake()
	{
    advancedSettings.username = Random.value.ToString("f6");
    advancedSettings.password = Random.value.ToString("f6");    
		MasterServer.ClearHostList();
		testMessage.text = "Testing network connection capabilities.";      
  }

  void Start()
  {
    HasInternet = Application.internetReachability != NetworkReachability.NotReachable;
    Debug.LogWarning("HasInternet = " + HasInternet);

    if (!HasInternet)
    {
      PlayOffline();
      infoText.text = "Offline";
    }
    else
    {
      MasterServer.ClearHostList();
      hostList = MasterServer.PollHostList();
      RefreshHostList();
    }
  }

  private void StartAsServerOrClient()
  {
    if (hostList.Length > 0)
    {
      if (hostList[hostList.Length - 1].connectedPlayers < 2 && hostList[hostList.Length - 1].playerLimit > 1)
      {
        advancedSettings.serverPort = (int.Parse(advancedSettings.serverPort) + hostList.Length - 1).ToString("f0");
        JoinServer(hostList.Length - 1);
        infoText.text = "Client " + (hostList.Length - 1).ToString();
      }
      else
      {
        advancedSettings.serverPort = (int.Parse(advancedSettings.serverPort) + hostList.Length).ToString("f0");
        StartServer();
        infoText.text = "Server " + (hostList.Length).ToString();
      }
    }
    else
    {
      StartServer();
      infoText.text = "Server " + (hostList.Length).ToString();
    }        
  }

  void Update ()
	{
		testMessage.text = testMessageText;
		if (!doneTesting && HasInternet)
		{
			TestConnection();
    }    
  }

	public void StartServer()
  {
		// This is where we start creating the server
		
		// All these 'else if' statements are just to warn the player that a field is incorrectly filled
		// Once all fields are appropriately filled, then the server will be created
		if (advancedSettings.serverPort == "" || int.Parse(advancedSettings.serverPort) <= 0 || int.Parse(advancedSettings.serverPort) > 65535)
		{
			errorText.text = "Error: "+"Invalid Port Number\n"+errorText.text;
		}
		else if (advancedSettings.maxPlayers == "" || int.Parse(advancedSettings.maxPlayers) <= 0)
		{
			errorText.text = "Error: "+"Invalid Max Players Number\n"+errorText.text;
		}
		else if (advancedSettings.serverName == "")
		{
			errorText.text = "Error: "+"Invalid Server Name\n"+errorText.text;
		}
		else if (advancedSettings.username == "")
		{
			errorText.text = "Error: "+"Invalid Userame\n"+errorText.text;
		}
		else if (advancedSettings.passwordProtected && advancedSettings.password == "")
		{
			errorText.text = "Error: "+"Invalid Password\n"+errorText.text;
		}
		else
    {
			Debug.Log("U: " + advancedSettings.username + ", SN: " + advancedSettings.serverName);
			if (advancedSettings.password != "" && advancedSettings.passwordProtected)
			{
				Network.incomingPassword = advancedSettings.password;
				Network.InitializeServer(int.Parse(advancedSettings.maxPlayers) - 1, int.Parse(advancedSettings.serverPort), useNat); 	// we check whether useNat is needed later on automatically. Again, the documentation mentioned earlier will provide better explanation
				MasterServer.RegisterHost(advancedSettings.gameName, "[P]" + advancedSettings.serverName);								// Now we register the server, using the unique gameName assigned earlier, and the server name assigned by the player.
																																		// This will display "[P]" in front of the server if there is a password
			}
			else if (advancedSettings.password == "" || !advancedSettings.passwordProtected)
			{
				// If we haven't put in a password, or if passwordProtected is false, go ahead and create a public server
				Network.InitializeServer(int.Parse(advancedSettings.maxPlayers) - 1, int.Parse(advancedSettings.serverPort), useNat);
				MasterServer.RegisterHost(advancedSettings.gameName, advancedSettings.serverName);
			}
		}    
	}
	
	void OnServerInitialized()
  {		
		Debug.Log("Server Initialized");
		if (advancedSettings.dedicated)
		{
			advancedSettings.showAdminMenu = true;
		}
    else
    {
      IsServer = true;
      if (!offlineMode)
        SpawnPlayer();
		}
	}
	
	public void JoinServer (int serverNumber)
	{
		//Only join if we have a username
		if (advancedSettings.username != "")
		{
			if (hostList[serverNumber].passwordProtected)
			{
				Network.Connect(hostList[serverNumber], advancedSettings.password);
			}
			else
			{
				Network.Connect(hostList[serverNumber]);
			}			
		}
		else
		{
			errorText.text = "Invalid Username\n"+errorText.text;
		}
	}

  void StartDuel()
  {
    serverPanel.SetActive(false);
    CharacterBase[] characterBases = FindObjectsOfType<CharacterBase>();
    foreach (var _characterBase in characterBases)
    {
      _characterBase.StartDuel();
    }
  }
	
	void OnConnectedToServer()
  {
    Invoke("StartDuel", 0.1f);
    SpawnPlayer();
	}

  void OnPlayerConnected(NetworkPlayer player)
  {    
    Invoke("StartDuel", 0.1f);
  }

  void SpawnPlayer()
  {
    GameObject networkPlayer = null;
    if (IsServer)
      networkPlayer = Network.Instantiate(playerObject, serverPosition.position, serverPosition.rotation, 0) as GameObject;
    else
      networkPlayer = Network.Instantiate(playerObject, clientPosition.position, clientPosition.rotation, 0) as GameObject;
    networkPlayer.GetComponentInChildren<CharacterBase>().IsMine = true;
    networkPlayer.GetComponentInChildren<PlayerSync>().IsMine = true;
    Debug.LogWarning("Player was spawn");
	}  

  public void RefreshHostList()
	{		
		MasterServer.ClearHostList();
    try
    {
      if (HasInternet)
        MasterServer.RequestHostList(advancedSettings.gameName);
    }
    catch (UnityException)
    {
      Debug.Log("Internet is not available.");
    }		
	}	

	void OnMasterServerEvent(MasterServerEvent msEvent)
  {
		if(msEvent == MasterServerEvent.HostListReceived)
    {
      hostList = MasterServer.PollHostList();
      StartAsServerOrClient();
      Debug.LogWarning("HostListReceived");
		}    
	}

	void OnFailedToConnect (NetworkConnectionError error){
		// If we can't connect, tell the user why
		errorText.text = "Error: "+error.ToString()+"\n" +errorText.text;
    Debug.LogWarning(" OnFailedToConnect");
	}

  void OnPlayerDisconnected(NetworkPlayer player){
		// When a player disconnects, we clean up after the player by doing the following
		Debug.Log("Cleaning up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	void OnDisconnectedFromServer (NetworkDisconnection info){
		// In here, we can check why we've been disconnected, and do things accordingly.
		// In this case, we check why we DC'd, then reload the level to get back to the menu
		if(info == NetworkDisconnection.Disconnected){errorText.text = "Disconnected\n"+errorText.text;}
		else if(info == NetworkDisconnection.LostConnection){errorText.text = "LostConnection\n"+errorText.text;}
		Application.LoadLevel(1);
	}

	public void Disconnect()
	{
		Network.Disconnect();
	}
	public void Username(string sUser)
	{
		advancedSettings.username = sUser;
	}

	public void Password(string sPass)
	{
		advancedSettings.password = sPass;
	}
	public void ServerName (string sName)
	{
		advancedSettings.serverName = sName;
	}
	public void ServerPort (string sPort)
	{
		advancedSettings.serverPort = sPort;
	}
	public void Dedicated (bool sDedi)
	{
		advancedSettings.dedicated = sDedi;
	}

	public void PasswordProtected(bool sPassProt)
	{
		advancedSettings.passwordProtected = sPassProt;
	}

	public void MaxPlayers(string sMax)
	{
		advancedSettings.maxPlayers = sMax;
	}

	void TestConnection ()
  {
		float timer = Time.time;
		// Start/Poll the connection test, report the results in a label and 
		// react to the results accordingly
		connectionTestResult = Network.TestConnection();
		switch (connectionTestResult)
    {
		case ConnectionTesterStatus.Error: 
			testMessageText = "Problem determining NAT capabilities";
			doneTesting = true;
			break;
			
		case ConnectionTesterStatus.Undetermined: 
			testMessageText = "Undetermined NAT capabilities";
			doneTesting = false;
			break;
			
		case ConnectionTesterStatus.PublicIPIsConnectable:
			testMessageText = "Directly connectable public IP address.";
			useNat = false;
			doneTesting = true;
			break;
			
			// This case is a bit special as we now need to check if we can 
			// circumvent the blocking by using NAT punchthrough
		case ConnectionTesterStatus.PublicIPPortBlocked:
			testMessageText = "Non-connectable public IP address (port " + advancedSettings.serverPort + " blocked), running a server is impossible.";
			useNat = false;
			// If no NAT punchthrough test has been performed on this public 
			// IP, force a test
			if (!probingPublicIP) {
				connectionTestResult = Network.TestConnectionNAT();
				probingPublicIP = true;
				testMessage.text = "Testing if blocked public IP can be circumvented";
				timer = Time.time + 10;
			}
			// NAT punchthrough test was performed but we still get blocked
			else if (Time.time > timer) {
				probingPublicIP = false; 		// reset
				useNat = true;
				doneTesting = true;
			}
			break;
		case ConnectionTesterStatus.PublicIPNoServerStarted:
			testMessageText = "Public IP address but server not initialized, "+
				"it must be started to check server accessibility. Restart "+
					"connection test when ready.";
			break;
			
		case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
			testMessageText = "Limited NAT punchthrough capabilities. Cannot "+
				"connect to all types of NAT servers. Running a server "+
					"is ill advised as not everyone can connect.";
			useNat = true;
			doneTesting = true;
			break;
			
		case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
			testMessageText = "Limited NAT punchthrough capabilities. Cannot "+
				"connect to all types of NAT servers. Running a server "+
					"is ill advised as not everyone can connect.";
			useNat = true;
			doneTesting = true;
			break;
			
		case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
		case ConnectionTesterStatus.NATpunchthroughFullCone:
			testMessageText = "NAT punchthrough capable. Can connect to all "+
				"servers and receive connections from all clients. Enabling "+
					"NAT punchthrough functionality.";
			useNat = true;
			doneTesting = true;
			break;
			
		default: 
			testMessageText = "Error in test routine, got " + connectionTestResult;
			break;
		}
		if (doneTesting) {
			if (useNat)
				shouldEnableNatMessage = "When starting a server the NAT "+
					"punchthrough feature should be enabled (useNat parameter)";
			else
				shouldEnableNatMessage = "NAT punchthrough not needed";
			testMessage.text = shouldEnableNatMessage + "Done testing";
		}
	}

  public void PlayOffline()
  {
    offlineMode = true;
    GameObject networkPlayer = null;
    CharacterBase[] characterBases = FindObjectsOfType<CharacterBase>();
    if (characterBases.Length == 0)
    {
      networkPlayer = Instantiate(playerObject, serverPosition.position, serverPosition.rotation) as GameObject;
      networkPlayer.GetComponentInChildren<CharacterBase>().IsMine = true;
      networkPlayer.GetComponentInChildren<PlayerSync>().IsMine = true;
    }
    networkPlayer = Instantiate(playerObject, clientPosition.position, clientPosition.rotation) as GameObject;
    networkPlayer.GetComponentInChildren<CharacterBase>().Ai = true;
    StartDuel();
    Network.maxConnections = 0;
  }
}
