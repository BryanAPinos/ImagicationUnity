using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;


public class DialogLogic : MonoBehaviour
{
    public string popUp;
    bool playerInRange = false;
    private PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider collision)
    {
        // Called when player enters the trigger collider
        // For our purposes, this means the player is close enough to the NPC to talk to them
        Debug.Log(collision.gameObject.name);
        Debug.Log(PhotonNetwork.NickName);
        Debug.Log("Collision");

        playerInRange = true;
        // Set the name of the NPC to the name of the parent object
        PopUpSystem.postName = transform.parent.name;
    }
    void OnTriggerExit(Collider collision)
    {
        // Clear the name of the NPC when the player leaves the trigger collider
        PopUpSystem.postName = "";
    }
}
