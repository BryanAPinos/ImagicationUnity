using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    public InputField inputFieldCreate;
    public GameObject intro;
    public GameObject join;
    public GameObject game;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Use a Button to Call this Function
    public void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    //Use a Button to Call this Function
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(inputFieldCreate.text, roomOptions, TypedLobby.Default);
    }

    //Use a Button to Call this Function
    public void JoinRandomly()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //Use a Button to Call this Function
    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

        base.OnConnectedToMaster();
    }

    public override void OnJoinedLobby()
    {
        intro.SetActive(false);
        join.SetActive(true);

        base.OnJoinedLobby();
    }

    public override void OnJoinedRoom()
    {
        join.SetActive(false);
        game.SetActive(true);

        camera.enabled = false;

        PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);

        base.OnJoinedRoom();
    }

    public override void OnLeftRoom()
    {
        join.SetActive(true);
        game.SetActive(false);

        camera.enabled = true;

        base.OnLeftRoom();
    }
}