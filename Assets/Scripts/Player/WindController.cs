using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public bool wind;
    public PhysicsObject physicsobject;
    public SpriteRenderer spWind;
    public SpriteRenderer spFade;
    public bool fade;
    float a = 0;
    [SerializeField] float speed = 0;
    // Start is called before the first frame update
    void Start()
    {
         StartCoroutine(Wind());
         physicsobject = GameObject.Find("Player").GetComponent<PhysicsObject>();
    }

    // Update is called once per frame
    public IEnumerator Wind(){
        yield return new WaitForSeconds(Random.Range(15,20));
        StartCoroutine(FadeIn());

        yield return new WaitForSeconds(6f);
        StartCoroutine(FadeOut());
        StartCoroutine(Wind());
    }

    void FixedUpdate(){
        speed = a;
        if(!physicsobject.transforming && !physicsobject.block && !physicsobject.unblock)
        physicsobject.Movement(Vector2.left*(speed/6), true);
        //physicsobject.rb.AddForce(Vector3.right*100, ForceMode2D.Impulse);
    }

    public IEnumerator FadeIn(){
            a = 0;
            do{
            spWind.color = new Color(1f,1f,1f,a);
            spFade.color = new Color(1f,1f,1f,a);
            yield return new WaitForSeconds(0.02f);
            a+=0.01f;
        }
        while(a < 1);
    }

    public IEnumerator FadeOut(){
            a = 1;
            do{
            spWind.color = new Color(1f,1f,1f,a);
            spFade.color = new Color(1f,1f,1f,a);
            yield return new WaitForSeconds(0.02f);
            a-=0.01f;
        }
        while(a > 0);
    }
}
