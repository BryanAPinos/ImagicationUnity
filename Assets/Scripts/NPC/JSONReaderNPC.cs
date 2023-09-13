// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class JSONReaderNPC : MonoBehaviour
// {
//     public TextAsset JSONFile;
    
//     [System.Serializable]
//     public class ChatData
//     {
//         public string npc;
//         public string user;
//         public string[] responses;
//     }
    
//     [System.Serializable]
//     public class CharacterData
//     {
//         public ChatData[] conversations;
//     }

//     // [System.Serializable]
//     // public class ChatDataContainer
//     // {
//     //     public Dictionary<string, CharacterData> Characters;
//     // }

//     // public NPCFlowChat npcChatInfo = new NPCFlowChat();
//     public CharacterData dataContainer = new CharacterData();

//     void Start()
//     {
//         // ChatDataContainer dataContainer = JsonUtility.FromJson<ChatDataContainer>(JSONFile.text);
//         dataContainer = JsonUtility.FromJson<CharacterData>(JSONFile.text);
//         // CharacterData dataContainer = JsonUtility.FromJson<CharacterData>(JSONFile.text);
//         Debug.Log("This is the JSONREADERNPC");
//         Debug.Log(dataContainer);
//         // Debug.Log(dataContainer.conversations.Length);
//         for(int i = 0; i < dataContainer.conversations.Length; i++)
//         {
//             Debug.Log(dataContainer.conversations[i]);
//         }


        
//         // CharacterData.conversations[i];
//     }

// }
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
// using System.Collections.Generic;
// using UnityEngine;

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
