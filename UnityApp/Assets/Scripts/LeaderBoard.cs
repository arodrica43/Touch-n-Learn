using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderBoard : MonoBehaviour
{
    public Slider earth_sl;
    public Slider air_sl;
    public Slider water_sl;
    public Text earth_score;
    public Text air_score;
    public Text water_score;
    public Models.Leaderboard scores = null;
    // Start is called before the first frame update

    void Start()
    {
        GetData();
        while(scores == null){
            //wait
        }
        //earth_sl.value = scores[0];
        //air_sl.value = scores[1];
        //water_sl.value = scores[2];

    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void GetData()
    {
       // string userNameFromInput = user_name.text;
        StartCoroutine(GetRequest("http://explora-app-api.herokuapp.com/leaderboard"));
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
                scores = JsonUtility.FromJson<Models.Leaderboard>(uwr.downloadHandler.text);
                //Debug.Log(scores);
                earth_sl.value = scores.escore;
                air_sl.value = scores.ascore;
                water_sl.value = scores.wscore;
                
                earth_score.text = "" + scores.escore;
                air_score.text = "" + scores.ascore;
                water_score.text = "" + scores.wscore;

                // This gamer may come with all necessary information to start the game on the last point
               // Debug.Log("Recived scores: " + scores.escore + scores.ascore + scores.wscore);
                //Here we should UpdateLogic(current_level, difficulty, current_clothes)
                //So we need to implement a GameData serializable object, in order to assign 
                //to each user a game_data object.
                /*if(gamer.user_name == user_name.text){
                    GameLogic.user.user_name = gamer.user_name;
                    
                    GameLogic.Init(gamer.lvl, gamer.score);
                    SceneManager.LoadScene("Main");
                }*/
            }else{
                Debug.Log("Username not found.");
                //error_txt.text = "User not found";
            }
            
        }
    }

}
