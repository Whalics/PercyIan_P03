using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] Animator playerAnimator;
    [SerializeField] SpriteRenderer spriteRenderer;
    
    public bool wait;

    [Header("Animation")]
    [SerializeField] string currentState;

    [Header("Script References")]
    [SerializeField] PhysicsObject physicsobject;
    void Start(){

        playerAnimator = this.gameObject.GetComponent<Animator>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        //physicsobject = this.gameObject.GetComponent<PhysicsObject>();
    } 

    void Update(){

        // if(!physicsobject.groundblock){
        //     physicsobject.nowalk = false;
        // }
        if(!physicsobject.unblock && !physicsobject.block && !physicsobject.transforming && (Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("left") || Input.GetKey("right")) && physicsobject.grounded)
            ChangeAnimatorState("Walk");

        if(!physicsobject.unblock && !wait && (Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("left") || Input.GetKey("right")) && physicsobject.grounded){
            //blockwalk = true;
            //StartCoroutine(BlockWalk());
        }
        
        if(!physicsobject.unblock && !physicsobject.block && !physicsobject.transforming && !physicsobject.grounded && physicsobject.velocity.y > 0f)
            ChangeAnimatorState("JumpUp");
        else if(!physicsobject.unblock && !physicsobject.block && !physicsobject.transforming && !physicsobject.grounded && physicsobject.velocity.y < 0f)
            ChangeAnimatorState("JumpDown");
        
        if(!physicsobject.unblock && !physicsobject.block && !physicsobject.transforming && (!Input.GetKey("a") && !Input.GetKey("d") && !Input.GetKey("left") && !Input.GetKey("right")) && physicsobject.grounded)
            ChangeAnimatorState("Idle");
        
        if(!physicsobject.unblock && !physicsobject.block && physicsobject.grounded && Input.GetKey(KeyCode.LeftControl)){
            ChangeAnimatorState("BlockGround");
        }
        
        // if(block && !physicsobject.grounded && physicsobject.velocity.y < -2f)
        //     ChangeAnimatorState("BlockDown");


        
        if((Input.GetKey("a") || Input.GetKey("left")) && (!Input.GetKey("d") && !Input.GetKey("right")))
            spriteRenderer.flipX = true;

         if((Input.GetKey("d") || Input.GetKey("right")) && (!Input.GetKey("a") && !Input.GetKey("left")))
            spriteRenderer.flipX = false;

        
    }

    public void ChangeAnimatorState(string newState){
        if(currentState == newState) return;

        playerAnimator.Play(newState);

        currentState = newState;
    }

    // public IEnumerator BlockWalk(){
        
    //     wait = true;
    //     physicsobject.nowalk = false;
    //     if(Input.GetKey(KeyCode.LeftControl)){
    //     physicsobject.maxSpeed = 1f;
    //     playerAnimator.Play("BlockWalk");
    //     yield return new WaitForSeconds(0.6f);
    //     physicsobject.nowalk = true;
    //     yield return new WaitForSeconds(0.4f);
    //     }
    //     wait = false;
    //     physicsobject.maxSpeed = 3.3f;
    // }
}
