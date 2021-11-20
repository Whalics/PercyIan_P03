using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerMovementController : PhysicsObject
{
    // Start is called before the first frame update
        public float maxSpeed = 3.1f;
        public float jumpTakeOffSpeed = 7f;
        public float blockSpeed = -20f;
        PlayerAnimatorController playeranimatorcontroller;
        public AudioSource jump;
        public AudioSource big_jump;
        //PauseMenu pausemenu;

        void Start()
        {
            playeranimatorcontroller = this.GetComponentInChildren<PlayerAnimatorController>();
            //pausemenu = GameObject.Find("Pause").GetComponent<PauseMenu>();
        }

        void Awake()
        {

        }
        
        
        void SetState(){
            if(transforming){
                gravityMultiplier = 0;
                velocity.y = Mathf.Abs(Physics2D.gravity.y) * Time.deltaTime;
            }
            if(!transforming && block){
                gravityMultiplier = 8;
                //gravityMultiplier = 1;
            }
            if(!transforming && !block){
                gravityMultiplier = 1;
            }

            if(transforming){
            playeranimatorcontroller.ChangeAnimatorState("Block");
        }
        }
         

        protected override void ComputeVelocity()
        {
            SetState();
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

            if (Input.GetKeyDown(KeyCode.LeftControl) && !grounded && !transforming && !block)
            {
                transforming = true;
                StartCoroutine(Transformed());
                //Physics.gravity = new Vector3(0, 9.81f, 0);
                //targetVelocity = move*0;
            }

            targetVelocity = move * maxSpeed;
        }

        public IEnumerator Transformed(){
            if(transforming && !block){
                playeranimatorcontroller.ChangeAnimatorState("Block");
            yield return new WaitForSeconds(0.4f);
                transforming = false;
                block = true;
            }
        }

        public IEnumerator Reset(){
            playeranimatorcontroller.ChangeAnimatorState("Unblock");
            yield return new WaitForSeconds(0.3f);
            transforming = false;
            block = false;
            landed = false;
        }
        
        protected override void ResetCo(){
            StartCoroutine(Land());
        }

        public IEnumerator Land(){
            landed = true;
            CameraShaker.Instance.ShakeOnce(4f, 6f, 0.2f, 0.2f);
            playeranimatorcontroller.ChangeAnimatorState("BlockLand");
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(Reset());
        }
}
