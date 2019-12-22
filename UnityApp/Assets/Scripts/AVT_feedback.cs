using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AVT_feedback : MonoBehaviour
{
    public GameObject canvas;
    public Button close;
    // Start is called before the first frame update
    void Start()
    {
        close.onClick.AddListener(Close);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close(){
        canvas.SetActive(false);
    }
}
