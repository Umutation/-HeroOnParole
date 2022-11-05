using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class civilianAI : MonoBehaviour
{

    public float mSpeed = 0, sprintSpeed = 6;  
    public int crimeSceneCount = 0, type = 0;  

    public Rigidbody2D rb; 
    public bool escaping = false, panicking = false, arrangedMovement = false; 
    bool crimePresent = false;
    public bool lightsActive = false; 
    public Animator anim; 
    GC gameController;

    //helicopter
    public GameObject lights;
    dayClock timeController; 


    

    Transform lastCrimeScene; 

   
    RaycastHit2D[] casts;

    void Start()                
    {
        timeController = GameObject.FindGameObjectWithTag("GameController").GetComponent<dayClock>();
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GC>();
        Destroy(this.gameObject, 25);
        int dir = Random.Range(0,2);
        GoRight();

        
        
        
        
    }

   

    public void GoRight(){

        rb.velocity = transform.right*mSpeed; 
  
    }



    void Update()
    {
        if (type == 1)
        {
            if (!lightsActive && timeController.activateLights)
            {
                lights.SetActive(true);
                lightsActive = true;
            }
            else if (lightsActive && !timeController.activateLights)
            {
                lights.SetActive(false);
                lightsActive = false;
            }
        }


        casts = Physics2D.CircleCastAll(transform.position, 2.5f, Vector2.zero);

       
        for(int i = 0; i<casts.Length; i++){

            if(casts[i].collider.CompareTag("crimeScene")){

             
                if(!escaping){
                    
                    CivilianEscape();

                    escaping = true;

                }   

            }
        }

      
    }

    
    public void CivilianEscape(){

        
        transform.rotation = Quaternion.Euler(0, 180, 0);
        rb.velocity = transform.right * sprintSpeed; 

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = gameController.warningEmote;


         
    }


}
