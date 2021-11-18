using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] Animator playerAnimator;
    [SerializeField] SpriteRenderer spriteRenderer;
    
    [Header("Animation")]
    [SerializeField] string currentState;

    [Header("Script References")]
    [SerializeField] PhysicsObject physicsobject;

    [Header("Variables")]
    [SerializeField] bool block;
    void Start(){

        playerAnimator = this.gameObject.GetComponent<Animator>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        //physicsobject = this.gameObject.GetComponent<PhysicsObject>();
    } 

    void Update(){
        if((Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("left") || Input.GetKey("right")) && physicsobject.grounded && !block)
            ChangeAnimatorState("Walk");
        
        if(!physicsobject.grounded && physicsobject.velocity.y > 0f && !block)
            ChangeAnimatorState("JumpUp");
        else if(!physicsobject.grounded && physicsobject.velocity.y < 0f && !block)
            ChangeAnimatorState("JumpDown");
        
        if((!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey("a") && !Input.GetKey("d") && !Input.GetKey("left") && !Input.GetKey("right")) && physicsobject.grounded)
            ChangeAnimatorState("Idle");

        if(!physicsobject.grounded && Input.GetKeyDown(KeyCode.LeftControl)){
            block = true;
            ChangeAnimatorState("Block");
        }
        
        // if(block && !physicsobject.grounded && physicsobject.velocity.y < -2f)
        //     ChangeAnimatorState("BlockDown");
        

        if(block && physicsobject.grounded)
            ChangeAnimatorState("BlockLand");

        if(block && (Input.GetKeyUp(KeyCode.LeftControl) || physicsobject.grounded))
            block = false;

        

        
        if((Input.GetKey("a") || Input.GetKey("left")) && (!Input.GetKey("d") && !Input.GetKey("right")))
            spriteRenderer.flipX = true;

         if((Input.GetKey("d") || Input.GetKey("right")) && (!Input.GetKey("a") && !Input.GetKey("left")))
            spriteRenderer.flipX = false;

        if(!physicsobject.grounded){
            Debug.Log("not grounded");
        }
    }

    public void ChangeAnimatorState(string newState){
        if(currentState == newState) return;

        playerAnimator.Play(newState);

        currentState = newState;
    }
}
