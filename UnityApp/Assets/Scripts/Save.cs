using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{
    // Start is called before the first frame update

    public Button bt;
    void Start()
    {
        bt.onClick.AddListener(PostData);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PostData()
    {

        //string userNameFromInput = userName.text;
        //int scoreFromInput = Int32.Parse(score.text);
        if(GameLogic.dress_selected){
            string json = JsonUtility.ToJson(GameLogic.user);
            Debug.Log(json);
            StartCoroutine(PostRequest("http://explora-app-api.herokuapp.com/list_save", json));
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
                 //GameLogic.user_name = userName.text;
                 GameLogic.initialized = false;
                 if(GameLogic.closing){
                      //SceneManager.LoadScene("Login");
                 }else{
                    //SceneManager.LoadScene(GameLogic.SelectScene());   
                 }
                
            }else{
                Debug.Log(uwr.downloadHandler.text);
                //error_txt.text = uwr.downloadHandler.text;
            }
        }
    }


}
