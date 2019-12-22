using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Puzzle : MonoBehaviour
{
    public Button redirect_login;
    public List<GameObject> pieces;


    // Start is called before the first frame update
    void Start()
    {
        redirect_login.onClick.AddListener(RedLog);
        GameLogic.closing = true;
        for(int i = 0; i < pieces.Count; i++){
            if((i + 1) < GameLogic.GetCurrentArea()){
                pieces[i].SetActive(true);
            }else{
                pieces[i].SetActive(false);
            }
        }
        
    }

    public void RedLog(){
        SceneManager.LoadScene("Login");
    }
    

    // Update is called once per frame
    void Update()
    {}
        
}
