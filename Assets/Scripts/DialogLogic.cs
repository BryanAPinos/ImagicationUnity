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
    // public static string postName = "";
    // public static bool isFrozen = false;

    // string postName = gameObject.name;

    // public GameObject popUpBox;

    // Start is called before the first frame update
    void Start()
    {
        // var pv = PhotonView.Get(this);
        // var pv = PhotonView.isMine;
        PV = GetComponent<PhotonView>();
        // if(PhotonView.IsMine){
        //     Debug.Log("me");

        // }
        // Debug.Log('pv');
        // Debug.Log('pv' + pv);
        // postName = gameObject.name;


        // postName = transform.parent.name;


    }

    // Update is called once per frame
    void Update()
    {
        // if(playerInRange)
        // {
        //     PopUpSystem pop = GameObject.Find("Game Manager").GetComponent<PopUpSystem>();
        //     pop.PopUp(popUp, "Pop");
        // }
        // else    
        // {
        //     PopUpSystem pop = GameObject.Find("Game Manager").GetComponent<PopUpSystem>();
        //     pop.PopUp(popUp, "Close");            
        // }
        // if(!playerInRange)
        // {
        //     PopUpSystem pop = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PopUpSystem>();
        //     pop.PopUp(popUp);
        
        // }
        
    }

    void OnTriggerEnter(Collider collision)
    {
        // if(col.gameObject.tag != "Player")
        // {
        //     return;
        // }
        // if(collision..IsMine && collision.gameObject.tag == "Student 1.2")
        // {
        //     Debug.Log(collision.gameObject.name);
        // }
        // Debug.Log(isLocalPlayer);
        Debug.Log(collision.gameObject.name);
        // Debug.Log(postName);
        // Debug.Log(JSONReader.myBuildingInfo);
        // Debug.Log(PhotonNetwork.player);
        // Debug.Log(isLocalPlayer);
        // Debug.Log(photonView.IsMine);
        // Debug.Log(PV.IsMine);
        // var pv = gameObject.GetComponent(PhotonView).Get(this);
        // if (PV.IsMine == false)
        // {
        //     return;
        // }
        Debug.Log(PhotonNetwork.NickName);
        // if(PhotonNetwork.NickName != PhotonNetwork.NickName){
        //     return;
        // }
        
        Debug.Log("Collision");
        // PopUpSystem.PopUp("This is a test");

        playerInRange = true;
        PopUpSystem.postName = transform.parent.name;

        // PopUpSystem pop = GameObject.Find("Game Manager").GetComponent<PopUpSystem>();
        // pop.PopUp(popUp, "Pop", postName);

        // popup = GameManager.playerPrefab.GetComponent<PopUpSystem>();
        // GameObject clone = (GameObject)Instantiate(myPrefab);


        // if(collision.tag == "Player")
        // {
        //     playerInRange = true;
        // }

    }
    void OnTriggerExit(Collider collision)
    {
        PopUpSystem.postName = "";
        // playerInRange = false;
        // PopUpSystem pop = GameObject.Find("Game Manager").GetComponent<PopUpSystem>();
        // pop.PopUp(popUp, "Close", postName); 
    }
}
