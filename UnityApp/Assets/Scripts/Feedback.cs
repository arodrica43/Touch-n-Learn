using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Feedback : MonoBehaviour
{
    public Button same;
    public Button diff;
    public Button info;
    public Button home;
    public Button profile;
    public Transform sameT;
    public Transform diffT;
    public GameObject cardL;
    public GameObject cardR;
    public GameObject cardX;
    public Texture texture;
    public Camera cam = null;
	private bool sme; 
    private bool transition = false;
    float height;
    float width;
    float speed;
    bool cycled = false;
    
    // Start is called before the first frame update
    void Start(){
    
        GameLogic.abx_dragged = 0;
        if(cam == null){
            cam = Camera.main;
        }
		//Debug.Log("my feedback");
        same.onClick.AddListener(Same);
        diff.onClick.AddListener(Diff);
        same.interactable = false;
        diff.interactable = false;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;


        if(GameLogic.hard_perception){
            same.gameObject.SetActive(false);
            diff.gameObject.SetActive(false);
        }else{
            sameT.position = new Vector2(3*width/10.0f,0);
            sameT.localScale = new Vector3(0.9f,0.5f,1);
            diffT.position = new Vector2(3*width/10.0f,-height/8.0f);
            diffT.localScale = new Vector3(0.9f,0.5f,1);
        }
            //same.interactable = true;
            //diff.interactable = true;
            profile.interactable = true;
            info.interactable = true;
            home.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
     
        if(GameLogic.stimulated){
            same.interactable = true;
            diff.interactable = true;
        }else{
            same.interactable = false;
            diff.interactable = false;
        }
        if(GameLogic.abx_dragged == 2 && GameLogic.hard_perception){
            GameLogic.abx_dragged = 0;
            Same();
        }
        Interactability();
        if(transition){
            //rend.material.mainTexture = texture;
            speed+= 0.5f;
            if(GameLogic.hard_perception && cardX != null){
                if(cardL.transform.position.x < -width/1.5f || cardR.transform.position.x < -width/1.5f){
                    cardL.transform.position = new Vector3(width,0,0);
                    cardR.transform.position = new Vector3(cardL.transform.position.x + width/6.0f,0,0);
                    cardX.transform.position = new Vector3(cardR.transform.position.x + width/6.0f,0,0);
                    
                    //speed = 1;
                    cycled = true;
                }
                if(cardL.transform.position.x > - width/6.0f){
                    //speed -= 0.8f;
                    cardL.transform.position += new Vector3(-0.1f*speed,0,0);
                    cardR.transform.position += new Vector3(-0.1f*speed,0,0);
                    cardX.transform.position += new Vector3(-0.1f*speed,0,0);
                    
                }
                if(cardL.transform.position.x <= - width/6.0f){
                    cardL.transform.position += new Vector3(-0.1f*speed,0,0);
                    cardR.transform.position += new Vector3(-0.1f*speed,0,0);
                    cardX.transform.position += new Vector3(-0.1f*speed,0,0);
                    if(cycled){
                        transition = false;
                        cycled = false;
                    }
                }
             }else{
                if(cardR.transform.position.x < -width/1.5f){
                cardL.transform.position += new Vector3(width + 2*cardL.transform.localScale.x,0,0);
                cardR.transform.position += new Vector3(width + 2*cardL.transform.localScale.x,0,0);
                //speed = 1;
                    cycled = true;
                }
                if(cardL.transform.position.x > - width/6.0f){
                    speed -= 0.8f;
                    cardL.transform.position += new Vector3(-0.1f*speed,0,0);
                    cardR.transform.position += new Vector3(-0.1f*speed,0,0);
                }
                if(cardL.transform.position.x <= - width/6.0f){
                    cardL.transform.position += new Vector3(-0.1f*speed,0,0);
                    cardR.transform.position += new Vector3(-0.1f*speed,0,0);
                    if(cycled){
                        transition = false;
                        cycled = false;
                    }
                }
             }
        }
        
    }
    private void Interactability(){
        bool val = !GameLogic.dressing;
        if(GameLogic.stimulated){
            same.interactable = val;
            diff.interactable = val;
        }else{
            same.interactable = false;
            diff.interactable = false;
        }
        profile.interactable = val;
        info.interactable = val;
        home.interactable = val;
            
    }
    public void Same()
    {
        if(!GameLogic.hard_perception){
            GameLogic.user.answers += "S";
        }else{
            GameLogic.answering = true;
        }
       // UpdateScore();
        UpdateAudios();
        RedirectFeedback();
    }

    public void Diff()
    {
       // UpdateScore();
        GameLogic.user.answers += "D";
        UpdateAudios();
        RedirectFeedback();
    }

    private void RedirectFeedback()
    {
        GameLogic.stimulated = false;
         if(GameLogic.SelectScene() == "PerceptionAX"){
           string s1 = GameLogic.current_stimulis.Dequeue();
           string s2 = GameLogic.current_stimulis.Dequeue();
           GameLogic.user.stimuli_history += s1 + s2;
        }else if(GameLogic.SelectScene() == "PerceptionABX"){
            string s1 = GameLogic.current_stimulis.Dequeue();
            string s2 = GameLogic.current_stimulis.Dequeue();
            string s3 = GameLogic.current_stimulis.Dequeue();
            GameLogic.user.stimuli_history += s1 + s2 + s3;
        }else{
            string s1 = GameLogic.current_stimulis.Dequeue();
            GameLogic.user.stimuli_history += s1;
        }
        GameLogic.current_stimulis = new Queue<string>();
        GameLogic.answering = true;
        GameLogic.update_stimuli = true;
        GameLogic.stimuli_change_count = 0;
        if(GameLogic.feedback_count == 5){
           GameLogic.dressing = true;
            SceneManager.LoadScene("Feedback");
            GameLogic.feedback_count = 0;
        }else{
            //GameLogic.user.lvl++;
            GameLogic.feedback_count++;
            GameLogic.user.lvl++;
            GameLogic.user.score++; // Here we update score and lvl if not dressing
        }
       
        
    }

    private void UpdateScore()
    {
        //here we analyze if the answer is correct or not and we distribute an score = 1,2,3 (stars)
        
        GameLogic.user.lvl++;
        //Debug.Log("Level:" + GameLogic.user.lvl);
        GameLogic.user.score++; // by default we state the lowest score, an evaluation of results is missing right now
        if(GameLogic.user.score >= 24 && GameLogic.user.score <30){
            GameLogic.hard_perception = true;
        }else{
            GameLogic.hard_perception = false;
        }
    }
    
    private void UpdateAudios()
    {
        //TODO: Change Audios
        transition = true; 
        GameLogic.update_stimuli = true;  
        speed = 7.5f;
	}
}