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

    void Start(){

        playerAnimator = this.gameObject.GetComponent<Animator>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        physicsobject = this.gameObject.GetComponent<PhysicsObject>();
    } 

    void Update(){
        if(Input.GetKey("a") || Input.GetKey("left"))
                spriteRenderer.flipX = true;

         if(Input.GetKey("d") || Input.GetKey("right"))
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
