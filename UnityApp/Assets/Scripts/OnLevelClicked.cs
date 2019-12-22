using UnityEngine;
using System.Collections;
using System;

using UnityEngine.SceneManagement;

public class OnLevelClicked : MonoBehaviour
{
    private Renderer render;
    private CircleCollider2D collider;
    public GameObject circle = null;

    private bool active = false;
    
	private float time_counter = 0;
	private float prev_time = 0;
	private GameObject mark;
    void Start()
    {
        render = GetComponent<Renderer>();
        collider = GetComponent<CircleCollider2D>();
		//Debug.Log(circle);
		
		

    }
  



    void Update()
    {
	   
			if (GameLogic.user.lvl == int.Parse(this.name))
		    {
			
			    if (!active)
			    {
				    mark = Instantiate(Resources.Load<GameObject>("Prefabs/Mark"), this.transform.position, Quaternion.identity);

			    }

			    active = true;
		    
		    }
		    else
		    {
			    if (active)
			    {
				    Destroy(mark);
			    }

			    active = false;
		    }
		    
	    

	  

	    //if active (!!!)
	    
            /*Vector3 tmp = Camera.main.ScreenToWorldPoint(
               new Vector3(Input.mousePosition.x,
                            Input.mousePosition.y,
                            Camera.main.nearClipPlane));
            Vector2 t = new Vector2(tmp.x, tmp.y);
            Vector2 collider_proj = new Vector2(transform.position.x, transform.position.y);
         
            if ((t - collider_proj).magnitude <  collider.radius)
            {
                render.material.color = Color.green;
				Vector3 index = new Vector3(keypoints.curr_word, keypoints.curr_spk, keypoints.curr_speed);
                Statistics.sCount[index] += 0.25f;
                
            }else{
                render.material.color = Color.white;
					
            }
			*/



           // OnTouch();


            //prev_time = Time.unscaledTime;		

    }

    void OnTouch()
    {
	    foreach (Touch t in Input.touches)
	    {
                
		    Vector3 tmp = Camera.main.ScreenToWorldPoint(
			    new Vector3(t.position.x,
				    t.position.y,
				    Camera.main.nearClipPlane));
		    Vector2 t_vec = new Vector2(tmp.x, tmp.y);
		    Vector2 collider_proj = new Vector2(transform.position.x, transform.position.y);
    
		    if ((t_vec - collider_proj).magnitude < collider.radius)
		    {
                
			    render.material.color = Color.green;
			    Play();
					
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

    void OnMouseDown()
    {
	    if (active)
	    {
		    Debug.Log("Here Clicked");
		    Play();
	    }

	  
    }
    
    public void Play()
    {
	    GameLogic.user.lvl = int.Parse(this.name);
	    SceneManager.LoadScene(GameLogic.SelectScene());
    }
}
