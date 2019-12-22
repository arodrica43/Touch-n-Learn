using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<GameObject> steps1;
    public List<GameObject> steps2;
    public List<GameObject> steps3;
    public List<GameObject> steps4;
    public List<GameObject> steps5;
    public List<GameObject> steps6;
    public List<GameObject> steps7;
    public List<GameObject> steps8;

    public List<GameObject> bases;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

        if(GameLogic.GetCurrentArea() > 1){
            bases[0].SetActive(true);
            foreach(GameObject s in steps1){
                s.SetActive(true);
            }
        }
         if(GameLogic.GetCurrentArea() > 2){
             bases[1].SetActive(true);
            foreach(GameObject s in steps2){
                s.SetActive(true);
            }
        }
         if(GameLogic.GetCurrentArea() > 3){
             bases[2].SetActive(true);
            foreach(GameObject s in steps3){
                s.SetActive(true);
            }
        }
         if(GameLogic.GetCurrentArea() > 4){
             bases[3].SetActive(true);
            foreach(GameObject s in steps4){
                s.SetActive(true);
            }
        }
         if(GameLogic.GetCurrentArea() > 5){
             bases[4].SetActive(true);
            foreach(GameObject s in steps5){
                s.SetActive(true);
            }
        }
         if(GameLogic.GetCurrentArea() > 6){
             bases[5].SetActive(true);
            foreach(GameObject s in steps6){
                s.SetActive(true);
            }
        }
         if(GameLogic.GetCurrentArea() > 7){
             bases[6].SetActive(true);
            foreach(GameObject s in steps7){
                s.SetActive(true);
            }
        }
         if(GameLogic.GetCurrentArea() > 8){
             bases[7].SetActive(true);
            foreach(GameObject s in steps8){
                s.SetActive(true);
            }
        }
        List<GameObject> steps;
        
        switch(GameLogic.GetCurrentArea()){
            case 1:
                steps = steps1;
                break;
            case 2:
                steps = steps2;
                break;
            case 3:
                steps = steps3;
                break;
            case 4:
                steps = steps4;
                break;
            case 5:
                steps = steps5;
                break;
            case 6:
                steps = steps6;
                break;
            case 7:
                steps = steps7;
                break;
            case 8:
                steps = steps8;
                break;
            default:
                steps = new List<GameObject>();
                break;
            
        }
  
        
        for(int i = 0; i <steps.Count; i++){
            if(i < ((GameLogic.user.lvl % 30 + 1) * steps.Count)/30){
                steps[i].SetActive(true);
            }else{
                steps[i].SetActive(false);
            }
        }
        
    }

 
}
