using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Button play;

	
    void Start()
    {

		play.onClick.AddListener(Play);

		
    }

    // Update is called once per frame
    void Update()
    {
	
        
	}

    public void Play()
    {
        //if lvl < k --> "PreTest", "PosTest", "Training" 
	
		int lvl = GameLogic.user.lvl;
		string scene;
		if(lvl <= 1){
			scene = "SameDifTest";
		}
		else if(lvl < 30){
			scene = "PerceptionABX";
		}
		else if(lvl < 60){
			scene = "ProductionA";
		}
		else if(lvl < 180){
			scene = "Production";
			switch(GameLogic.mode){
				case GameLogic.Mode.A:
					scene += "A"; 
					break;
				case GameLogic.Mode.AV:
					scene += "AV"; 	
					break;
				case GameLogic.Mode.AVT:
					scene += "AVT"; 
					break;
			}
		}else if(lvl < 210){
			scene = "Perception";
		}
		else if(lvl < 240){
			scene = "ProductionA";
		}else{
			scene = "Main";
		}

        SceneManager.LoadScene(scene);
    }
}
