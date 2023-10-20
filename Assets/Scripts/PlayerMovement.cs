using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

namespace Imagication
{
    // public class PlayerMovement : MonoBehaviourPunCallbacks
    public class PlayerMovement : MonoBehaviourPun
    {
        public CharacterController controller;
        public float speed = 8f;
        public float turnSpeed = 45f;
        public float jumpHeight = 3f;
        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;
        Vector3 velocity;
        bool isGrounded;

        //Mobile
        public GameObject JoystickUI;

        //Joystick reference for assign in inspector
        [SerializeField] private bl_Joystick Joystick;
        [SerializeField] private float Speed = 2f;

        //Mobile
        private Vector2 touchStartPos;
        private float rotationSpeed = 8.0f;

        //Animation
        private Animator _anim;

        #region Public Fields

        // [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        // public static GameObject LocalPlayerInstance;

        #endregion

        #region Private Fields

        // [Tooltip("The Player's UI GameObject Prefab")]
        // [SerializeField]
        // private GameObject playerUiPrefab;

        #endregion

        void Start()
        {
            _anim = GetComponent<Animator>();

            // Update is called once per frame
            void Update()
            {
                if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
                {
                    return;
                }

                if (PopUpSystem.isFrozen == true || ChatManager.chatBoxFrozen == true)
                {
                    return;
                }

                // Use vertical and horizontal axis to move the player
                var vel = Vector3.forward * Input.GetAxis("Vertical") * speed;
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

                // prevent player from floating/clipping
                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                    _anim.SetBool("Height", false);

                }

                float x = Input.GetAxis("Horizontal"); // A/D or Left/Right or Joystick
                float z = Input.GetAxis("Vertical"); // W/S or Up/Down or Joystick

                Vector3 move = transform.forward * z;

                controller.Move(move * speed * Time.deltaTime);

                // Handle Jumping
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    _anim.SetBool("Height", true);
                }

                velocity.y += gravity * Time.deltaTime;

                // Apply gravity to the player
                controller.Move(velocity * Time.deltaTime);

                // Rotate the player based on horizontal input 
                transform.Rotate(Vector3.up, x * Time.deltaTime * turnSpeed);

                //Animator
                _anim.SetFloat("Speed", vel.z);

                //MobileMovement
                if (Application.isMobilePlatform || Launcher.mobileTest) # TODO: avoid double implementation of input for mobile
                {
                    JoystickUI.SetActive(true);
                    float v = Joystick.Vertical; //get the vertical value of joystick
                    float h = Joystick.Horizontal;//get the horizontal value of joystick

                    _anim.SetFloat("Speed", v);
                    //in case you using keys instead of axis (due keys are bool and not float) you can do this:
                    //bool isKeyPressed = (Joystick.Horizontal > 0) ? true : false;

                    // Vector3 move = transform.forward * z;
                    // controller.Move(move * speed * Time.deltaTime);

                    //ready!, you not need more.
                    // Vector3 translate = (new Vector3(h, 0, v) * Time.deltaTime) * Speed;
                    Vector3 translate = (new Vector3(h, 0, v));
                    Vector3 playerForward = transform.forward;
                    Vector3 moveDirection = playerForward * translate.z + transform.right * translate.x;
                    // moveDirection.Normalize();

                    // transform.Translate(translate);
                    // controller.Move(translate  * Speed * Time.deltaTime);
                    controller.Move(moveDirection * Speed * Time.deltaTime);



                    if (Input.touchCount > 0)
                    {
                        Touch touch = Input.GetTouch(0);

                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                // Record the start position of the touch
                                touchStartPos = touch.position;
                                break;

                            case TouchPhase.Moved:
                                // Calculate the swipe direction based on the touch movement
                                Vector2 touchDelta = touch.position - touchStartPos;
                                float rotationAmount = touchDelta.x * rotationSpeed * Time.deltaTime;

                                // Rotate the character based on the swipe
                                // transform.Rotate(Vector3.up, rotationAmount, Space.World);
                                transform.Rotate(Vector3.up, rotationAmount);

                                // Update the touch start position for the next frame
                                touchStartPos = touch.position;
                                break;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(PopUpSystem.teleName))
                {
                    if (PopUpSystem.teleName == "BabbioDoor")
                        teleBabbio();
                    if (PopUpSystem.teleName == "B104Door")
                        teleExitBabbio();
                    if (PopUpSystem.teleName == "BurchardDoor")
                        teleBurchard();
                    if (PopUpSystem.teleName == "B111Door")
                        teleExitBurchard();
                    if (PopUpSystem.teleName == "EdwinDoor")
                        teleEdwin();
                    if (PopUpSystem.teleName == "EAS223Door")
                        teleExitEdwin();
                    if (PopUpSystem.teleName == "McLeanDoor")
                        teleMcLeanDoor();
                    if (PopUpSystem.teleName == "X105Door")
                        teleExitMcLeanDoor();
                }

            }
            void teleBabbio()
            {
                transform.position = new Vector3(102, 36, 732);
                transform.rotation = Quaternion.Euler(0, -90, 0);
                PopUpSystem.teleName = "";
            }
            void teleExitBabbio()
            {
                transform.position = new Vector3(126, 27, 326);
                transform.rotation = Quaternion.Euler(0, 100, 0);
                PopUpSystem.teleName = "";
            }
            void teleBurchard()
            {
                transform.position = new Vector3(79, 47, 792);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                PopUpSystem.teleName = "";
            }
            void teleExitBurchard()
            {
                transform.position = new Vector3(170, 26, 326);
                transform.rotation = Quaternion.Euler(0, -75, 0);
                PopUpSystem.teleName = "";
            }
            void teleEdwin()
            {
                transform.position = new Vector3(101, 41, 780);
                transform.rotation = Quaternion.Euler(0, 180, 0);
                PopUpSystem.teleName = "";
            }
            void teleExitEdwin()
            {
                transform.position = new Vector3(184, 23, 376);
                transform.rotation = Quaternion.Euler(0, -75, 0);
                PopUpSystem.teleName = "";
            }
            void teleMcLeanDoor()
            {
                transform.position = new Vector3(96, 38, 757);
                transform.rotation = Quaternion.Euler(0, 90, 0);
                PopUpSystem.teleName = "";
            }
            void teleExitMcLeanDoor()
            {
                transform.position = new Vector3(155, 22, 377);
                transform.rotation = Quaternion.Euler(0, 100, 0);
                PopUpSystem.teleName = "";
            }
        }
    }