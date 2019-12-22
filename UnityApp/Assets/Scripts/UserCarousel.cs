using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UserCarousel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject carousel;
    public RectTransform carouselt;
   // private User userlist;
    public GameObject usertab;
    private bool done = false;
    private List<Models.User> users = null;
     public bool acredited = false;
    
    void Start()
    {
        
        if(!GameLogic.data_init){
			GameLogic.InitData();
            GameLogic.data_init  = true;
		}
        GetData();
        /*while(users == null){
            Debug.Log(users);
        }*/
       
        //  Vector3(0, 3.2f - 2.75f*i -carouselt.sizeDelta.y/2.1225f, 0)
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!done && users != null){
            carouselt.sizeDelta = new Vector2(carouselt.sizeDelta.x, 43.53f*users.Count);
            carouselt.position = new Vector3(0,-carouselt.sizeDelta.y/2.0f,0);
            for(int i = 0; i < users.Count; i++){
                GameObject instance = Instantiate(usertab, new Vector3(0,-carouselt.sizeDelta.y/2.135f - 2.8f*i,0), Quaternion.identity);
                UserTab usertb = instance.GetComponent<UserTab>();
                usertb.user = users[i];
                instance.transform.SetParent(carousel.transform);
                instance.transform.localScale = new Vector3(1,1,1);
            }
            done = true;
        }

    }

     public void GetData()
    {
        Debug.Log("SendRequest");
        StartCoroutine(GetRequest("http://explora-app-api.herokuapp.com/list?key=mykey"));
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();
        //Debug.Log(uwr.downloadHandler.text);
        Models.UserList userlist = JsonUtility.FromJson<Models.UserList>(uwr.downloadHandler.text);
        users = new List<Models.User>();
        GameLogic.num_users = userlist.users.Count;
        for(int i = 0; i < userlist.users.Count; i++){
            users.Add(JsonUtility.FromJson<Models.User>(userlist.users[i]));
            //Debug.Log(JsonUtility.FromJson<Models.User>(userlist.users[i]).user_name);  
        }
        
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
