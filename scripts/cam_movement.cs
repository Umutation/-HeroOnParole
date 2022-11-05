using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class cam_movement : MonoBehaviour
{   

    public float camera_speed = 0; 
    GC gameController;

    Vector2 edges;

    [SerializeField] GameObject escapeLeft, escapeRight;

    void Start()
    {   
        
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GC>();
        transform.GetComponent<Rigidbody2D>().velocity = transform.right * camera_speed; 
    }


    void Update()
    {
        edges = new Vector2(transform.position.x - 10, transform.position.x + 10);



    }

    IEnumerator displayLeave(GameObject direction, GameObject criminal)
    {
        direction.SetActive(true);
        yield return new WaitForSeconds(2);
        direction.SetActive(false);
        Destroy(criminal);
       

    }


    private void OnTriggerExit2D(Collider2D other) {


        if (other.CompareTag("criminal"))
        {

         
            if(other.transform.position.x<=edges.x || other.transform.position.x >=edges.y)
            {

                if (other.transform.position.x <edges.x)
                {
                    StartCoroutine(displayLeave(escapeLeft, other.gameObject));
                }
                else
                {
                    StartCoroutine(displayLeave(escapeRight, other.gameObject));

                }
                gameController.ReduceBadge();
            }
                

            
        }

      

    }


}
