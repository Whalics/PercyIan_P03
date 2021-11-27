﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhysicsObject : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] protected Vector2 targetVelocity;
    public LayerMask L_Breakable;
    public bool grounded;
    protected Vector2 groundNormal;
    public float minGroundNormalY = 0.65f;
    [SerializeField] public Vector2 velocity;
    public float gravityModifier = 2f;
    public float gravityMultiplier;
    public Rigidbody2D rb;
    protected const float minMoveDistance = 0.001f;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected const float shellRadius = 0.01f;
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    public bool transforming;
    public bool block;
    public bool landed;
    public bool slow;
    public ParticleSystem part;
    public bool broken = false;
    public Transform rcpos;
    public bool nowalk;
    public float maxSpeed;
    public bool unblock;
    public bool onGround;
    Vector2 down = new Vector2(0,-1);
    //OnEnable is called when script is enabled
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero;

        ComputeVelocity();
    }
    protected virtual void ComputeVelocity()
    {

    }

    protected virtual void ResetCo()
    {

    }

    protected virtual void Shake()
    {

    }

    protected virtual void ResetAB()
    {

    }
    //Fixed update is a concrete update function
    void FixedUpdate()
    {

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime * gravityMultiplier;

        if(!block && !transforming && !nowalk && !unblock)
        velocity.x = targetVelocity.x;
        else
        velocity.x = 0;

        grounded = false;
        

        Vector2 deltaPosition = velocity * Time.deltaTime;

        
        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    public void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        if(distance > minMoveDistance)
        {
            int count = rb.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for(int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }
            for(int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if(currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if(yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                float projection = Vector2.Dot(velocity, currentNormal);
                if(projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        if(broken && grounded && block && !landed){
            ResetCo();
            Debug.Log("ResetCo caled cuz alr hit and landed ");
        }
        if(!broken && grounded && block && !landed){
            Raycast();
            Debug.Log("Raycast caled cuz havent hit yet");
        }
        

        // if(broken && grounded && block && !landed){
        //     ResetCo();
        // }
        rb.position = rb.position + move.normalized * distance;
    }

    public void Raycast(){
            RaycastHit2D hit = Physics2D.Raycast(rcpos.position,down, Mathf.Infinity, L_Breakable);
            Debug.DrawRay(rcpos.position,Vector3.down*100, Color.blue);
            if(hit.collider != null){
                if(hit && !broken && !onGround){
                    broken = true;
                    hit.collider.gameObject.SetActive(false);
                    part.Play();
                    ResetAB();
                    Debug.Log("ResetAB caled czu raycast hit and abt to hit again");
                    Shake();
                    
                }
                
                //Debug.Log("didnt hit");
                
            }
            if(!broken){
             Debug.Log("ResetCo called cuz never hit");
                ResetCo();
            }

        }
}
