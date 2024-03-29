﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGroundWalk : MonoBehaviour
{
    public AudioClip settle;
    public AudioSource source;
    public ParticleSystem dust;
   

    public void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            source.PlayOneShot(settle,0.1f);
            dust.Play();
       }
   }
}
