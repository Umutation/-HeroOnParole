using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHelicopter : MonoBehaviour
{   

    [SerializeField] GameObject rocket; 
    Rigidbody2D rb;                             
    public float copterSpeed = 0;
    float fireDelay = 2;
    [SerializeField] Transform rocketLauncher;    
    GameObject player; 
    public GameObject lights;
    dayClock gameController;
    bool lightsActive = false;




    void Start()
    {   
        rb = transform.parent.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("HERO");
        transform.rotation = Quaternion.Euler(0, 180,0);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<dayClock>();
        rb.velocity =  copterSpeed* transform.right; 
        StartCoroutine(ShootRockets());
        
    }
    
    
    IEnumerator ShootRockets(){

        while(true){
            Instantiate(rocket, rocketLauncher.position, Quaternion.identity); 
            yield return new WaitForSeconds(fireDelay);

        }

    }

    void Update()
    {

        if (!lightsActive && gameController.activateLights)
        {
            
            lights.SetActive(true);
            lightsActive = true;
        }
        else if (lightsActive && !gameController.activateLights)
        {
            lights.SetActive(false);
            lightsActive = false;
        }
    }
}
