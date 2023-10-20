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
        Debug.Log(collision.gameObject.name);
        Debug.Log(PhotonNetwork.NickName);
        Debug.Log("Collision");

        playerInRange = true;
        PopUpSystem.postName = transform.parent.name;
    }
    void OnTriggerExit(Collider collision)
    {
        PopUpSystem.postName = "";
    }
}
