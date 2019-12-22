using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dress : MonoBehaviour
{
    public Button go;
    // Start is called before the first frame update
    void Start()
    {
        go.onClick.AddListener(Go);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Go(){
        Debug.Log("Pressing");
        GameLogic.selected_animal = int.Parse(this.name);
        GameLogic.current_scene = "Profile";
        SceneManager.LoadScene("Feedback");
    }

}
