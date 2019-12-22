using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour {

    public Button bt;
    public InputField user_name;
    public Text error_txt;
    
    void Start()
    {
        if(!GameLogic.data_init){
			GameLogic.InitData();
            GameLogic.data_init  = true;
		}
        bt.onClick.AddListener(GetData);  
        
    }
    public void GetData()
    {
        string userNameFromInput = user_name.text;
        StartCoroutine(GetRequest("http://explora-app-api.herokuapp.com/list?user_name=" + user_name.text));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            if(uwr.downloadHandler.text != "USER NOT FOUND"){
                Models.User gamer = JsonUtility.FromJson<Models.User>(uwr.downloadHandler.text);
                // This gamer may come with all necessary information to start the game on the last point
                Debug.Log("Interpreted as: " + gamer.bag);
                //Here we should UpdateLogic(current_level, difficulty, current_clothes)
                //So we need to implement a GameData serializable object, in order to assign 
                //to each user a game_data object.
                if(gamer.user_name == user_name.text){
                    //GameLogic.user.user_name = gamer.user_name;
                    
                    GameLogic.InitUser(gamer);
                    SceneManager.LoadScene("Main");
                }
            }else{
                Debug.Log("Username not found.");
                error_txt.text = "User not found";
            }
            
        }
    }
}
