using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;
public class explosionLight : MonoBehaviour
{
    public Light2D ll; 
    void Start()
    {
        StartCoroutine(LightDimmer());
    }

    
    IEnumerator LightDimmer()
    {
        while (true)
        {
            ll.intensity -= 0.03f;
            if (ll.intensity <= 0)
            {
                ll.intensity = 0;
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }

    }
}
