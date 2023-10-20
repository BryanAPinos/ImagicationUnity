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
            passwordLocked = string.IsNullOrEmpty(password) || password != "password123";
        }
    }
}