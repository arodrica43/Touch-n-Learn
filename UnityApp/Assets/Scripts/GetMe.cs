using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetMe : MonoBehaviour {

    public Button bt;
    
    void Start()
    {
        bt.onClick.AddListener(GetData);    
    }
    public void GetData()
    {
        StartCoroutine(GetRequest("http://explora-app-api.herokuapp.com/list?key=mykey"));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();
        //Debug.Log(uwr.downloadHandler.text);
        Models.UserList gamers = JsonUtility.FromJson<Models.UserList>(uwr.downloadHandler.text);
        Debug.Log(JsonUtility.FromJson<Models.User>(gamers.users[1]).user_name);  
        if (uwr.isNetworkError)
        {
            
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }
        
    }
}
