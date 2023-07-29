using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class billboard : MonoBehaviour
{

    GameObject player;
    SpriteRenderer billBoard;
    GC gameController;
    public GameObject badges; 
    public Sprite evilMood, angelMood;
    Sprite billBoardSprite; 
    float height; 
    float width;

    void Start()
    {
        billBoard = GetComponent<SpriteRenderer>();
        billBoardSprite = angelMood; 
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GC>();
        player = GameObject.FindGameObjectWithTag("HERO");
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }

    void Update()
    {
    //    if (gameController.billboardChanged && this.transform.position.x -  player.transform.position.x > width) 
    //    {
            

    //        billBoardSprite = evilMood;
            
    //    }

        billBoard.sprite = billBoardSprite; 
    }
}
