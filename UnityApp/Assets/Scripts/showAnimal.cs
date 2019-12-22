using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showAnimal : MonoBehaviour
{

    public List<GameObject> animals;
    int area0;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        animals[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameLogic.dressing){
            for(int i = 0; i < animals.Count; i++){
                if(animals[i].name.Equals("Animal" + GameLogic.GetCurrentArea())){
                    animals[i].SetActive(true);
                }else{
                    animals[i].SetActive(false);
                }
            }
        }
       
    }
}
