using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerMovementController : PhysicsObject
{
    // Start is called before the first frame update
        
        public float jumpTakeOffSpeed = 7f;
        public float blockSpeed = -20f;
        PlayerAnimatorController playeranimatorcontroller;
        public AudioSource jump;
        public AudioSource big_jump;
        public bool wait;
        public AudioClip landThud;
        public AudioClip clothRipple;
        public AudioSource source;
        public AudioClip clothRipple2;
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
            if(transforming && !grounded){
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

        //     if(transforming){
        //     playeranimatorcontroller.ChangeAnimatorState("Block");
        // }
        }
         

        protected override void ComputeVelocity()
        {
            SetState();
            Vector2 move = Vector2.zero;
            
            move.x = Input.GetAxis("Horizontal");
            
            // if(Input.GetKey(KeyCode.LeftControl)){
            //     groundblock = true;
            // }

            

            if (Input.GetButtonDown("Jump") && grounded && !block)
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

            if (Input.GetKeyDown(KeyCode.LeftControl) && !transforming && !block && !unblock)
            {
                if(grounded)
                    onGround = true;
                else
                    onGround = false;

                transforming = true;
                StartCoroutine(Transformed());
                Debug.Log("transforming");
                //Physics.gravity = new Vector3(0, 9.81f, 0);
                //targetVelocity = move*0;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && grounded && !transforming && !block){
                //groundblock = true;
                StartCoroutine(ShakeDelayCo());
            }

            if(grounded && block && !unblock && !transforming && Input.GetKeyUp(KeyCode.LeftControl)){
                StartCoroutine(Unblock());
            }

            // if(grounded && !wait && Input.GetKeyUp(KeyCode.LeftControl)){
            //     StartCoroutine(Reset());
            //     Debug.Log("deasdfasdf");
                
            // }

            targetVelocity = move * maxSpeed;
        }

        public IEnumerator Transformed(){
            source.PlayOneShot(clothRipple,1f);
            if(!onGround && transforming && !block)
                playeranimatorcontroller.ChangeAnimatorState("Block");
            if(onGround && transforming && !block)
                playeranimatorcontroller.ChangeAnimatorState("GroundBlock");
            yield return new WaitForSeconds(0.3f);
                transforming = false;
                block = true;
            // if(!Input.GetKey(KeyCode.LeftControl))
            //     StartCoroutine(Unblock());
        }

        public IEnumerator Reset(){
            transforming = false;
            block = false;
            landed = false;
            nowalk = false;
            unblock = false;
            yield break;
        }

        public IEnumerator Unblock(){
            unblock = true;
            source.PlayOneShot(clothRipple2,1f);
            playeranimatorcontroller.ChangeAnimatorState("Unblock");
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(Reset());
        }

        public IEnumerator ResetAfterbreak(){
            yield return new WaitForSeconds(0.1f);
            transforming = false;
            landed = false;
        }
        
        protected override void ResetCo(){
            StartCoroutine(Land());
        }

        public IEnumerator Land(){
            landed = true;
            source.PlayOneShot(landThud,1f);
            //if(Input.GetKey(KeyCode.LeftControl)){
                //groundblock = true;
            //}
            Shake();
            playeranimatorcontroller.ChangeAnimatorState("BlockLand");
            yield return new WaitForSeconds(0.3f);
            playeranimatorcontroller.ChangeAnimatorState("BlockIdle");
            if(!Input.GetKey(KeyCode.LeftControl)){
                StartCoroutine(Unblock());
            }
            onGround = false;
        }

        protected override void ResetAB()
        {
            StartCoroutine(ResetAfterbreak());
        }

        protected override void Shake(){
            CameraShaker.Instance.ShakeOnce(4f, 6f, 0.2f, 0.2f);
        }

        // protected override void ShakeDelay(){
        //     CameraShaker.Instance.ShakeOnce(4f, 6f, 0.2f, 0.2f);
        // }

        public IEnumerator ShakeDelayCo(){
            yield return new WaitForSeconds(0.3f);
            CameraShaker.Instance.ShakeOnce(3f, 5f, 0.2f, 0.2f);
            yield return new WaitForSeconds(0.5f);
        }
        
}
