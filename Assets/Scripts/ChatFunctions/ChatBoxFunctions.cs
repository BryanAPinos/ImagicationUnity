using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxFunctions : MonoBehaviour
{
    // [SerializeField] ContentSizeFitter contentSizeFitter;

    bool isChatShowing = false;
    public GameObject chatBox;
    public Button button;
    public Sprite newSprite;
    public Sprite oldSprite;


    void Start (){
        ToggleChat();
    }

    public void ToggleChat (){
        isChatShowing = !isChatShowing;
        if(isChatShowing){
            // contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            chatBox.SetActive(true);
            button.GetComponent<Image>().sprite = oldSprite;

        }
        else
        {
            // contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            chatBox.SetActive(false);
            button.GetComponent<Image>().sprite = newSprite;
            // oldImage.sprite = newSprite;
        }
   }

}
