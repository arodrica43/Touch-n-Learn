using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class BaseExContent : MonoBehaviour
{
    public Button infoB;
    public Button homeB;
    public Button profileB;
    public Text usernameT;
    public Text scoreT;
    //public SpriteRenderer barI;
    public Image progressI;

    public List<GameObject> steps;

    public RectTransform info;
    public RectTransform home;
    public RectTransform profile;
    public RectTransform username;
    public RectTransform score;
    public Transform star;
    public Transform bar;
    public Transform progress;
    public Camera cam = null;

    // Start is called before the first frame update
    void Start()
    {
        GameLogic.current_stimulis = new Queue<string>();
        infoB.onClick.AddListener(Info);
        homeB.onClick.AddListener(Home);
        profileB.onClick.AddListener(Profile);
        profileB.image.sprite = GameLogic.user.getAvatar();


        if(cam == null){
            cam = Camera.main;
        }
         //Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        usernameT.text = GameLogic.user.user_name;
        info.position = new Vector2(6.3f*width/20.0f,8.0f*height/20.0f);
        home.position = new Vector2(8.0f*width/20.0f,8.0f*height/20.0f);
        profile.position = new Vector2(4.6f*width/20.0f,8.0f*height/20.0f);
        username.position = new Vector2(2.3f*width/20.0f,8.0f*height/20.0f);
        //star.position = new Vector2(4.3f*width/20.0f,5.7f*height/20.0f);
        score.position = new Vector2(4.6f*width/20.0f,6.1f*height/20.0f);
        
        bar.position = new Vector3(0,-8.5f*height/20.0f,0);
        bar.localScale = new Vector3(width/1.5f,height/0.8f,1);
      
        
        if(GameLogic.hard_perception){
            progress.position = new Vector2(Screen.width/2.0f,2.4f*Screen.height/9.0f);
            progress.localScale = new Vector2(Screen.width/500.0f,Screen.height/1400.0f);

        }else{
            progress.position = new Vector2(3*Screen.width/4.0f,3.0f*Screen.height/9.0f);
            progress.localScale = new Vector2(Screen.width/500.0f,Screen.height/1400.0f);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
         //update by current_level % 6 -> cases
        scoreT.text = "" + GameLogic.user.score;

        for(int i = 0; i < steps.Count; i++){
            if(i < GameLogic.user.lvl % 30 || (GameLogic.user.lvl %30 == 0 && i < 30)){
                steps[i].SetActive(true);
            }else{
                steps[i].SetActive(false);
            }
        }
    }

    public void Info(){
        //TO DO: Implement Info Scene
        SceneManager.LoadScene("Main");
    }

    public void Home(){
        string scene = SceneManager.GetActiveScene().name;
        if(scene.Equals("Main")){
            SceneManager.LoadScene(GameLogic.SelectScene());   
        }else{
            SceneManager.LoadScene("Main"); 
        }
          
    }

    public void Profile(){
        PostData();
        GameLogic.current_scene = GameLogic.SelectScene();
        SceneManager.LoadScene("Profile");
    }

      public void PostData()
    {

        //string userNameFromInput = userName.text;
        //int scoreFromInput = Int32.Parse(score.text);

        string json = JsonUtility.ToJson(GameLogic.user);
        Debug.Log(json);
        StartCoroutine(PostRequest("http://explora-app-api.herokuapp.com/list_save", json));
    }



    IEnumerator PostRequest(string url, string json)
    {
        
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();
       
        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            if(string.Equals(uwr.downloadHandler.text, "OK")){
                 //GameLogic.user_name = userName.text;
                 //GameLogic.initialized = false;
                 //SceneManager.LoadScene("Login");
            }else{
                Debug.Log(uwr.downloadHandler.text);
                //error_txt.text = uwr.downloadHandler.text;
            }
        }
    }
}
