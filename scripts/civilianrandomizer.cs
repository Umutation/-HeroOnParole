using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class civilianrandomizer : MonoBehaviour
{
    [SerializeField] GameObject[] maleHeads, fMaleHeads, maleBodies, fMaleBodies, bottoms; 

    GameObject head, body, bottom; 

    void Start()
    {   
        int randGender = Random.Range(0,3); 

        bool isFemale = randGender == 2 ? true : false; 
        int randomIndex;

        if(!isFemale){

            randomIndex = Random.Range(0, maleHeads.Length);

            head = maleHeads[randomIndex];

            randomIndex = Random.Range(0, maleBodies.Length);

            body = maleBodies[randomIndex];



        }else{


            randomIndex = Random.Range(0, fMaleHeads.Length);

            head = fMaleHeads[randomIndex];

            randomIndex = Random.Range(0, fMaleBodies.Length);

            body = fMaleBodies[randomIndex];
        }

        randomIndex = Random.Range(0, bottoms.Length);

        bottom = bottoms[randomIndex];

       

        GameObject bodyComponent = Instantiate(head, transform.position, Quaternion.Euler(0,0,0));

        bodyComponent.transform.SetParent(gameObject.transform);
        
        bodyComponent = Instantiate(body , transform.position,  Quaternion.Euler(0,0,0));

        bodyComponent.transform.SetParent(gameObject.transform);

        bodyComponent = Instantiate(bottom, transform.position, Quaternion.Euler(0,0,0));
        Color newColor; 
        if (randomIndex > 3)
        {
            float H, S, V; 
            newColor = new Color(Mathf.Floor(Random.Range(0, 256))/255, Mathf.Floor(Random.Range(0, 256))/255, Mathf.Floor(Random.Range(0, 256))/255, 1);
            bodyComponent.GetComponent<SpriteRenderer>().color = newColor;
        }

        bodyComponent.transform.SetParent(gameObject.transform);
    }

   


}
