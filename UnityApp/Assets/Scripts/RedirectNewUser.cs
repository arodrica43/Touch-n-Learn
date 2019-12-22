using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RedirectNewUser : MonoBehaviour
{
    public Button bt;
    // Start is called before the first frame update
    void Start()
    {
        bt.onClick.AddListener(CreateUser);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateUser(){
        SceneManager.LoadScene("Register");
    }
}
