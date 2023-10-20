using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

namespace Imagication
{
	public class MobileLauncher : MonoBehaviourPunCallbacks
	{


		#region Private Serializable Fields

		[Tooltip("The Ui Panel to let the user enter name, connect and play")]
		[SerializeField]
		private GameObject controlPanel;

		[Tooltip("The UI Label to inform the user that the connection is in progress")]
		[SerializeField]
		private GameObject progressLabel;

		[Tooltip("The UI Label to inform the user forgot their name and cannot continue")]
		[SerializeField]
		private GameObject forgottenName;

		[Tooltip("The UI Label to inform the user did not enter a passwordand cannot continue")]
		[SerializeField]
		private GameObject forgottenPassword;

		[SerializeField]
		private GameObject Buttons;

		[Tooltip("The maximum number of players per room")]
		[SerializeField]
		private byte maxPlayersPerRoom = 10;

		private TouchScreenKeyboard keyboard;
		private string inputText = "";


		#endregion


		#region Private Fields
		/// <summary>
		/// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
		/// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
		/// Typically this is used for the OnConnectedToMaster() callback.
		/// </summary>
		bool isConnecting;

		/// <summary>
		/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
		/// </summary>
		string gameVersion = "1";

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during early initialization phase.
		/// </summary>
		void Awake()
		{
			// progressLabel.SetActive(false);
			// controlPanel.SetActive(true);
			// if (loaderAnime==null)
			// {
			// 	Debug.LogError("<Color=Red><b>Missing</b></Color> loaderAnime Reference.",this);
			// }

			// #Critical
			// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.AutomaticallySyncScene = true;

			// UserData.MyUserDataName = playerName.text;

		}


		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity during initialization phase.
		/// </summary>
		void Start()
		{
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
			forgottenName.SetActive(false);
			forgottenPassword.SetActive(false);
			Buttons.SetActive(true);

			// if (Application.platform == RuntimePlatform.WindowsPlayer)
			Debug.Log(Application.platform);

			if (!Application.isMobilePlatform)
			{
				Debug.Log("Do something special here");
			}
		}

		#endregion
#if UNITY_STANDALONE_WIN

		Debug.Log("Standalone Windows");

#endif
		private void Update()
		{
			// Check for a user interaction to open the keyboard (e.g., a button press)
			if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
			{
				Debug.Log("Test");
				TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
				// Open the touch screen keyboard
				keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
			}

			// Check if the keyboard is active and update the inputText accordingly
			if (keyboard != null && keyboard.active)
			{
				inputText = keyboard.text;

				// Handle keyboard events or actions here if needed
			}
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				Debug.Log("Do something special here");
			}
			if (Application.platform == RuntimePlatform.WindowsPlayer)
			{
				Debug.Log("Do something special here Windows");
			}
		}


		#region Public Methods

		// public void start()
		// {
		//     progressLabel.SetActive(false);
		//     controlPanel.SetActive(true);
		// }


		/// <summary>
		/// Start the connection process. 
		/// - If already connected, we attempt joining a random room
		/// - if not yet connected, Connect this application instance to Photon Cloud Network
		/// </summary>
		public void Connect()
		{
			// we want to make sure the log is clear everytime we connect, we might have several failed attempted if connection failed.
			// feedbackText.text = "";
			if (PlayerNameInputField.locked)
			{
				forgottenName.SetActive(true);
				Debug.Log("Don't forget your own name");
				return;
			}

			if (TourGuideSelection.model == "tourguide")
			{
				if (PasswordInput.passwordLocked)
				{
					forgottenPassword.SetActive(true);
					return;
				}
			}

			// keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
			// isConnecting = true;
			// isConnecting = PhotonNetwork.ConnectUsingSettings();


			// hide the Play button for visual consistency
			progressLabel.SetActive(true);
			controlPanel.SetActive(false);
			forgottenName.SetActive(false);
			forgottenPassword.SetActive(false);
			Buttons.SetActive(false);
			PopUpSystem.isFrozen = false;

			// Debug.Log(playerName);



			// start the loader animation for visual effect.
			// if (loaderAnime!=null)
			// {
			// 	loaderAnime.StartLoaderAnimation();
			// }

			// we check if we are connected or not, we join if we are , else we initiate the connection to the server.
			if (PhotonNetwork.IsConnected)
			{
				// LogFeedback("Joining Room...");
				// #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
				PhotonNetwork.JoinRandomRoom();
			}
			else
			{

				// LogFeedback("Connecting...");

				// #Critical, we must first and foremost connect to Photon Online Server.
				// PhotonNetwork.ConnectUsingSettings();
				isConnecting = PhotonNetwork.ConnectUsingSettings();

				PhotonNetwork.GameVersion = gameVersion;

				// PhotonNetwork.GameVersion = this.gameVersion;
			}
			// Debug.Log(plrName);
			// ChatManager.GetConnected(plrName.text);
			// ChatManager.GetConnected(gameObject);
		}

		// /// <summary>
		// /// Logs the feedback in the UI view for the player, as opposed to inside the Unity Editor for the developer.
		// /// </summary>
		// /// <param name="message">Message.</param>
		// void LogFeedback(string message)
		// {
		// 	// we do not assume there is a feedbackText defined.
		// 	if (feedbackText == null) {
		// 		return;
		// 	}

		// 	// add new messages as a new line and at the bottom of the log.
		// 	feedbackText.text += System.Environment.NewLine+message;
		// }

		#endregion


		#region MonoBehaviourPunCallbacks CallBacks
		// // below, we implement some callbacks of PUN
		// // you can find PUN's callbacks in the class MonoBehaviourPunCallbacks


		// /// <summary>
		// /// Called after the connection to the master is established and authenticated
		// /// </summary>
		public override void OnConnectedToMaster()
		{
			//     // we don't want to do anything if we are not attempting to join a room. 
			// 	// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
			// 	// we don't want to do anything.
			Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
			if (isConnecting)
			{
				// 		// LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
				// Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

				// 		// #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
				PhotonNetwork.JoinRandomRoom();
				isConnecting = false;
			}
		}

		/// <summary>
		/// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
		/// </summary>
		/// <remarks>
		/// Most likely all rooms are full or no rooms are available. <br/>
		/// </remarks>
		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			// LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
			Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			// PhotonNetwork.CreateRoom(null, new RoomOptions());

			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
		}


		/// <summary>
		/// Called after disconnecting from the Photon server.
		/// </summary>
		public override void OnDisconnected(DisconnectCause cause)
		{
			// 	// LogFeedback("<Color=Red>OnDisconnected</Color> "+cause);
			// Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");
			Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);


			// 	// #Critical: we failed to connect or got disconnected. There is not much we can do. Typically, a UI system should be in place to let the user attemp to connect again.
			// 	// loaderAnime.StopLoaderAnimation();

			isConnecting = false;
			progressLabel.SetActive(false);
			controlPanel.SetActive(true);
			forgottenName.SetActive(false);
			forgottenPassword.SetActive(false);
			Buttons.SetActive(true);


		}

		/// <summary>
		/// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
		/// </summary>
		/// <remarks>
		/// This method is commonly used to instantiate player characters.
		/// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
		///
		/// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
		/// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
		/// enough players are in the room to start playing.
		/// </remarks>
		public override void OnJoinedRoom()
		{
			// LogFeedback("<Color=Green>OnJoinedRoom</Color> with "+PhotonNetwork.CurrentRoom.PlayerCount+" Player(s)");
			Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

			// #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("We load the 'Stevens Campus' ");

				// #Critical
				// Load the Room Level. 
				// PhotonNetwork.LoadLevel("Stevens Campus");
				// PhotonNetwork.LoadLevel("StevensFullMap");
				PhotonNetwork.LoadLevel("MobileStevensFullMap");
			}
		}

		#endregion
	}
}