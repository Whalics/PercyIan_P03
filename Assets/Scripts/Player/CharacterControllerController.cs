using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerController : MonoBehaviour
{

    public float moveSpeed;
    public float jumpSpeed;
    public LayerMask groundLayer;
    public Transform groundLevel;
    public Vector2 boxSize = new Vector2(0.1f,0.1f);
    public Rigidbody2D rb;
    public Animator animator;
    public bool doJump = false;
    public float move = 0;
    public Collider2D isGrounded;

    // Start is called before the first frame update
    
    void Awake(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move = Input.GetAxis("Horizontal");
        if(Input.GetButtonDown("Jump")){
            doJump = true;
        }
    }

    void FixedUpdate(){
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        isGrounded = IsGrounded();

        if(doJump)
            Jump();
    }

    public Collider2D IsGrounded(){
        return Physics2D.OverlapBox(groundLevel.position, boxSize, groundLayer, 0);
    }

    void Jump(){
        if(isGrounded){
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
        doJump = false;
    }
}
