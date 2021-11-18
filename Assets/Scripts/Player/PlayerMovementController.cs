using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : PhysicsObject
{
    // Start is called before the first frame update
        public float maxSpeed = 3.1f;
        public float jumpTakeOffSpeed = 7f;
        public float blockSpeed = 1f;
        //PlayerAnimatorController playeranimatorcontroller;
        public AudioSource jump;
        public AudioSource big_jump;
        //PauseMenu pausemenu;

        void Start()
        {
            //playeranimatorcontroller = this.GetComponent<PlayerAnimatorController>();
            //pausemenu = GameObject.Find("Pause").GetComponent<PauseMenu>();
        }

        void Awake()
        {

        }

        protected override void ComputeVelocity()
        {
            Vector2 move = Vector2.zero;
            
            move.x = Input.GetAxis("Horizontal");
            
            
            
            if (Input.GetButtonDown("Jump") && grounded)
            {
                velocity.y = jumpTakeOffSpeed;
                jump.Play();
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * 0.5f;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && !grounded)
            {
                Physics.gravity = Vector3.zero;
                velocity.y = blockSpeed;
                targetVelocity = move*0;
            }

            targetVelocity = move * maxSpeed;
        }
}
