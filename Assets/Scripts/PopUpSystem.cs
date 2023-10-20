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

public class PopUpSystem : MonoBehaviourPun
{
    public GameObject KeyPoint;
    public GameObject spacebarBox;
    public GameObject blocker;
    public GameObject demoLimit;

    public TextMeshProUGUI InfoTitle;
    public TextMeshProUGUI keyPointsFound;
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

    public GameObject hat;
    private Transform PlayerTransform;
    GameObject player;

    List<string> nameOfBuildings = new List<string>();
    int currentPage = 1;

    public void PopUp(string popUpTrigger, string postName)
    {
        // Show the pop up box with the building name and information
        string[] buildInfo;

        JSONReader json = GameObject.Find("JSONReader").GetComponent<JSONReader>();
        buildInfo = json.getBuildingInfo(postName);

        if (popUpTrigger == "Pop") # TODO handle this better
        {
            spacebarBox.SetActive(true);
        }

        if (popUpTrigger == "Close")
        {
            spacebarBox.SetActive(false);
        }

        spacebarTitle.text = buildInfo[0];
        InfoTitle.text = buildInfo[0];

        popUpText.text = buildInfo[1];
        popUpText2.text = buildInfo[2];

        trigger = popUpTrigger;
    }

    void OnTriggerEnter(Collider collision)
    {
        // If the player collides with a building, show the pop up box

        // Ensure that the player who would be seeing the pop up box is the one who triggered it 
        if (!photonView.IsMine)
        {
            return;
        }
        if (collision.name == "BabbioDoor" || collision.name == "B104Door" || collision.name == "BurchardDoor" || collision.name == "B111Door" || collision.name == "EdwinDoor" || collision.name == "EAS223Door" || collision.name == "McLeanDoor" || collision.name == "X105Door")
        {
            Debug.Log(collision.name);
            teleName = collision.name;
            return;
        }
        if (collision.name == "White Board")
        {
            VideoController.videoPlayer1.Play();
            VideoController.videoPlayer2.Play();
            VideoController.videoPlayer1.SetDirectAudioMute(0, false);
            return;
        }
        if (collision.name == "DuckDance")
        {
            VideoController1.videoPlayer1.Play();
            VideoController1.videoPlayer2.Play();
            VideoController1.videoPlayer1.SetDirectAudioMute(0, false);
            return;
        }
        if (collision.transform.parent.name == "StationaryNPC")
        {
            Stationary npcStationary = collision.transform.parent.GetComponent<Stationary>();
            npcStationary.chatPopUp(true, gameObject);
            return;
        }
        if (collision.name == "Cone")
        {
            blocker.SetActive(true);
            return;
        }
        if (collision.name == "Wall")
        {
            demoLimit.SetActive(true);
            return;
        }
        if (collision.name == "Plane")
        {
            Debug.Log("Babbio in range");
            picture = true;
            return;
        }

        postName = collision.transform.parent.name;
        exclamationPoint = collision.transform.parent.Find("exclamation_point").gameObject;
        PopUp("Pop", postName);
    }

    void OnTriggerExit(Collider collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (collision.name == "White Board")
        {
            VideoController.videoPlayer1.SetDirectAudioMute(0, true);
            return;
        }
        if (collision.name == "DuckDance")
        {
            VideoController1.videoPlayer1.SetDirectAudioMute(0, true);
            return;
        }
        if (collision.transform.parent.name == "StationaryNPC")
        {
            Stationary npcStationary = collision.transform.parent.GetComponent<Stationary>();
            npcStationary.chatPopUp(false, gameObject);
            return;
        }
        if (collision.name == "Cone")
        {
            blocker.SetActive(false);
            return;
        }
        if (collision.name == "Wall")
        {
            demoLimit.SetActive(false);
            return;
        }
        postName = "";
        PopUp("Close", postName);
    }

    private void Update()
    {
        // If the player clicks on the building, show the pop up box
        if (picture && Input.GetMouseButtonDown(0)) // TODO: only true when "plane" is clicked
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("cond met");

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.name == "Plane")
                    Debug.Log("Babbio Pic object is clicked by mouse");
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && trigger == "Pop")
        {
            noName = true; // hide the name of the building
            isFrozen = true; // freeze the player ('freeze' their movement)
            popUpText.gameObject.SetActive(true); // show first page
            popUpText2.gameObject.SetActive(false);

            spacebarBox.SetActive(false);
            KeyPoint.SetActive(true);

            // Hide the exclamation point only for the player who triggered it
            if (photonView.IsMine)
            {
                exclamationPoint.SetActive(false);
            }

            // If the player hadn't found this buildling yet, they earn a key point
            if (!nameOfBuildings.Contains(postName))
                nameOfBuildings.Add(postName);
            keyPointsFound.text = nameOfBuildings.Count + "/" + JSONReader.numberOfKeyPoints + " Key Points Found";

            // If popUpText2 is showing, show the end button, otherwise show the continue button
            if (popUpText2.text.Length > 1 || !string.IsNullOrEmpty(popUpText2.text))
            {
                continueButton.gameObject.SetActive(true);
                endButton.gameObject.SetActive(false);
            }
            else
            {
                continueButton.gameObject.SetActive(false);
                endButton.gameObject.SetActive(true);
            }

            // If the player has found all the buildings, show the hat
            if (nameOfBuildings.Count == 4) # TODO: dynamic count of buildings
            {
                hat.gameObject.SetActive(true);
            }
        }
    }

    public void unFreeze()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
