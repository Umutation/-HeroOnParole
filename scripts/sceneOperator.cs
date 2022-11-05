using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneOperator : MonoBehaviour
{

    public crimescene crimeType;
    public bool crimeResolved = false;  
    
    private void Start(){

        
        Destroy(this.gameObject, 35);

    }

    private void Update(){


        if(crimeResolved){

            this.tag = "solvedCrime";
        }
    }
   

 
    
}
