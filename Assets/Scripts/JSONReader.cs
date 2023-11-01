using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JSONReader : MonoBehaviour
{
    public TextAsset JSONFile;
    public static int numberOfKeyPoints;

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
        // Campus is a collection of Building objects
        public Building[] Campus;
    }

    public BuildingInfo myBuildingInfo = new BuildingInfo();

    // Start is called before the first frame update
    void Start()
    {
        // Use the JSON file to populate the BuildingInfo object
        myBuildingInfo = JsonUtility.FromJson<BuildingInfo>(JSONFile.text);
        Debug.Log(myBuildingInfo.Campus.Length);

        // Set the number of key points to the number of buildings in the JSON file
        numberOfKeyPoints = myBuildingInfo.Campus.Length;
    }

    public string[] getBuildingInfo(string postName)
    {
        string[] buildingInfo = new string[3];

        // Search for and return the buildling info for the building whose title == postName
        for (int i = 0; i < myBuildingInfo.Campus.Length; i++)
        {
            if (myBuildingInfo.Campus[i].title == postName)
            {
                buildingInfo[0] = myBuildingInfo.Campus[i].title;
                buildingInfo[1] = myBuildingInfo.Campus[i].description0;
                buildingInfo[2] = myBuildingInfo.Campus[i].description1;
            }
        }

        return buildingInfo;
    }
}
