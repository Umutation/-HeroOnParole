using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GC : MonoBehaviour
{

    #region Objects
    [SerializeField] Camera cam; 
    [SerializeField] GameObject player; 
    GameObject[] enemies;

    #endregion
    #region MapBoundaries
    float xMin,xMax; 
    float airMin = 3.75f, airMax = 2; 
    float groundLevel = -4f;
    #endregion 

    #region Emotes

    public Sprite warningEmote; 
    public Sprite shockEmote ;

    #endregion

    #region Events
    [SerializeField] GameObject[] crimes; 
    [SerializeField] civ[] civilians; 
    [SerializeField] civ[] aircrafts;
    bool airAttackPresent = false;

    #endregion


    #region Settings

    public int crime_density = 30;
    public int population_density = 5; 
    public int aircraft_density = 10;
    public int criminal_speed = 5;
     
    #endregion

    #region Stats
    public int crimesStopped = 0; 
    public bool gameOver = false; 
    int meters = 0;
    public bool billboardChanged = false; 
    
    #endregion 

    #region CAMERA&EFFECTS
    public Volume effectManager;
    DepthOfField blur;
    Bloom shining;  
    ChromaticAberration ca; 
    #endregion

    #region UI
    [SerializeField] Text metersText; 
    [SerializeField] Image gameOverPanel;
    [SerializeField] GameObject badges;
    [SerializeField] Text solvedCrimesText;
    [SerializeField] Text actualScore, distanceText, crimesStoppedText;
    [SerializeField] GameObject gameStats;
    #endregion

    void Start()
    {
        effectManager.profile.TryGet<Bloom>(out shining);
        effectManager.profile.TryGet<ChromaticAberration>(out ca);
        effectManager.profile.TryGet<DepthOfField>(out blur);

        ca.intensity.value = 0; 
        blur.focalLength.value = 1; 
        StartCoroutine(DistanceTravelled());
    }

    void Update()
    {   
        CheckGameState();
        airAttackPresent = AirAttackCheck();
        
        xMin = cam.transform.position.x+12f; 
        xMax = cam.transform.position.x+17f;
        solvedCrimesText.text = crimesStopped.ToString();

    }

    private void FixedUpdate() {

        #region stunEffects
        if(player.GetComponent<control_player>().isStunned){

            StartCoroutine(StunPlayer());
            shining.intensity.value = Mathf.PingPong(Time.time * 4, 8);
            ca.intensity.value = Mathf.PingPong(Time.time *6,1);
            blur.focalLength.value = Mathf.PingPong(Time.time*2,1);



        }else{

            shining.intensity.value = 3; 
            ca.intensity.value = 0; 
            blur.focalLength.value = 1; 


        }
        #endregion

    }

    public IEnumerator StunPlayer(){

        yield return new WaitForSeconds(5);
        player.GetComponent<control_player>().isStunned = false; 
    }

    IEnumerator DistanceTravelled(){

        
        
        while(true){
            yield return new WaitForSeconds(0.1f);
            meters+=1; 
            metersText.text = meters.ToString(); 
            if(meters%crime_density==0){

                CreateCrime();
            }


            if(meters%population_density==0){

                CreateNPC(civilians); 
            }

            if(meters%aircraft_density==0&&!airAttackPresent){
                
                CreateNPC(aircrafts);

            }
        }

    }

    bool AirAttackCheck(){

        enemies = GameObject.FindGameObjectsWithTag("crimeScene");
        
        for(int i = 0; i<enemies.Length; i++){

            if(enemies[i].GetComponent<sceneOperator>().crimeType.crimeOnAir){

                return true;
            }
        }

        return false;
    }

    void GameOver(){

        gameOverPanel.gameObject.SetActive(true);
        cam.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        gameOver = true;
        player.GetComponent<control_player>().enabled = false;
        StopAllCoroutines();
        actualScore.text = (crimesStopped * 50 + meters).ToString();
        distanceText.text = meters.ToString();
        crimesStoppedText.text = crimesStopped.ToString() + "  X 50";
        gameStats.SetActive(false); 
        this.enabled = false;


    }

    void CheckGameState(){

        if(badges.transform.childCount<=0){

            GameOver();
        }
    }
    public void ReduceBadge(){

        if (!gameOver)
        {
            //Destroy(badges.transform.GetChild(badges.transform.childCount - 1).gameObject);
            //billboardChanged = true; 

        }

    }
    void CreateCrime(){

        int randomIndex = Random.Range(0, crimes.Length);
        GameObject crime = crimes[randomIndex];
        crimescene crimeType = crime.GetComponent<sceneOperator>().crimeType; 

        while(crimeType.requiredMeters>meters){

            randomIndex = Random.Range(0, crimes.Length);
            crime = crimes[randomIndex];
            crimeType = crime.GetComponent<sceneOperator>().crimeType; 
        }
        

        Vector2 crime_location;

        if(crimeType.crimeOnAir){

            CreateEnemyAircraft(crime);
        }else{

            crime_location = new Vector2(Random.Range(cam.transform.position.x+17f, cam.transform.position.x+24), groundLevel);
            Instantiate(crime, crime_location, Quaternion.identity); 
        }
         
        
    }



    void CreateNPC(civ[] category){

        civ NPCtraits = category[Random.Range(0, category.Length)]; 
        float xLoc = Random.Range(xMin, xMax);
        float yLoc = Random.Range(NPCtraits.yMin, NPCtraits.yMax);
        Vector2 randLoc  = new Vector2(xLoc, yLoc); 
        GameObject NPC = Instantiate(NPCtraits.obj, randLoc, Quaternion.identity);
        civilianAI aiscript = NPC.GetComponent<civilianAI>();
        aiscript.mSpeed = NPCtraits.speed;
        aiscript.type = NPCtraits.type; 

        
     





    }

    void CreateEnemyAircraft(GameObject enemy){
        float yLoc = Random.Range(airMin, airMax); 
        float xLoc = Random.Range(xMin, xMax);
        Vector2 randLoc  = new Vector2(xLoc, yLoc); 
        GameObject createdEnemy = Instantiate(enemy, randLoc,  Quaternion.identity);
        createdEnemy.transform.rotation = Quaternion.Euler(0,180, 0);
        airAttackPresent = true; 


    }


    public void MassEscape(Vector2 sLoc){

        RaycastHit2D[] rc = Physics2D.CircleCastAll(sLoc, 2.5f, Vector2.zero); 
        GameObject civilian; 
        for(int i = 0; i<rc.Length; i++){
            
            if(rc[i].collider.CompareTag("civilian")){
                civilian = rc[i].collider.gameObject; 
                civilian.GetComponent<civilianAI>().CivilianEscape();
            }
        }


         
    }


   

    public bool CheckCrime(Transform scene, crimescene type){
        
        
        int hostageCount = 0; 

        for(int k = 0; k<scene.childCount; k++){

            if(scene.GetChild(k).CompareTag("hostage")){
                
                hostageCount++; 
            }
        }

        if(hostageCount<type.hostageCount)return false;

        if (scene.GetComponent<sceneOperator>().crimeType.criminalCount > 0)
        {
            return false; 
        }
           
        
        scene.GetComponent<sceneOperator>().crimeResolved = true; 
        return true; 
        

    }


    public void Criminal_Escape(Transform scene){
        int random_dir = 0; 
        for(int k = 0; k<scene.childCount; k++){

            if(scene.GetChild(k).CompareTag("criminal")){
                random_dir = Random.Range(0,2); 
                if(random_dir==1) scene.GetChild(k).GetComponent<Rigidbody2D>().velocity = scene.GetChild(k).transform.right*criminal_speed; 
                else scene.GetChild(k).GetComponent<Rigidbody2D>().velocity = scene.GetChild(k).transform.right*(criminal_speed*-1);
                scene.GetChild(k).GetChild(0).gameObject.SetActive(true); 
                scene.GetChild(k).GetChild(0).GetComponent<SpriteRenderer>().sprite = warningEmote; 
            }
            
        }

    }
}
