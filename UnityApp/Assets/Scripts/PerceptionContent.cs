using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionContent : MonoBehaviour
{
    public Transform cardL;
    public Transform cardR;
    public Transform cardX;
    public Transform box;
    public Transform icon;

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;


        if(GameLogic.hard_perception){
            cardL.position = new Vector3(-width/3,0,0);
            cardL.localScale = new Vector3(width/7,width/7,1);
            cardR.position = new Vector2(-width/6,0);
            cardR.localScale = new Vector3(width/7,width/7,1);
            cardX.position = new Vector2(0,0);
            cardX.localScale = new Vector3(width/7,width/7,1);
            box.position = new Vector2(width/3,0);
            box.localScale = new Vector3(1.3f*width/7,1.3f*width/7,1);
            icon.position = new Vector2(width/3,-height/4);
            icon.localScale = new Vector3(1.3f*width/7,1.3f*width/16,1);
            
        }else{
            cardL.position = new Vector2(-width/4,-height/13.0f);
            cardL.localScale = new Vector3(width/5,width/5,1);
            cardR.position = new Vector2(0,-height/13.0f);
            cardR.localScale = new Vector3(width/5,width/5,1);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}