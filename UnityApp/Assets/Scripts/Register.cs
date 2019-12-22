using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Button> buttons;
    public Button go;
    public Text error_txt;
    public InputField user_name;
    private bool selected = false;
    public ScrollRect scroll;
    public GameObject textfield;
    private int tmp_idx;


    void Start()
    {
        for(int i = 0; i < buttons.Count; i++){
            buttons[i].onClick.AddListener(SelectAvatar);
        }
        go.onClick.AddListener(PostData);
        //error_txt.text = "" + GameLogic.data_init  + " -- " +  GameLogic.videos;
    }

    // Update is called once per frame
    void Update()
    {
        if(selected){
            for(int i = 0; i < buttons.Count; i++){
                Button bt = buttons[i];
                if(bt.transform.position.x < 1f && bt.transform.position.x > -1f){
                    tmp_idx = i;

                    //bt.transform.localScale = new Vector3(0.3f,0.5f,1);
               
                    
                    if( bt.transform.position.y < 1.5f){
                        bt.transform.position -= new Vector3(0,-0.05f,0);
                    }else{
                       // bt.transform.position = new Vector3(0,1f,0);
                        //selected = false;
                    }
                }else{
                   if( bt.transform.position.x < 100f &&  bt.transform.position.x > -100f){
                        bt.transform.position += bt.transform.position*0.05f;
                    }else{

                    }
                   
                }
            }
        }
        
    }
    


    public void SelectAvatar()
    {
        textfield.SetActive(true);
        scroll.horizontal = false;
        selected = true;
    }

    public void PostData()
    {

        string userNameFromInput = user_name.text;
        //int scoreFromInput = Int32.Parse(score.text);
        if(!user_name.text.Equals("") && !(user_name.text.Contains(" ") && user_name.text.Length < 6)){
             Models.User gamer = new Models.User();
            gamer.user_name = userNameFromInput;
            gamer.mode = GameLogic.num_users % 3;
            gamer.setCType(tmp_idx);// here we select the avatar
            GameLogic.InitUser(gamer);
            string json = JsonUtility.ToJson(gamer);
            Debug.Log(json);
            StartCoroutine(PostRequest("http://explora-app-api.herokuapp.com/list_add", json));
        }
        else{
             error_txt.text = "Invalid Username";
        }
       
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
                 GameLogic.user.user_name = user_name.text;
                 SceneManager.LoadScene("Main");
            }else{
                error_txt.text = uwr.downloadHandler.text;
            }
        }
    }


}
