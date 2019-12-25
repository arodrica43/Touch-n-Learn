using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Threading;

public class TouchAction : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audio;
    public AudioClip clip;
    public VideoPlayer video;
    private float t0;
    public Transform square;
    private Vector3 tmp_pos;
    public GameObject rw;
    private bool dragging;
    private bool dragged; 
    public Texture playing;
    public Texture paused;    
    private Renderer rend;
    private bool play = false;
    private bool stimulating = false;
    private bool stimulated = false;
    public bool updating = false;
    private bool done = false;
    private bool nowhere = false;
    private string current_stimuli_key;
    private Vector3 key = new Vector3(0,0,0);
    private IEnumerator coroutine;
    private IEnumerator coroutineb;
    private bool working = false;
    void Start()
    {
        rend = GetComponent<Renderer>();
        GameLogic.update_stimuli = false;
        GameLogic.stimuli_play_count = 0;
        GameLogic.stimulated = false;

       //GameLogic.first_assigned = false;
        coroutine = WaitAndPrint(UnityEngine.Random.Range(0.0f,0.6f));
        StartCoroutine(coroutine);        
        
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            key = GameLogic.SelectStimuli_coord();
            //Debug.Log("Currkey:" +key);
 
            //Debug.Log(key);
            current_stimuli_key = "" + key.x + string.Format("{0:00}",key.y) + key.z;
            //GameLogic.user.stimuli_history += current_stimuli_key;
            
            //bt.onClick.AddListener(Transition);
            // Load .wav to the AudioClip
            if(GameLogic.user.lvl > 60 && GameLogic.user.lvl <= 180){
                
                video.clip = GameLogic.SelectAVStimuli(key);

            }
            audio.clip = GameLogic.SelectAStimuli(key);
            
            rend.material.mainTexture = paused;

            //print("WaitAndPrint " + Time.time);
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator WaitAndPrintUp(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            GameLogic.stimuli_change_count++;
            key = GameLogic.SelectStimuli_coord();
            //Debug.Log(key);
            current_stimuli_key = "" + key.x + string.Format("{0:00}",key.y) + key.z;
            done = false;
           // GameLogic.user.stimuli_history += current_stimuli_key;

            if(GameLogic.user.lvl > 60 && GameLogic.user.lvl <= 180){
                video.clip = GameLogic.SelectAVStimuli(key);
            }
            audio.clip = GameLogic.SelectAStimuli(key);
              
            if(GameLogic.SelectScene() == "PerceptionAX"){
                if(GameLogic.stimuli_change_count == 2){
                        GameLogic.update_stimuli = false;
                }
            }else if(GameLogic.SelectScene() == "PerceptionABX"){
                if(GameLogic.stimuli_change_count == 3){
                        GameLogic.update_stimuli = false;
                }
            }else{
                if(GameLogic.stimuli_change_count == 1){
                        GameLogic.update_stimuli = false;
                }
            }
            //Debug.Log("Stopping..");
            StopCoroutine(coroutineb);
        }
    }





    // Update is called once per frame
    void Update()
    {
           
        if(GameLogic.SelectScene() == "PerceptionAX"){
            if(GameLogic.stimuli_play_count > 1){
                GameLogic.stimulated = true;
                GameLogic.stimuli_play_count = 0;

            }
        }else if(GameLogic.SelectScene() == "PerceptionABX"){
            if(GameLogic.stimuli_play_count > 2){
                GameLogic.stimulated = true;
                GameLogic.stimuli_play_count = 0;
            }
        }else{
            if(GameLogic.stimuli_play_count > 0){
                GameLogic.stimulated = true;
                GameLogic.stimuli_play_count = 0;
            }
        }
        if(play && stimulating){
            tmp_pos = new Vector3(square.transform.position.x, square.transform.position.y, square.transform.position.z);
            GameLogic.stimuli_play_count++;
            stimulating = false;
        }

        if(!done){
            GameLogic.current_stimulis.Enqueue(current_stimuli_key);
            done = true;
        }
        //Debug.Log(current_stimuli_key);
        if(dragging){
        
            square.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(1.0f);
            
            Camera cam = Camera.main;
            float height = 2f * cam.orthographicSize;
            float width = height * cam.aspect;

            if(square.transform.position.x >= width/3.0f - 1.3f*width/20.0f  && square.transform.position.y >= -  1.3f*width/20.0f &&
                square.transform.position.x <= width/3.0f +  1.3f*width/20.0f && square.transform.position.y <= 1.3f*width/20.0f){
                dragging = false;
                dragged = true;
                square.transform.position = new Vector2(width/3.0f ,0); 
                GameLogic.abx_dragged++;
                //should store if it's the first or the second ???? how
                if(GameLogic.user.lvl % 6 == 0){
                     GameLogic.user.answers += current_stimuli_key;

                }
               
            }
        }
        if(GameLogic.answering){
            if(GameLogic.user.lvl % 6 != 0){
                // GameLogic.user.stimuli_history += current_stimuli_key;
            }
            //GameLogic.user.stimuli_history += current_stimuli_key;
            GameLogic.stimuli_save_count++;
            if(GameLogic.SelectScene() == "PerceptionAX"){
                if(GameLogic.stimuli_save_count == 2){
                    GameLogic.answering = false;
                    GameLogic.stimuli_save_count = 0;
                }
            }else if(GameLogic.SelectScene() == "PerceptionABX"){
                if(dragged){
                    GameLogic.user.answers += current_stimuli_key;
                    dragged = false;
                }
                if(GameLogic.stimuli_save_count == 3){
                    GameLogic.answering = false;
                    GameLogic.stimuli_save_count = 0;
                }
            }else{
                if(GameLogic.stimuli_save_count == 1){
                    GameLogic.answering = false;
                    GameLogic.stimuli_save_count = 0;
                }
            }
        }
        //Debug.Log(GameLogic.user.stimuli_history);
        //Debug.Log(GameLogic.user.answers);
        if(GameLogic.update_stimuli && !updating){
            stimulated = false;
            updating = true;
            play = false;
            rend.material.mainTexture = paused;
  
            GameLogic.first_assigned = false;
           
            //StartCoroutine(WaitAndPrint(UnityEngine.Random.Range(0.0f,1.0f)));
            coroutineb = WaitAndPrintUp(UnityEngine.Random.Range(0.0f,0.6f));
            StartCoroutine(coroutineb);

        }

        if(!GameLogic.update_stimuli){
            updating = false;
        }

        if(GameLogic.hard_perception  && audio.time > 5 && GameLogic.stimulated){
            //drag and drop comparision

            foreach (Touch t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                {
                    t0 = Time.fixedTime;
                    tmp_pos = new Vector2(square.position.x, square.position.y);
                }

                if (t0 != null)
                {
                    if (Time.fixedTime - t0 > 2)
                    {
                        if ((t.position - new Vector2(square.position.x, square.position.y)).magnitude < Screen.width/35)
                        {
                            square.position = t.position;
                        }
                        else
                        {
                            square.position = tmp_pos;   
                        }
                        
                        if (t.phase == TouchPhase.Ended)
                        {
                            //t0 = null;
                            if ((t.position - new Vector2(square.position.x, square.position.y)).magnitude < Screen.width/35)
                            {
                                if (GameLogic.DragRewardDist(t.position))//, int.Parse(rw.name)))
                                {
                                    Destroy(rw);
                                    //UpdateRewardList();
                                }
                                else
                                {
                                    square.position = tmp_pos;
                                }
                            }
                        }


                    }
                }
            

            }
        }
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

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if ((t_vec - collider_proj).x <  collider.size.x && (t_vec - collider_proj).y <  collider.size.y)
            {

                audio.Play();
                video.Play();
                if (t.phase == TouchPhase.Ended)
                {
                    audio.Stop();
                    video.Stop();
                }
                break;
            }
            else
            {
                audio.Stop();
                video.Stop();
            }
        }


    }
    
    void OnMouseDown()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        if(!(square.transform.position.x >= width/3.0f - 1.3f*width/20.0f  && square.transform.position.y >= -  1.3f*width/20.0f &&
                square.transform.position.x <= width/3.0f +  1.3f*width/20.0f && square.transform.position.y <= 1.3f*width/20.0f)){
            Vector3 pos = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(1.0f);
            if(play){
                rend.material.mainTexture = paused;
                audio.Pause();
                if(video != null){
                    video.renderMode = VideoRenderMode.RenderTexture;
                    video.Pause();
                }
                play = false;
            }else if(!GameLogic.dressing && !play){
                rend.material.mainTexture = playing;
                if(GameLogic.user.lvl > 60 && GameLogic.user.lvl <= 180){
                    //Depends on mode
                    if(GameLogic.mode == GameLogic.Mode.A){
                        audio.Play();
                    }else if(GameLogic.mode == GameLogic.Mode.AV){
                        video.renderMode = VideoRenderMode.MaterialOverride;
                        video.Play();
                    }
                }else{
                    audio.Play();
                }
                
                play = true;
                if(!stimulated){
                    stimulating = true;
                    stimulated = true;
                }
                
            }
        
            if(GameLogic.hard_perception && !dragging && !GameLogic.dressing && GameLogic.stimulated){
            
                dragging = true;
            
            }
        }
       
    }


    void OnMouseUp()
    {       
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        Vector3 pos = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(1.0f);
        if(GameLogic.hard_perception && dragging && !GameLogic.dressing && GameLogic.stimulated){
         
            dragging = false;
            //square.transform.position = new Vector3(tmp_pos.x, tmp_pos.y, tmp_pos.z);
            if(!(square.transform.position.x >= width/3.0f - 1.3f*width/20.0f  && square.transform.position.y >= -  1.3f*width/20.0f &&
                square.transform.position.x <= width/3.0f +  1.3f*width/20.0f && square.transform.position.y <= 1.3f*width/20.0f)){
                    square.transform.position = tmp_pos;   
            }
        
           
        }
        else if(dragging && pos.x  > Screen.width/2.0f){
            square.transform.position = new Vector2(0,0);
            dragging = false;
        }
           

    }


    public void Transition(){
        
    }
}
