using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;

// public class PopUpSystem : MonoBehaviour
// public class PopUpSystem : MonoBehaviourPunCallbacks
// public class PopUpSystem : NetworkBehaviour
public class PopUpSystem : MonoBehaviourPun
{
    // public GameObject popUpBox;
    public GameObject KeyPoint;
    public GameObject spacebarBox;
    public GameObject blocker;
    public GameObject demoLimit;
    // public Animator animator;
    // public Text InfoTitle;
    public TextMeshProUGUI InfoTitle;
    // public Text keyPointsFound;
    public TextMeshProUGUI keyPointsFound;
    // public Text popUpText;
    public TextMeshProUGUI popUpText;
    public TextMeshProUGUI popUpText2;

    public TextMeshProUGUI spacebarTitle;

    public Button endButton;
    public Button continueButton;

    GameObject exclamationPoint;
    GameObject nonMain;


    private string trigger = "";
    public static bool picture = false;
    public static bool isFrozen = false;
    public static bool noName = false;
    public static string postName = "";
    public static string teleName = "";
    
    List<string> nameOfBuildings = new List<string>();

    public GameObject hat;
    private Transform PlayerTransform;
    // GameObject[] player;
    GameObject player;

    int currentPage = 1;

    public void PopUp(string popUpTrigger, string postName)
    {
        string[] buildInfo; 
       
        JSONReader json = GameObject.Find("JSONReader").GetComponent<JSONReader>();
        buildInfo = json.getBuildingInfo(postName);
    
        if(popUpTrigger == "Pop")
        {
            spacebarBox.SetActive(true);
            // if(!nameOfBuildings.Contains(postName))
            //     nameOfBuildings.Add(postName);
        }

        if(popUpTrigger == "Close")
        {
            spacebarBox.SetActive(false);
        }

        
        spacebarTitle.text = buildInfo[0];
        InfoTitle.text = buildInfo[0];
        popUpText.text = buildInfo[1];
        popUpText2.text = buildInfo[2];

        // keyPointsFound.text = nameOfBuildings.Count + "/" + JSONReader.numberOfKeyPoints  + " Key Points Found";
       
        trigger = popUpTrigger;

 
       
    }
    // public Stationary station;
    void OnTriggerEnter(Collider collision)
    {
        if(!photonView.IsMine){
            return;
        }
        if(collision.name == "BabbioDoor" || collision.name == "B104Door" || collision.name == "BurchardDoor" || collision.name == "B111Door" || collision.name == "EdwinDoor" || collision.name == "EAS223Door" || collision.name == "McLeanDoor" || collision.name == "X105Door"){
            // player = GameObject.FindGameObjectsWithTag("Player");
            // player = GameObject.FindWithTag("Player");
            // Debug.Log(GameObject.FindGameObjectsWithTag("Player")[0].transform.position);
            // Debug.Log(GameObject.FindGameObjectsWithTag("Player"));
            Debug.Log(collision.name);
            teleName = collision.name;
            // Code below to make invis
            // if(teleName == "BabbioDoor"){
            //     nonMain = GameObject.Find("FinalRoom");
            //     nonMain.layer = 0;

            //     var children = nonMain.GetComponentsInChildren<Transform>(includeInactive: true);
            //     foreach (var child in children)
            //     {
            //         child.gameObject.layer = 0;
            //     }
            //     Debug.Log(nonMain.layer);
            // }
            // if(teleName == "B104Door"){
            //     nonMain = GameObject.Find("FinalRoom");
            //     nonMain.layer = 11;
            //     var children = nonMain.GetComponentsInChildren<Transform>(includeInactive: true);
            //     foreach (var child in children)
            //     {
            //         child.gameObject.layer = 11;
            //     }

            //     Debug.Log(nonMain.layer);
            // }
            // Code above to make invis

            // PlayerMovement.teleBabbio();
            // player.transform.position = new Vector3(0,5,0);
            return;
        }
        if(collision.name == "White Board")
        {
            VideoController.videoPlayer1.Play();
            VideoController.videoPlayer2.Play();
            VideoController.videoPlayer1.SetDirectAudioMute(0,false);
            // VideoController.videoPlayer2.SetDirectAudioMute(0,false);
            return;
        }
        if(collision.name == "DuckDance")
        {
            VideoController1.videoPlayer1.Play();
            VideoController1.videoPlayer2.Play();
            VideoController1.videoPlayer1.SetDirectAudioMute(0,false);
            // VideoController.videoPlayer2.SetDirectAudioMute(0,false);
            return;
        }
        if(collision.transform.parent.name == "StationaryNPC"){
            // Debug.Log(collision.transform.parent);
            // Debug.Log(gameObject);user
            Stationary npcStationary = collision.transform.parent.GetComponent<Stationary>();
            npcStationary.chatPopUp(true, gameObject);
            // Debug.Log(collision.transform.parent.GetChild(0)); //To Get The Mascot
            // Debug.Log(collision.transform.parent.GetChild(2)); //To Get The Canvas
            return;
        }
        if(collision.name == "Cone")
        {
            blocker.SetActive(true);
            return;
        }
        if(collision.name == "Wall")
        {
            demoLimit.SetActive(true);
            return;
        }
        // if(collision.name == "Babbio Pic")
        if(collision.name == "Plane")
        {
            Debug.Log("Babbio in range");
            picture = true;
            return;
        }

        postName = collision.transform.parent.name;
        // exclamationPoint = collision.transform.parent.GetChild(0).name; //pulls first child 
        exclamationPoint = collision.transform.parent.Find("exclamation_point").gameObject;
        // Debug.Log(exclamationPoint);
        PopUp("Pop", postName);
    }

     void OnTriggerExit(Collider collision)
    {
        if(!photonView.IsMine){
            return;
        }
        
        // if(collision.transform.parent.name == "StationaryNPC"){
        //     Debug.Log("ExitAtilla");
        //     return;
        // }
        if(collision.name == "White Board")
        {
            VideoController.videoPlayer1.SetDirectAudioMute(0,true);
            // VideoController.videoPlayer2.SetDirectAudioMute(0,true);
            return;
        }
        if(collision.name == "DuckDance")
        {
            VideoController1.videoPlayer1.SetDirectAudioMute(0,true);
            // VideoController.videoPlayer2.SetDirectAudioMute(0,true);
            return;
        }
        // if(collision.transform.parent.name == "StationaryNPC"){
        //     // Debug.Log(collision.transform.parent);
        //     Stationary npcStationary = collision.transform.parent.GetComponent<Stationary>();
        //     npcStationary.chatPopUp(false, gameObject);
        //     // Debug.Log(collision.transform.parent.GetChild(0)); //To Get The Mascot
        //     // Debug.Log(collision.transform.parent.GetChild(2)); //To Get The Canvas
        //     return;
        // }
        if(collision.transform.parent.name == "StationaryNPC"){
            Stationary npcStationary = collision.transform.parent.GetComponent<Stationary>();
            npcStationary.chatPopUp(false, gameObject);
            return;
        }
        if(collision.name == "Cone")
        {
            blocker.SetActive(false);
            return;
        }
        if(collision.name == "Wall")
        {
            demoLimit.SetActive(false);
            return;
        }
        postName = "";
        PopUp("Close", postName);

    }

    private void Update()
    {
       	if (picture && Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("cond met");
			
			if (Physics.Raycast(ray, out hit)) {
                // Debug.Log(hit.collider.name);
                Debug.Log(hit.transform.name);
                
				if (hit.transform.name == "Plane" )
                    Debug.Log("Babbio Pic object is clicked by mouse");
			}
		}

        if(Input.GetKeyDown(KeyCode.Space) && trigger == "Pop")
        {
            noName = true;
            isFrozen = true;
            popUpText.gameObject.SetActive(true);
            popUpText2.gameObject.SetActive(false);

            spacebarBox.SetActive(false);
            KeyPoint.SetActive(true);
            if(photonView.IsMine){
                exclamationPoint.SetActive(false);
            // return;
            }
            // Debug.Log(popUpText2.text.Length);
            // PhotonNetwork.NickName.SetActive(false);
            // PlayerUiPrefab.SetActive(false);

            if(!nameOfBuildings.Contains(postName))
                nameOfBuildings.Add(postName);
                keyPointsFound.text = nameOfBuildings.Count + "/" + JSONReader.numberOfKeyPoints  + " Key Points Found";

            if(popUpText2.text.Length > 1 || !string.IsNullOrEmpty(popUpText2.text)){
                continueButton.gameObject.SetActive(true);
                endButton.gameObject.SetActive(false);
            }
            else
            {
                continueButton.gameObject.SetActive(false);
                endButton.gameObject.SetActive(true);
            }
            if(nameOfBuildings.Count == 4)
            {
                hat.gameObject.SetActive(true);
            }

        }

        
    }

    public void unFreeze()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }
        KeyPoint.SetActive(false);

        isFrozen = false;
        noName = false;
    }

    public void nextPage()
    {
        popUpText.gameObject.SetActive(false);
        popUpText2.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
        endButton.gameObject.SetActive(true);
    }
   

}
