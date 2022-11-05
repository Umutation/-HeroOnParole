using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robberAi : MonoBehaviour
{
    RaycastHit2D[] casts;
    Rigidbody2D rb;                                                                             
    public float robberSpeed = 15;
    bool escaping = false; 
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        casts = Physics2D.CircleCastAll(transform.position,10, Vector2.zero);
        for(int i = 0; i<casts.Length; i++){

            if(casts[i].collider.CompareTag("HERO") && !escaping){  
                robberRunAway();
                escaping = true;
             
            }
        }
    }

    private void robberRunAway(){

        transform.rotation = Quaternion.Euler(0, 180,0);
        rb.velocity = robberSpeed * transform.right*-1; 
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
