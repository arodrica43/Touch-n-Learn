using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complement : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject dress;
    private bool dressed = false;
    private SpriteRenderer dress_rend;
    public Renderer icon_rend;
    private int dress_idx;
    private Vector2 key;
    private bool yet_dressed;
    
    void Start()
    {
      GameLogic.dress_selected = false;
      dress_idx = int.Parse("" + this.name[1]) - 1;   
      dressed =  GameLogic.user.AccessBag(GameLogic.selected_animal,dress_idx);
      yet_dressed = dressed;
      if(yet_dressed){
        icon_rend.material.color = new Color(icon_rend.material.color.r,icon_rend.material.color.g,icon_rend.material.color.b,0.3f);
      }
      //Debug.Log(dressed);
        
      key = new Vector2(GameLogic.selected_animal + 1, dress_idx);
      foreach(Vector3 sp_key in GameLogic.special_keys){
        Vector2 special_key = new Vector2(sp_key.y, sp_key.z);
        if(special_key == key){
          if(sp_key.x == 0){
            dress.transform.position = new Vector3(dress.transform.position.x, 
                                                    dress.transform.position.y,
                                                    -8.1205f);
          }else{
            dress.transform.position = new Vector3(dress.transform.position.x, 
                                                    dress.transform.position.y,
                                                    -8.121f - 0.00005f*sp_key.x);
          }
        }
      }
      dress_rend = dress.GetComponent<SpriteRenderer>();
      icon_rend.material.mainTexture = GameLogic.dress_icons[key];
      dress_rend.sprite = GameLogic.dresses[key];
      dress.SetActive(GameLogic.user.AccessBag(GameLogic.selected_animal,dress_idx));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
      if(GameLogic.user.lvl > 30*(GameLogic.selected_animal + 1)){
        if(dressed){
          GameLogic.user.BagUpdate(GameLogic.selected_animal,dress_idx, false);
          dress.SetActive(false);
          //GameLogic.dress_selected = false;
          dressed = !dressed;
          icon_rend.material.color = new Color(icon_rend.material.color.r,icon_rend.material.color.g,icon_rend.material.color.b,1);
        }else{
          GameLogic.user.BagUpdate(GameLogic.selected_animal,dress_idx, true);
          dress.SetActive(true);
         // GameLogic.dress_selected = true;
          dressed = !dressed;
          icon_rend.material.color = new Color(icon_rend.material.color.r,icon_rend.material.color.g,icon_rend.material.color.b,0.3f);
        
        }
      }else{
        if(!yet_dressed){
          //dressed = !dressed;
          if(dressed){
            GameLogic.user.BagUpdate(GameLogic.selected_animal,dress_idx, false);
            dress.SetActive(false);
            GameLogic.dress_selected = false;
            dressed = !dressed;
            icon_rend.material.color = new Color(icon_rend.material.color.r,icon_rend.material.color.g,icon_rend.material.color.b,1);
          }else{
            if(!GameLogic.dress_selected){
              GameLogic.user.BagUpdate(GameLogic.selected_animal,dress_idx, true);
              dress.SetActive(true);
              GameLogic.dress_selected = true;
              dressed = !dressed;
              icon_rend.material.color = new Color(icon_rend.material.color.r,icon_rend.material.color.g,icon_rend.material.color.b,0.3f);
            }
          }
        }
      }

    }
}
