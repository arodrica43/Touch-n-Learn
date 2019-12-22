using UnityEngine;
using System.Collections;
using System;

using UnityEngine.SceneManagement;

public class TurnColorScript : MonoBehaviour
{
    private Renderer render;
    private CircleCollider2D collider;
    public GameObject circle = null;
    public Camera cam = null;
    private bool active = false;
    
	private float time_counter = 0;
	private float prev_time = 0;
	private GameObject mark;
    void Start()
    {
        foreach(Camera c in Camera.allCameras){
            if(c.gameObject.name == "CamX"){
                cam = c;
            }
        }
        if(cam == null){
            //Debug.Log("Main cam...");
            cam = Camera.main;
        }
	    
        render = GetComponent<Renderer>();
        collider = GetComponent<CircleCollider2D>();
		//Debug.Log(circle);
		
		

    }
  



    void Update()
    {
            Vector3 tmp = cam.ScreenToWorldPoint(
               new Vector3(Input.mousePosition.x,
                            Input.mousePosition.y,
                            cam.nearClipPlane));
            Vector2 t = new Vector2(tmp.x, tmp.y);
            Vector2 collider_proj = new Vector2(transform.position.x, transform.position.y);
         
            if ((t - collider_proj).magnitude <  collider.radius)
            {
                render.material.color = new Color(116.0f/255,221.0f/255,137.0f/255,1.0f);
				//Vector3 index = new Vector3(keypoints.curr_word, keypoints.curr_spk, keypoints.curr_speed);
//                Statistics.sCount[index] += 0.25f;
                
            }else{
                render.material.color = Color.white;
					
            }
			



            OnTouch();


            //prev_time = Time.unscaledTime;		

    }

    void OnTouch()
    {
	    foreach (Touch t in Input.touches)
	    {
                
		    Vector3 tmp = cam.ScreenToWorldPoint(
			    new Vector3(t.position.x,
				    t.position.y,
				    cam.nearClipPlane));
		    Vector2 t_vec = new Vector2(tmp.x, tmp.y);
		    Vector2 collider_proj = new Vector2(transform.position.x, transform.position.y);
    
		    if ((t_vec - collider_proj).magnitude < collider.radius)
		    {
                
			    render.material.color = new Color(116.0f/255,221.0f/255,137.0f/255,1.0f);
//			    Play();
					
			    if (t.phase == TouchPhase.Ended)
			    {
				    render.material.color = Color.white;
			    }
			    break;
		    }
		    else
		    {
			    render.material.color = Color.white;
		    }
	    }


    }

}
