using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxFunctions : MonoBehaviour
{
    // Initialize chatBox to be hidden
    bool isChatShowing = false;
    public GameObject chatBox;
    public Button button;
    public Sprite newSprite;
    public Sprite oldSprite;

    void Start()
    {
        // Toggle chatBox visibilty when button is clicked
        ToggleChat();
    }

    public void ToggleChat()
    {
        isChatShowing = !isChatShowing;
        if (isChatShowing)
        {
            chatBox.SetActive(true);
            button.GetComponent<Image>().sprite = oldSprite;
        }
        else
        {
            chatBox.SetActive(false);
            button.GetComponent<Image>().sprite = newSprite;
        }
    }

}
