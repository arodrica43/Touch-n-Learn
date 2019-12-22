using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCardPos : MonoBehaviour
{
    public GameObject stimuli;
    public GameObject rec;
    public RectTransform  stimuli_transform;
    //public RectTransform  rec_transform;
	public Camera cam2;

    private bool horizontal;

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
		if(cam2 == null){
			cam2 = Camera.main;
		}
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        /*if (Screen.width > Screen.height)
        {
            stimuli_transform.transform.position = new Vector2(-width, 0);
            //rec_transform.transform.position = new Vector2(width/10.0f, -height/10);
            //Camera.main.rect = new Rect(0.8f,0,0.2f,1);
			//cam2.rect = new Rect(0,0,0.8f,1);
        }
        else
        {
            stimuli_transform.transform.position = new Vector2(width/7.0f, height/6);
            //rec_transform.transform.position = new Vector2(Screen.width/2, Screen.height/2);
			//Camera.main.rect = new Rect(0,0,1,0.2f);
			//cam2.rect = new Rect(0,0.2f,1,0.8f);
           
        }*/


        //stimuli.transform.position = new Vector2(50, Screen.height / 5 + 15);
     

       

        
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 tmpPos = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height));
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        /*if (Screen.width > Screen.height)
        {
            stimuli_transform.transform.position = new Vector2(-width/4.0f, 0);
			
            stimuli_transform.localScale = (new Vector3(tmpPos.x/1.3f, 1.2f*tmpPos.y, stimuli_transform.localScale.z));
			//stimuli_transform.sizeDelta = new Vector2(3*Screen.width/4,3*Screen.height/4);
            //rec_transform.transform.position = new Vector2(width/6.0f,0);
			//cam2.rect= new Rect(0.8f,0,0.2f,1);
			//Camera.main.rect  = new Rect(0,0,0.8f,1);
            
        }
        else
        {
            
        	
            stimuli_transform.localScale = new Vector3(tmpPos.x*1.5f, tmpPos.y*1.5f , stimuli_transform.localScale.z);
            stimuli_transform.transform.position = new Vector2(0, 0);
            //rec_transform.transform.position = new Vector2(Screen.width/2, Screen.height/8);
			//cam2.rect = new Rect(0,0,1,0.2f);
			//Camera.main.rect = new Rect(0,0.2f,1,0.8f);		

            //stimuli_transform.transform.position = new Vector2(0, height/6);
            //rec_transform.transform.position = new Vector2(Screen.width/2, -7*Screen.height/8);
           
        }
        */
       
    }
}
