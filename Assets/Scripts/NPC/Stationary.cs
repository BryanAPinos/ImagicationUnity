using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Stationary : MonoBehaviourPun
{
    Rigidbody rb;
    Animator anim;

    public GameObject chatBox;
    public TextMeshProUGUI chatTitle;
    public TextMeshProUGUI npcMessage;
    public GameObject spacebarPopUp;
    public TextMeshProUGUI spacebarTitle;
    public TextMeshProUGUI spacebarInstruction;
    public ScrollRect myScrollRect;
    private static bool allowChat;
    private Vector3 targetObject;
    private AudioSource audioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        audioSource = GetComponent<AudioSource>();
    }


    void Update()
    {
        // If the player is close enough to the NPC, show the spacebar popup
        // and allow the player to talk to the NPC
        if (Input.GetKeyDown(KeyCode.Space) && allowChat)
        {
            audioSource.Play();
            spacebarPopUp.SetActive(false);
            chatBox.SetActive(true);
            anim.SetBool("Idle", false);
            anim.SetBool("isTalking", true);
            PopUpSystem.isFrozen = true;
            gameObject.transform.LookAt(targetObject);
            gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        }
    }
    public void chatPopUp(bool action, GameObject user)
    {
        // Enable the spacebar popup and set its text to the name of the NPC
        spacebarTitle.text = gameObject.transform.GetChild(0).name;
        spacebarInstruction.text = "Press <sprite=0> to talk to " + gameObject.transform.GetChild(0).name + ".";

        // Chat popup
        chatTitle.text = "Meet " + gameObject.transform.GetChild(0).name;
        allowChat = action;
        Vector3 targetPosition = new Vector3(user.transform.position.x, user.transform.position.y, user.transform.position.z);
        targetObject = targetPosition;

        spacebarPopUp.SetActive(action);
    }

    public void opt1()
    {
        // audioSource.Play();
        anim.SetBool("chickenDance", false);
        myScrollRect.verticalNormalizedPosition = 1;
        npcMessage.text = "I am flattered that you asked! As the beloved symbol of Stevens, I embody the spirit of our community—tenacious, forward-thinking, and always ready to take on new challenges. I love free bread, getting up at the quack of dawn, high fives and cheering on Stevens! My favorite colors are cardinal and gray. At big events, you’ll see me walking around greeting everyone. I hope to see you in person!";
    }
    public void opt2()
    {
        // audioSource.Play();
        anim.SetBool("chickenDance", false);
        myScrollRect.verticalNormalizedPosition = 1;
        npcMessage.text = "Ah, the wonderful opportunities that await you near Stevens Institute of Technology! Hoboken, our lively hometown, is brimming with exciting things to see and do. Allow me, Attila the Duck, to quack up a list of fantastic activities just a beak away from our campus: 1. Stroll along the waterfront 2. Catch live music and performances 3. Explore Washington Street 4. Visit Pier A Park 5. Hop on a ferry to NYC";
    }
    public void opt3()
    {
        // audioSource.Play();
        anim.SetBool("chickenDance", false);
        myScrollRect.verticalNormalizedPosition = 1;
        npcMessage.text = "The campus life and the student community at Stevens Institute of Technology are absolutely feather-tastic!  You'll find plenty of clubs, events, and opportunities to connect with like-minded peers. The close-knit environment fosters strong relationships between students and faculty. Get ready for an exciting and fulfilling college experience with a supportive flock by your side. Quack!";
    }
    public void opt4()
    {
        // audioSource.Play();
        audioSource.Stop();
        npcMessage.SetText("Check out my moves!");
        myScrollRect.verticalNormalizedPosition = 1;
        anim.SetBool("chickenDance", true);

    }
    private IEnumerator ByeWithDelay()
    {
        // Give the player 5 seconds to read the NPC's message before closing the chatbox
        yield return new WaitForSeconds(5f);
        chatBox.SetActive(false);
        npcMessage.text = "Quack-quack! Greetings, esteemed visitor! I'm Attila the Duck, the proud and spirited mascot of Stevens Institute of Technology. As you waddle your way onto our beautiful campus, let me be your cheerful guide and share a little about myself and this amazing place you're about to explore.";
        PopUpSystem.isFrozen = false;
        anim.SetBool("isTalking", false);
        anim.SetBool("Idle", true);

    }
    public void Bye()
    {
        // Perform the NPC's goodbye animation and close the chatbox
        audioSource.Play();
        anim.SetBool("chickenDance", false);
        myScrollRect.verticalNormalizedPosition = 1;
        npcMessage.text = "Bye! Hope to see you around!";
        allowChat = false;
        StartCoroutine(ByeWithDelay());
    }
}
