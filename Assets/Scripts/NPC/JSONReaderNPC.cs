using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class JSONReaderNPC : MonoBehaviour

{
    public TextAsset JSONFile;  // Assign the JSON file in the Unity inspector
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
        public string user;
        public string[] responses;
    }

    private void Awake()
    {
        dialogueData = new Dictionary<string, List<Dialogue>>();
        LoadDialogueData();
    }

    private void LoadDialogueData()
    {
        string jsonText = JSONFile.text;
        Debug.Log("This is jsonText");
        Debug.Log(jsonText);

        DialogueContainer dialogueContainer = JsonUtility.FromJson<DialogueContainer>(jsonText);
        Debug.Log("this is dialogueContainer");
        Debug.Log(dialogueContainer.dialogueData);

        dialogueData = dialogueContainer.dialogueData;
    }

    private void StartConversation(string header)
    {
        Debug.Log("Start Convo");
        Debug.Log(dialogueData);
        if (dialogueData.ContainsKey(header))
        {
            List<Dialogue> dialogues = dialogueData[header];

            foreach (Dialogue dialogue in dialogues)
            {
                Debug.Log("NPC: " + dialogue.npc);

                // Perform actions based on the user's response
                foreach (string response in dialogue.responses)
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
