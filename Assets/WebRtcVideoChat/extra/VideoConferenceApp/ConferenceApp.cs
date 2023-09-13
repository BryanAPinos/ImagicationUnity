/* 
 * Copyright (C) 2021 because-why-not.com Limited
 * 
 * Please refer to the license.txt for license information
 */
using Byn.Awrtc;
using Byn.Awrtc.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Byn.Unity.Examples
{

    /// <summary>
    /// USE AT YOUR OWN RISK!
    /// 
    /// Allows to test the future feature for conference calls / n to n connections.
    /// 
    /// The current conference support is still limited. It will be replaced with a better
    /// implementation in the future. If you need a more stable (but more complicated)
    /// method now use IMediaNetwork instead.
    /// 
    /// Typical problems with the ConferenceApp:
    /// * handling multiple streams can be very CPU intensive (keep resolution low)
    /// * System can't handle failed direct connections
    ///     e.g. if 3 users join the same call but two of them 
    ///     just can't connect directly due to firewall / stun fails.
    ///     Use TURN if possible to reduce the risk of this happening!
    /// 
    /// 
    /// Note the signaling server server needs the correct flag in config.json
    ///     "address_sharing": true
    ///     !!!!
    ///     
    /// e.g.:
    /// 
    ///    "apps": [
    ///        {
    ///            "name": "ConferenceApp",
    ///            "path": "/conferenceapp",
    ///            "address_sharing": true
    ///        }
    ///        
    /// for url ws://because-why-not.com:12776/conferenceapp
    /// </summary>
    public class ConferenceApp : MonoBehaviour
    {
        /// <summary>
        /// Length limit of signaling server address
        /// </summary>
        private const int MAX_CODE_LENGTH = 256;

        #region UI
        /// <summary>
        /// Input field used to enter the room name.
        /// </summary>
        public InputField uRoomName;

        /// <summary>
        /// Input field to enter a new message.
        /// </summary>
        public InputField uMessageField;

        /// <summary>
        /// Output message list to show incoming and sent messages + output messages of the
        /// system itself.
        /// </summary>
        public MessageList uOutput;
        public Text connectionStatus;
        public GameObject VCPanel;
        public Image VCPanelImg;

        /// <summary>
        /// Join button to connect to a server.
        /// </summary>
        public Button uJoin;

        /// <summary>
        /// Send button.
        /// </summary>
        public Button uSend;


        /// <summary>
        /// Shutdown button. Disconnects all connections + shuts down the server if started.
        /// </summary>
        public Button uShutdown;

        /// <summary>
        /// Panel with the join button. Will be hidden after setup
        /// </summary>
        public GameObject uSetupPanel;

        /// <summary>
        /// Space used for video images
        /// </summary>
        public GameObject uVideoLayout;

        /// <summary>
        /// Prefab used for new user screen / video image
        /// </summary>
        public GameObject uVideoPrefab;


        /// <summary>
        /// Texture used to indicate users that don't stream video.
        /// </summary>
        public Texture2D uNoImgTexture;
        #endregion


        public GameObject micButton;
        public Sprite micOnSprite;
        public Sprite micOffSprite;

        /// <summary>
        /// Call class handling all the functionality
        /// </summary>
        private ICall mCall;

        private MediaConfig mMediaConfig = new MediaConfig();
        /// <summary>
        /// Configuration of audio / video functionality
        /// </summary>
        public MediaConfig MediaConfig
        {
            get
            {
                return mMediaConfig;
            }
            set
            {

                mMediaConfig = value;
            }
        }

        private NetworkConfig mNetConfig = new NetworkConfig();
        /// <summary>
        /// Network / server configuration
        /// </summary>
        public NetworkConfig NetConfig { get { return mNetConfig; } set { mNetConfig = value; } }

        /// <summary>
        /// Class used to keep track of each individual connection and its data / ui
        /// </summary>
        private class VideoData
        {
            public GameObject uiObject;
            public Texture2D texture;
            public RawImage image;

        }

        /// <summary>
        /// Dictionary to resolve connection ID with their specific data
        /// </summary>
        private Dictionary<ConnectionId, VideoData> mVideoUiElements = new Dictionary<ConnectionId, VideoData>();

        public int ConnectionCount
        {
            get
            {
                return mVideoUiElements.Count - 1;
            }
        }

        /// <summary>
        /// Unity start.
        /// </summary>
        private void Start()
        {
            UnityCallFactory.RequestLogLevelStatic(UnityCallFactory.LogLevel.Info);
            UnityCallFactory.EnsureInit(OnCallFactoryReady, OnCallFactoryFailed);
        }
        
        protected virtual void OnCallFactoryReady()
        {
                //to trigger android permission requests
                StartCoroutine(ExampleGlobals.RequestPermissions());
                //use video and audio by default (the UI is toggled on by default as well it will change on click )
                MediaConfig.Video = false;
                MediaConfig.Audio = false;
                MediaConfig.VideoDeviceName = UnityCallFactory.Instance.GetDefaultVideoDevice();

                NetConfig.IceServers.Add(ExampleGlobals.DefaultIceServer);
                NetConfig.SignalingUrl = ExampleGlobals.SignalingConference;
                NetConfig.IsConference = true;
                this.uRoomName.text = Application.productName + "_con";
        }

        protected virtual void OnCallFactoryFailed(string error)
        {
            string fullErrorMsg = typeof(CallApp).Name + " can't start. The " + typeof(UnityCallFactory).Name + " failed to initialize with following error: " + error;
            Debug.LogError(fullErrorMsg);
        }


        /// <summary>
        /// Creates the call object and uses the configure method to activate the 
        /// video / audio support if the values are set to true.
        /// </summary>
        /// <param name="useAudio">Uses the local microphone for the call</param>
        /// <param name="useVideo">Uses a local camera for the call. The camera will start
        /// generating new frames after this call so the user can see himself before
        /// the call is connected.</param>
        private void Setup(bool useAudio = true, bool useVideo = true)
        {
            // Append("Setting up ...");
            Append("Connecting...");

            //setup the server
            Debug.Log("Creating ICall with " + NetConfig);
            mCall = UnityCallFactory.Instance.Create(NetConfig);
            if (mCall == null)
            {
                Append("Failed to create the call");
                return;
            }

            // Append("Call created!");
            VCPanel.SetActive(true);
            VCPanelImg.color = new Color32(46,204,113,255);
            Append("Connected!");
            // toggleMic();

            mCall.CallEvent += Call_CallEvent;

            //setup local video element
            SetupVideoUi(ConnectionId.INVALID);
            mCall.Configure(MediaConfig);


            SetGuiState(false);
        }

        /// <summary>
        /// Destroys the call object and shows the setup screen again.
        /// Called after a call ends or an error occurred.
        /// </summary>
        private void ResetCall()
        {
            foreach (var v in mVideoUiElements)
            {
                Destroy(v.Value.uiObject);
                if (v.Value.texture != null)
                    Destroy(v.Value.texture);
            }
            mVideoUiElements.Clear();
            CleanupCall();
            SetGuiState(true);
        }

        /// <summary>
        /// Handler of call events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Call_CallEvent(object sender, CallEventArgs e)
        {
            switch (e.Type)
            {
                case CallEventType.CallAccepted:
                    //Outgoing call was successful or an incoming call arrived
                    // Append("Connection established");
                    Append("Enjoy the call!");
                    OnNewCall(e as CallAcceptedEventArgs);
                    break;
                case CallEventType.CallEnded:
                    OnCallEnded(e as CallEndedEventArgs);
                    break;
                case CallEventType.ListeningFailed:
                    Append("Failed to listen for incoming calls! Server might be down!");
                    ResetCall();
                    break;

                case CallEventType.ConnectionFailed:
                    {
                        //this should be impossible to happen in conference mode!
                        ErrorEventArgs args = e as ErrorEventArgs;
                        Append("Error: " + args.Info);
                        Debug.LogError(args.Info);
                        ResetCall();
                    }
                    break;

                case CallEventType.FrameUpdate:
                    //new frame received from webrtc (either from local camera or network)
                    FrameUpdateEventArgs frameargs = e as FrameUpdateEventArgs;
                    UpdateFrame(frameargs);
                    break;
                case CallEventType.Message:
                    {
                        //text message received
                        MessageEventArgs args = e as MessageEventArgs;
                        Append(args.Content);
                        break;
                    }
                case CallEventType.WaitForIncomingCall:
                    {
                        //the chat app will wait for another app to connect via the same string
                        WaitForIncomingCallEventArgs args = e as WaitForIncomingCallEventArgs;
                        // Append("Waiting for incoming call address: " + args.Address);
                        Append("Waiting for someone to join");
                        break;
                    }
            }

        }

        /// <summary>
        /// Event triggers for a new incoming call
        /// (in conference mode there is no difference between incoming / outgoing)
        /// </summary>
        /// <param name="args"></param>
        private void OnNewCall(CallAcceptedEventArgs args)
        {
            SetupVideoUi(args.ConnectionId);
        }

        /// <summary>
        /// Creates the connection specific data / ui
        /// </summary>
        /// <param name="id"></param>
        private void SetupVideoUi(ConnectionId id)
        {
            //create texture + ui element
            VideoData vd = new VideoData();
            vd.uiObject = Instantiate(uVideoPrefab);
            vd.uiObject.transform.SetParent(uVideoLayout.transform, false);
            vd.image = vd.uiObject.GetComponentInChildren<RawImage>();
            vd.image.texture = uNoImgTexture;
            mVideoUiElements[id] = vd;
        }

        /// <summary>
        /// User left. Cleanup connection specific data / ui
        /// </summary>
        /// <param name="args"></param>
        private void OnCallEnded(CallEndedEventArgs args)
        {
            VideoData data;
            if (mVideoUiElements.TryGetValue(args.ConnectionId, out data))
            {
                if (data.texture != null)
                    Destroy(data.texture);
                Destroy(data.uiObject);
                mVideoUiElements.Remove(args.ConnectionId);
            }
        }




        /// <summary>
        /// Updates the frame for a connection id. If the id is new it will create a
        /// visible image for it. The frame can be null for connections that
        /// don't sent frames.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="frame"></param>
        private void UpdateFrame(FrameUpdateEventArgs args)
        {
            if (mVideoUiElements.ContainsKey(args.ConnectionId))
            {
                VideoData videoData = mVideoUiElements[args.ConnectionId];
                //make sure not to overwrite / destroy our texture for missing image data
                if (videoData.image.texture == this.uNoImgTexture)
                    videoData.image.texture = null;
                bool mirror = args.IsRemote == false;
                //converts the frame data to a texture and sets it to the raw image
                UnityMediaHelper.UpdateRawImageTransform(videoData.image, args.Frame, mirror);
                videoData.texture = videoData.image.texture as Texture2D;
            }
        }

        /// <summary>
        /// Destroys the call. Used if unity destroys the object or if a call
        /// ended / failed due to an error.
        /// 
        /// </summary>
        private void CleanupCall()
        {
            if (mCall != null)
            {

                Debug.Log("Destroying call!");
                mCall.Dispose();
                mCall = null;
                Debug.Log("Call destroyed");
                Append("");
                VCPanelImg.color = UnityEngine.Color.red;
                VCPanel.SetActive(false);


                // Append("Disconnecting");
            }
        }
        private void OnDestroy()
        {
            CleanupCall();
        }


        /// <summary>
        /// toggle audio on / off
        /// </summary>
        /// <param name="state"></param>
        public void AudioToggle(bool state)
        {
            // Debug.Log(state);
            MediaConfig.Audio = state;
        }

        /// <summary>
        /// toggle video on / off
        /// </summary>
        /// <param name="state"></param>
        public void VideoToggle(bool state)
        {
            MediaConfig.Video = state;
        }

        /// <summary>
        /// Adds a new message to the message view
        /// </summary>
        /// <param name="text"></param>
        private void Append(string text)
        {
            if (uOutput != null)
            {
                uOutput.AddTextEntry(text);
                connectionStatus.text = text;
            }
            else
            {
                Debug.Log("Chat: " + text);
            }
        }

        /// <summary>
        /// The call object needs to be updated regularly to sync data received via webrtc with
        /// unity. All events will be triggered during the update method in the unity main thread
        /// to avoid multi threading errors
        /// </summary>
        private void Update()
        {
            if (mCall != null)
            {
                //update the call
                mCall.Update();
            }
        }

        #region UI 
        /// <summary>
        /// Shows the setup screen or the chat + video
        /// </summary>
        /// <param name="showSetup">true Shows the setup. False hides it.</param>
        private void SetGuiState(bool showSetup)
        {
            uSetupPanel.SetActive(showSetup);

            uSend.interactable = !showSetup;
            uShutdown.interactable = !showSetup;
            uMessageField.interactable = !showSetup;

        }

        /// <summary>
        /// Join button pressed. Tries to join a room.
        /// </summary>
        public void JoinButtonPressed()
        {
            MediaConfig.Audio = true;
            Setup();
            EnsureLength();
            mCall.Listen(uRoomName.text);
            micButton.GetComponent<Image>().sprite = micOnSprite;
            micButton.SetActive(true);
        }

        /// <summary>
        /// Helper to enforce the length limit
        /// </summary>
        private void EnsureLength()
        {
            if (uRoomName.text.Length > MAX_CODE_LENGTH)
            {
                uRoomName.text = uRoomName.text.Substring(0, MAX_CODE_LENGTH);
            }
        }

        /// <summary>
        /// This is called if the send button
        /// </summary>
        public void SendButtonPressed()
        {
            //get the message written into the text field
            string msg = uMessageField.text;
            SendMsg(msg);
        }

        /// <summary>
        /// User either pressed enter or left the text field
        /// -> if return key was pressed send the message
        /// </summary>
        public void InputOnEndEdit()
        {
            if (Input.GetKey(KeyCode.Return))
            {
                string msg = uMessageField.text;
                SendMsg(msg);
            }
        }

        /// <summary>
        /// Sends a message to the other end
        /// </summary>
        /// <param name="msg"></param>
        private void SendMsg(string msg)
        {
            if (String.IsNullOrEmpty(msg))
            {
                //never send null or empty messages. webrtc can't deal with that
                return;
            }

            Append(msg);
            mCall.Send(msg);

            //reset UI
            uMessageField.text = "";
            uMessageField.Select();
        }



        /// <summary>
        /// Shutdown button pressed. Shuts the network down.
        /// </summary>
        public void ShutdownButtonPressed()
        {
            ResetCall();
            micButton.SetActive(false);
        }
        #endregion

        public void toggleMic(){
            if(micButton.GetComponent<Image>().sprite == micOnSprite){
                micButton.GetComponent<Image>().sprite = micOffSprite;
                mCall.SetMute(true);
                return;
            }
            micButton.GetComponent<Image>().sprite = micOnSprite;
            mCall.SetMute(false);

        }

    }

   

}