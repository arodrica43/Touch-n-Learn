using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UserTab : MonoBehaviour
{

    public Models.User user;
    public Button bt;
    public Text name;
    public SpriteRenderer icon;
   // private Login login;
    // Start is called before the first frame update
    void Start()
    {
        //login = new Login();
        name.text = user.user_name;
        icon.sprite = user.getAvatar();
        bt.onClick.AddListener(GetData);
       // login.bt = bt;
       // login.user_name = new InputField(name.text);

    }

    // Update is called once per frame
    void Update()
    {
        //login.Update();
         name.text = user.user_name;
         icon.sprite = user.getAvatar();

    }

    public void GetData()
    {

        StartCoroutine(GetRequest("http://explora-app-api.herokuapp.com/list?key=mykey&user_name=" + user.user_name));
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
                if(gamer.user_name == user.user_name){
                    //GameLogic.user.user_name = gamer.user_name;
                    
                    GameLogic.InitUser(gamer);
                    SceneManager.LoadScene("Main");
                }
            }else{
                Debug.Log("Username not found.");
                //error_txt.text = "User not found";
            }
            
        }
    }
   
}
