using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Imagication
{
    [RequireComponent(typeof(InputField))]
    public class PasswordInput : MonoBehaviour
    {
        public static bool passwordLocked = true;

        void Start()
        {
            passwordLocked = true;
        }

        public void CheckInput(string password)
        {
            // check inputfield contains the string password <-- the password for now.
            if (string.IsNullOrEmpty(password))
            {
                // just a debug.Log to show that the password is correct (can be removed)
                Debug.Log("Password needed");
                passwordLocked = true;
                return;
            }
            if (password == "password123")
            {
                passwordLocked = false;
            }
        }
    }
}