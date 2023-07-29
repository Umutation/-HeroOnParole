using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class dayClock : MonoBehaviour
{
    public Volume vol;
    public float tick, seconds, bgWaitTime = 0.05f;
    public int mins = 0, hours = 0, days = 0, lightsOnTime;
    float H = 0.555f, S = 1  , V = 1;
    public bool activateLights = false, notFirstDay = false; 
    public GameObject[] lights;
    public GameObject curbSun;

    public Image[] bgImgs; 
    void Start()
    {
        vol = GetComponent<Volume>();
       
        StartCoroutine(bgOperator());
    }
     
    IEnumerator bgOperator()
    {
        bool nightOver = false;
        float opacity = 0;
        Image previousImg = bgImgs[0], beginningImg = bgImgs[0]; 
        while (true)
        {

            bgWaitTime = 0.05f;
       
            for(int i = 1; i<bgImgs.Length; i++)
            {
            
                opacity = 0; 
                while (bgImgs[i].color.a < 1)
                {
                    opacity += 0.0025f;
                    bgImgs[i].color = new Color(bgImgs[i].color.r, bgImgs[i].color.g, bgImgs[i].color.b, opacity);
                    if (i == 2 && opacity > 0.5f)
                    {
                        bgWaitTime = 0.02f;
                    }
                    yield return new WaitForSeconds(bgWaitTime);
                }

              
                if (i == 3)
                {
                    bgWaitTime = 0.025f;
                }
                previousImg.color = new Color(previousImg.color.r, previousImg.color.g, previousImg.color.b, 0);
                previousImg = bgImgs[i];
            
            }

            beginningImg.color = new Color(beginningImg.color.r, beginningImg.color.g, beginningImg.color.b, 1f);
            opacity = 1; 
            while (bgImgs[bgImgs.Length - 1].color.a > 0)
            {
                opacity -= 0.0025f;
                bgImgs[bgImgs.Length - 1].color = new Color(bgImgs[bgImgs.Length - 1].color.r, bgImgs[bgImgs.Length - 1].color.g, bgImgs[bgImgs.Length - 1].color.b, opacity);
                yield return new WaitForSeconds(0.05f);


            }

            
             
        }

    }
    public void CalculateTime()
    {

        seconds += Time.fixedDeltaTime * tick;

        if (seconds >= 60)
        {

            seconds = 0;
            mins += 1; 
        }

        if (mins >= 60)
        {
            mins = 0; 
            hours += 1;
           
        }

        if (hours == 24)
        {
            hours = 0;
            notFirstDay = true;
            days += 1;
           
        }

        ControlVol();
    }
    
    void FixedUpdate()
    {

        CalculateTime();
      
   
       
    }

    public void ControlVol()
    {

        if (hours >= 21 && hours <22)
        {
            vol.weight = (float)hours / 24;


            if (!activateLights)
            {
                
                for (int i = 0; i < lights.Length; i++)
                {

                    lights[i].SetActive(true);
                }
                //curbSun.SetActive(false);


                activateLights = true; 
            }

         

        }
        
        if(hours >= 6 && hours < 7 && notFirstDay)
        {

            vol.weight = 1 - (float)mins / 60;

            if (activateLights)
            {
                for (int i = 0; i < lights.Length; i++)
                {

                    lights[i].SetActive(false);
                    
                }
                //curbSun.SetActive(true);
                activateLights = false;

            }
            
        }

    }
}
