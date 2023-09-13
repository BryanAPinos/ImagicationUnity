using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


namespace Imagication
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields

		// static public GameManager Instance;

		[Tooltip("The prefab to use for representing the player")]
		public GameObject playerPrefab;

		[Tooltip("The prefab to use for representing the tour guide")]
		public GameObject tourGuidePrefab;

		#endregion
        // public GameObject bodyColor;
        // public GameObject leftArm;
        // public GameObject rightArm;

        // public Color color;



		// #region Private Fields

		// private GameObject instance;

        // [Tooltip("The prefab to use for representing the player")]
        // [SerializeField]
        // private GameObject playerPrefab;

        // #endregion

        // #region MonoBehaviour CallBacks

        // /// <summary>
        // /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        // /// </summary>
        void Start()
		{
		// 	Instance = this;

		// 	// in case we started this demo with the wrong scene being active, simply load the menu scene
		// 	if (!PhotonNetwork.IsConnected)
		// 	{
		// 		SceneManager.LoadScene("Launcher");

		// 		return;
		// 	}

			if (playerPrefab == null) { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

				Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
			} 
			else 
			{
				Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				// PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);


				if (PlayerManager.LocalPlayerInstance == null)
				{
					
				    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
					// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
					if (TourGuideSelection.model== "student") {
						// bodyColor = playerPrefab.transform.Find("User/body").gameObject;
						// leftArm = playerPrefab.transform.Find("User/l_sleeve").gameObject;
                		// rightArm = playerPrefab.transform.Find("User/r_sleeve").gameObject;

                		// color = UnityEngine.Random.ColorHSV();

						// bodyColor.GetComponent<Renderer>().sharedMaterial.color = color;
						// leftArm.GetComponent<Renderer>().sharedMaterial.color = color;
                		// rightArm.GetComponent<Renderer>().sharedMaterial.color = color;

						// PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
						PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(150f,30f,300f), Quaternion.identity, 0);
						
					}
					

					if(TourGuideSelection.model == "tourguide") {
						// PhotonNetwork.Instantiate(this.tourGuidePrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
						PhotonNetwork.Instantiate(this.tourGuidePrefab.name, new Vector3(150f,30f,300f), Quaternion.identity, 0);
						}
				}
				else
				{
					if(SceneManagerHelper.ActiveSceneName == "B104")
					{
						TourGuideSelection.model = "student";
						PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
					}
					Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
				}


			}

		}

		

		// /// <summary>
		// /// MonoBehaviour method called on GameObject by Unity on every frame.
		// /// </summary>
		// void Update()
		// {
		// 	// "back" button of phone equals "Escape". quit app if that's pressed
		// 	if (Input.GetKeyDown(KeyCode.Escape))
		// 	{
		// 		QuitApplication();
		// 	}
		// }

        // #endregion



        #region Photon Callbacks

        // /// <summary>
        // /// Called when a Photon Player got connected. We need to then load a bigger scene.
        // /// </summary>
        // /// <param name="other">Other.</param>
        public override void OnPlayerEnteredRoom(Player other)
		{
			Debug.Log( "OnPlayerEnteredRoom() {0}" + other.NickName); // not seen if you're the player connecting

			if ( PhotonNetwork.IsMasterClient )
			{
				Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom

				LoadArena();
			}
		}

		// /// <summary>
		// /// Called when a Photon Player got disconnected. We need to load a smaller scene.
		// /// </summary>
		// /// <param name="other">Other.</param>
		public override void OnPlayerLeftRoom(Player other)
		{
			Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

			// Debug.Log( "OnPlayerLeftRoom() " + other.NickName ); // seen when other disconnects

			if ( PhotonNetwork.IsMasterClient )
			{
				Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); // called before OnPlayerLeftRoom

				LoadArena(); 
			}
		}

		/// <summary>
		/// Called when the local player left the room. We need to load the launcher scene.
		/// </summary>
		public override void OnLeftRoom()
		{
			// SceneManager.LoadScene("Launcher");
			SceneManager.LoadScene(0);
		}

		#endregion

		#region Public Methods

		public void LeaveRoom()
		{
			PopUpSystem.isFrozen = false;
			PopUpSystem.noName = false;
			PhotonNetwork.LeaveRoom();
		}

		// public void QuitApplication()
		// {
		// 	Application.Quit();
		// }

		#endregion

		#region Private Methods

		void LoadArena()
		{
			if ( ! PhotonNetwork.IsMasterClient )
			{
				Debug.LogError( "PhotonNetwork : Trying to Load a level but we are not the master Client" );
			}

			Debug.LogFormat( "PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount );

			// PhotonNetwork.LoadLevel("PunBasics-Room for "+PhotonNetwork.CurrentRoom.PlayerCount);
			// PhotonNetwork.LoadLevel("Stevens Campus");
			PhotonNetwork.LoadLevel("StevensFullMap");
			// PhotonNetwork.LoadLevel("B104");
		}

		#endregion


    }
}