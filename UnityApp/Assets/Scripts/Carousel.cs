using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carousel : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 tmp_pos;
    public List<Button> avatars;
    void Start()
    {
        tmp_pos = avatars[0].transform.position;
        avatars[0].transform.localScale = new Vector3(0.15f,0.27f,1);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = new Vector3(0,0,0);
        float tmp_min = 99999f;
        int tmp_idx = 0;
        if((tmp_pos - avatars[0].transform.position).x < 0.1f && (tmp_pos - avatars[0].transform.position).x > -0.1f ){
           
            
            for(int i = 0; i < avatars.Count; i++){
                if(avatars[i].transform.position.magnitude < tmp_min){
                    tmp_idx = i;
                    tmp_min = avatars[i].transform.position.magnitude;
                    difference = -avatars[i].transform.position;
                    //avatars[i].transform.localScale = new Vector3(0.15f,0.27f,1);
                }
            }
            
            for(int i = 0; i < avatars.Count; i++){
                 avatars[i].transform.position += new Vector3(difference.x,0,0)*0.1f;
            }

        }
        for(int i = 0; i < avatars.Count; i++){
           
            if((avatars[i].transform.position.x)*(avatars[i].transform.position.x) < 1.5f){
                  
                avatars[i].interactable = true;        
                avatars[i].transform.localScale = new Vector3(0.15f,0.27f,1)*(1 + avatars[i].transform.position.y/4);
            }else{
                avatars[i].interactable = false;
                avatars[i].transform.localScale = new Vector3(0.10f,0.19f,1);
            }
        }
        tmp_pos = avatars[0].transform.position;
        
    }
}
