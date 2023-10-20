using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

namespace Imagication
{
    public class ChatManager : MonoBehaviour, IChatClientListener
    {
        private ChatClient chatClient;

        public Text channelName;

        public InputField plrName;
        public Text connectionState;


        public InputField msgInput;
        public Text msgArea;

        public InputField[] channels;

        public InputField trymy;

        public GameObject msgPanel;
        public GameObject chatBox;


        private string worldchat;
        [SerializeField] private string userID;
        public static bool chatBoxFrozen = false;


        // Start is called before the first frame update
        void Start()
        {

            Debug.Log("Chat Manager Start");
            Application.runInBackground = true;
            if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
            {
                Debug.LogError("No AppID Provided");
                return;
            }

            worldchat = "world";
            GetConnected();

        }

        // Update is called once per frame
        void Update()
        {
            if (chatClient != null)
            {
                chatClient.Service();
            }
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SendMsg();
            }
            if (msgInput.isFocused)
            {
                chatBoxFrozen = true;
            }
            else
            {
                chatBoxFrozen = false;
            }
        }

        public void GetConnected()
        {
            Debug.Log("Connecting");
            chatClient = new ChatClient(this);
            chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
                new Photon.Chat.AuthenticationValues(PhotonNetwork.NickName));
        }

        public void SendMsg()
        {
            string msg = msgInput.text;
            if (string.IsNullOrEmpty(msg))
            {
                return;
            }
            chatClient.PublishMessage(worldchat, msgInput.text);
            msgInput.text = "";
            chatBox.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

        }

        public void DebugReturn(DebugLevel level, string message)
        {

        }

        public void OnDisconnected()
        {
        }

        public void OnConnected()
        {
            chatClient.Subscribe(new string[] { worldchat });
            chatClient.SetOnlineStatus(ChatUserStatus.Online);

        }

        public void OnChatStateChange(ChatState state)
        {

        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            for (int i = 0; i < senders.Length; i++)
            {
                msgArea.text += senders[i] + ": " + messages[i] + "\n";
            }
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {

        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            foreach (var channel in channels)
            {
                this.chatClient.PublishMessage(channel, "joined");
            }
            connectionState.text = "Connected";
        }

        public void OnUnsubscribed(string[] channels)
        {
        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
        }

        public void OnUserSubscribed(string channel, string user)
        {
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
        }

        public void OnInputField()
        {
        }
    }
}