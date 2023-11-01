using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imagication
{
    public class PlayerAnimatorManager : MonoBehaviour
    {

        #region Private Fields

        [SerializeField]
        private float directionDampTime = 0.25f;

        #endregion


        #region MonoBehaviour Callbacks

        private Animator _anim;

        void Start()
        {
            _anim = GetComponent<Animator>();
            if (!_anim)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (!_anim)
            {
                return;
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Ensure the vertcal directional values are non-negative
            if (v < 0)
            {
                v = 0;
            }

            _anim.SetFloat("Speed", h * h + v * v);

            _anim.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

            // deal with Jumping
            AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
            // only allow jumping if we are running
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetButtonDown("Fire2"))
                {
                    _anim.SetTrigger("Jump");
                }
            }
        }

        #endregion
    }
}
