using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

[CreateAssetMenu(fileName = "civ", menuName = "suppppp/civ", order = 0)]
public class civ : ScriptableObject {
    

    public float speed;
    public int type; // 0   CIV, 1 HELICOPTER
    public GameObject obj; 
    public float yMin= 0, yMax = 0; 

}

