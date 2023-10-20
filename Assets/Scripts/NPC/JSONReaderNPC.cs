using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class JSONReaderNPC : MonoBehaviour

{
    // Assign the JSON file in the Unity inspector
    public TextAsset JSONFile;

    // Dictionary to store the dialogue data
    private Dictionary<string, List<Dialogue>> dialogueData;

    [System.Serializable]
    private class DialogueContainer
    {
        public Dictionary<string, List<Dialogue>> dialogueData;
    }

    [System.Serializable]
    private class Dialogue
    {
        public string npc;
        public string[] npc_responses;
    }

    private void Awake()
    {
        // When the game starts, load the dialogue data from the JSON file
        dialogueData = new Dictionary<string, List<Dialogue>>();
        LoadDialogueData();
    }

    private void LoadDialogueData()
    {
        string jsonText = JSONFile.text;
        Debug.Log("jsonText: " + jsonText);

        DialogueContainer dialogueContainer = JsonUtility.FromJson<DialogueContainer>(jsonText);
        Debug.Log("dialogueContainer: " + dialogueContainer);

        dialogueData = dialogueContainer.dialogueData;
        Debug.Log("dialogueData: " + dialogueData);
    }

    private void StartConversation(string header)
    {
        Debug.Log("Start Convo");
        Debug.Log(dialogueData);

        // Check if the header exists in the JSON file
        if (dialogueData.ContainsKey(header))
        {
            // Get the list of dialogues for the header
            List<Dialogue> dialogues = dialogueData[header];

            // Display the NPC's dialogue
            foreach (Dialogue dialogue in dialogues)
            {
                Debug.Log("NPC: " + dialogue.npc);

                // Perform actions based on the user's response
                foreach (string response in dialogue.npc_responses)
                {
                    Debug.Log("User: " + response);
                }
            }
        }
    }

    // Call this method to start a conversation with Attila
    public void StartAttilaConversation()
    {
        StartConversation("Attila");
    }

    // Call this method to start a conversation with Dr. Doofenshmirtz
    public void StartDoofenshmirtzConversation()
    {
        StartConversation("Dr. Doofenshmirtz");
    }
}
