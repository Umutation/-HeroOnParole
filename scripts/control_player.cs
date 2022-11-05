using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class control_player : MonoBehaviour
{
    

    #region Objects
    Transform crimescene; 
    crimescene crimeType; 
    [SerializeField] LayerMask LM;  
    [SerializeField] Camera cam;
    SpriteRenderer sr;
    Vector2 movement;
    Rigidbody2D rb; 
    #endregion


    #region Animation
    [SerializeField] Animator anim; 
    #endregion 

    #region Head;
    [SerializeField] GameObject head; 
    [SerializeField]Animator headAnim;
     SpriteRenderer headSr;
    #endregion

    #region Blood;
    float bloodLevel = 0;
    GameObject bloodController;
    public Animator bloodAnim;
    float bloodIncrement = 1f;


    #endregion


    #region Laser; 
    bool lasering = false, overHeat = false;
    [SerializeField] float laserDistance; 
    public LineRenderer LR; 
    [SerializeField] GameObject eye; 
    Vector3 laser_aim;
    RaycastHit2D[] hitInfo;
    Vector2 eyePos; 
    Vector2 direction;
    Vector3 screenPoint; 
    Vector3 mousePos;
    float heat = 0.0f, heatLimit = 1; 
    public Sprite overHeatFrameImg, heatFrameImg;
    public Image heatFrame, heatBar;
    public Animator heatAnim; 
    #endregion


    #region GameStats
    GC gameController; 
    [SerializeField] float  movement_speed = 4,gravityscale = 1f;
    float groundLevel = -4;
    bool isFlying = false;
    public int civKillCount = 0; 
    #endregion
     

    #region CivilianBlood
    public bool isStunned = false;
    public ParticleSystem bloodDrips;
    public ParticleSystem bloodParticles;
    public ParticleSystem fleshParticles;

    public ParticleSystem boneParticles;
    public GameObject bloodSplatter;
    #endregion

    #region explosion

    public ParticleSystem explosionP;
    #endregion

    #region moneybag
    public ParticleSystem money;
    #endregion

    void Start()
    {
        bloodDrips.Stop();
        sr = GetComponent<SpriteRenderer>();
        headSr = head.transform.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GC>();

        heatBar.fillAmount = 0.0f; 
        
        rb.velocity = transform.right*4;
    }

    public void BloodSplatter(GameObject target)
    {
        int randomSize = Random.Range(0, 2);

        ParticleSystem boneP = Instantiate(boneParticles, new Vector3(target.transform.position.x, target.transform.position.y + 1f, -5), Quaternion.identity);
        ParticleSystem bloodP = Instantiate(bloodParticles, new Vector3(target.transform.position.x, target.transform.position.y+ 0.7f, -5), Quaternion.identity);
        GameObject bloodS = Instantiate(bloodSplatter, new Vector3(target.transform.position.x, target.transform.position.y - 0.6f, -5), Quaternion.identity);
        ParticleSystem fleshP = Instantiate(fleshParticles, new Vector3(target.transform.position.x, target.transform.position.y + 1.5f, -5), Quaternion.identity);

        bloodS.transform.localScale += new Vector3(randomSize, randomSize, 1);

        boneP.Play();
        bloodP.Play();
        fleshP.Play();
        Destroy(bloodS, 7.5f);


    }

    public void Explosion(GameObject target)
    {

        ParticleSystem expP = Instantiate(explosionP, new Vector3(target.transform.position.x, target.transform.position.y, -5), Quaternion.identity);
        expP.Play();
    }


    Vector2 LaserFixer(Vector2 start , Vector2 end){

        float multiplier = 1.1f;

        while(Mathf.Abs(Vector2.Distance(start, end))<laserDistance){
            
            end *= multiplier;  
            

        }

        return end;
    }
  
    void Gravity(){
        
        
       if(!isFlying) transform.Translate(Time.deltaTime * gravityscale * Vector3.down);
    }


   


    IEnumerator OverHeat()
    {
        clearLaser();
        lasering = false;
        heatFrame.sprite = overHeatFrameImg;
        heatAnim.SetBool("overheat", true); 
        yield return new WaitForSeconds(3);

        
        overHeat = false;

        heatFrame.sprite = heatFrameImg;
        heatAnim.SetBool("overheat", false);



    }

    IEnumerator ChargeLaser(){  

        anim.SetBool("isLasering", true);
            
        bloodAnim.SetBool("lasering", true);
       

        headAnim.SetBool("headLasering", true); 

        yield return new WaitForSeconds(0.2f);

        lasering = true;
        StartCoroutine(HeatUp());

    }

    void BloodLevelAdjuster(float bloodLvl)
    {

        if (bloodLvl >= 3)
        {
            bloodDrips.Play();
        }
        else
        {
            bloodDrips.Stop();
        }
        bloodAnim.SetFloat("BLOODLVL", bloodLvl);

    }

    void ShootLaser(Vector3 start, Vector3 end){
        


        LR.SetPosition(0,start);
        LR.SetPosition(1,end);

        hitInfo = Physics2D.LinecastAll(start, end, LM);


        if (hitInfo.Length > 0)
        {
            for(int i =0; i<hitInfo.Length; i++)
            {
                
                if(hitInfo[i].collider.tag=="hostage"){
          
                    CheckHostageKill(hitInfo[i].collider.gameObject);
                }else if(hitInfo[i].collider.tag=="criminal"){

            
                    CheckCriminalKill(hitInfo[i].collider.gameObject);

                }else if(hitInfo[i].collider.tag=="civilian"){
      
                    CivilianKill(hitInfo[i].collider.gameObject);
                }else if(hitInfo[i].collider.tag=="Rocket"){

                    hitInfo[i].collider.GetComponent<rocket>().explode();
                    Destroy(hitInfo[i].collider.gameObject);
                }else if(hitInfo[i].collider.tag == "moneybag")
                {
                    
                    ParticleSystem moneyLoad = Instantiate(money, hitInfo[i].collider.transform.position, Quaternion.identity);
                    Destroy(hitInfo[i].collider.gameObject);
                    gameController.crimesStopped++;
                }
                
               
            }
        }

    }

    void CheckCriminalKill(GameObject criminal){

        
        crimescene = criminal.transform.parent; 
        crimeType = crimescene.GetComponent<sceneOperator>().crimeType;
        if (!crimeType.crimeOnAir)
        {
            BloodSplatter(criminal);
        }
        else
        {
            Explosion(criminal);
        }
        Destroy(criminal.gameObject);
        crimescene.GetComponent<sceneOperator>().crimeType.criminalCount -= 1; 
        if(gameController.CheckCrime(crimescene, crimeType))gameController.crimesStopped++; 
       
    }

    void CheckHostageKill(GameObject hostage){
        
        crimescene = hostage.transform.parent;
        crimeType = crimescene.GetComponent<sceneOperator>().crimeType;
        BloodSplatter(hostage);
        Destroy(hostage.gameObject); 
        gameController.Criminal_Escape(crimescene.transform);
        if(!gameController.CheckCrime(crimescene, crimeType)&&crimescene.GetComponent<sceneOperator>().crimeResolved)gameController.crimesStopped--;
        gameController.ReduceBadge();
        civKillCount++;
    }

    void CivilianKill(GameObject civilian){

        if (civilian.GetComponent<civilianAI>().type == 0)
        {
            BloodSplatter(civilian);
        }
        else
        {
            Explosion(civilian);
        }
        
        
        gameController.ReduceBadge();
        gameController.MassEscape(civilian.transform.position);
        Destroy(civilian.gameObject);
        civKillCount++;
        
    }



    void clearLaser(){

        LR.SetPosition(0, Vector3.zero);
        LR.SetPosition(1, Vector3.zero);
    }



   private void OnTriggerEnter2D(Collider2D other) {
        

        if(other.CompareTag("criminal")){


            CheckCriminalKill(other.gameObject);
            bloodLevel += bloodIncrement;
            BloodLevelAdjuster(bloodLevel);
        }
        else if(other.CompareTag("hostage")){

            CheckHostageKill(other.gameObject);
            bloodLevel += bloodIncrement;
            BloodLevelAdjuster(bloodLevel);


        }
        else if(other.CompareTag("civilian")){

            CivilianKill(other.gameObject);
            bloodLevel += bloodIncrement;
            BloodLevelAdjuster(bloodLevel);

        }



    }

    IEnumerator StopLasering()
    {
        anim.SetBool("isLasering", false);
        bloodAnim.SetBool("lasering", false);

        headAnim.SetBool("headLasering", false);
        yield return new WaitForSeconds(0.2f);
        lasering = false;
        BloodLevelAdjuster(bloodLevel);
        clearLaser();
        StartCoroutine(CoolOff());

    }

    private void FixedUpdate()
    {

        Gravity();
        rb.velocity = movement * movement_speed;

    }

    void Update()
    {
        eyePos = eye.transform.position;
        screenPoint = Input.mousePosition;
        mousePos = cam.ScreenToWorldPoint(screenPoint);
        heatBar.fillAmount = heat;
        #region Clamp
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,cam.transform.position.x-8.5f, 
        cam.transform.position.x + 8.5f), Mathf.Clamp(transform.position.y, groundLevel, cam.transform.position.y+4.2f),
         -1);
        #endregion

        #region Movement

        if(Input.GetKeyDown("space")){
            isFlying = true;                   
            
        }

        if(Input.GetKeyUp("space")){
            
            isFlying = false; 
        }

        if(isFlying){
            transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y+0.01f),  Time.fixedDeltaTime *10000) ;
        }

        #endregion

        if (Input.GetMouseButtonDown(0)&&!overHeat)
        {
            StartCoroutine(ChargeLaser());
          
        }

        if (lasering)
        {

            ShootLaser(new Vector3(eyePos.x, eyePos.y, -5), new Vector3(mousePos.x, mousePos.y, -5));
            
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(StopLasering());
           
        }
    }

    IEnumerator HeatUp()
    {
        while (lasering)
        {
            heat += 0.01f;
            yield return new WaitForSeconds(0.05f);
            
            if (heat > heatLimit)
            {
                overHeat = true;

                StartCoroutine(OverHeat());
               
            }
        }
    }

    IEnumerator CoolOff()
    {
        while (!lasering)
        {
            heat -= 0.01f;
            yield return new WaitForSeconds(0.05f);
       
            if (heat < 0) heat = 0;

        }



    }





}
