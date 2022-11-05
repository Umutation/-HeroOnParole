using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{

    public ParticleSystem miniExplosion; 
    Rigidbody2D rb; 
    public float rocketSpeed =0;
    GameObject player; 
    GC gameController; 

    public void explode()
    {
        ParticleSystem explosion = Instantiate(miniExplosion, transform.position, Quaternion.identity);
        explosion.Play();

    }
    float Rotator(Transform target){

       
        Vector2 direction = target.position - transform.position;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg-95; 

        return angle;



    }
    void Start()
    {   

        player = GameObject.FindGameObjectWithTag("HERO");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GC>();
        rb = GetComponent<Rigidbody2D>();
        transform.rotation = Quaternion.Euler(0, 0, -Rotator(player.transform));
        rb.velocity = transform.right*rocketSpeed; 
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("HERO")){
            player.GetComponent<control_player>().isStunned = true;
            explode();
            Destroy(this.gameObject);
            
        }else if(other.CompareTag("civilian")){
            Destroy(other.gameObject);
            gameController.ReduceBadge();
            Destroy(this.gameObject);
            explode();
        }

       
        
    }

   
}
