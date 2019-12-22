using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Text debug;
    // Start is called before the first frame update
    void Start()
    {
        if(!GameLogic.data_init){
			GameLogic.InitData();
            GameLogic.data_init  = true;
		}
    }

    // Update is called once per frame
    void Update()
    {
        
        debug.text = "Debug: " + GameLogic.audios.Keys.Count + " -- " + GameLogic.audios.Values.Count;
    }
}
