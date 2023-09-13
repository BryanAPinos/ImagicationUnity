using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JSONReader : MonoBehaviour
{
    public TextAsset JSONFile;
    // public Text test;
    public static int numberOfKeyPoints;
    // public static string[] nameOfBuildings = new string[numberOfKeyPoints];


    [System.Serializable]
    public class Building
    {
        public string title;
        public string description0;
        public string description1;
    }
    
    [System.Serializable]
    public class BuildingInfo
    {
        public Building[] Campus;
    }

    public BuildingInfo myBuildingInfo = new BuildingInfo();
    // Start is called before the first frame update
    void Start()
    {
        myBuildingInfo = JsonUtility.FromJson<BuildingInfo>(JSONFile.text);
        Debug.Log(myBuildingInfo.Campus.Length);
        numberOfKeyPoints = myBuildingInfo.Campus.Length;
        // Debug.Log(myBuildingInfo.Campus[0].Length);
        // Debug.Log(myBuildingInfo.ToString());
        // FindObjectOfType<Text>().text = myBuildingInfo.ToString();
        // GameObject.Find("Text").text = myBuildingInfo.ToString();
        // test.text = BuildingInfo.ToString();
        // test.text = myBuildingInfo.Campus[0].title;
    }

    public string[] getBuildingInfo(string postName)
    {
        string[] buildingInfo = new string[3];
        // string[] nameOfBuildings = new string[numberOfKeyPoints];
        // string[] buildingInfo = new string[myBuildingInfo.Campus.Length];

        for(int i = 0; i < myBuildingInfo.Campus.Length; i++){
            // nameOfBuildings[i] = myBuildingInfo.Campus[i].title;
            if(myBuildingInfo.Campus[i].title == postName){
                buildingInfo[0] = myBuildingInfo.Campus[i].title;
                buildingInfo[1] = myBuildingInfo.Campus[i].description0;
                buildingInfo[2] = myBuildingInfo.Campus[i].description1;
            }
        }
        
        // buildingInfo[0] = myBuildingInfo.Campus[0].title;

        return buildingInfo;
    }
}
