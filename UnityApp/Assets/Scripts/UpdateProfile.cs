using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UpdateProfile : MonoBehaviour
{
    public Button back;
    public Button home;
    public Button info;
    public Image avatar; 
    public Text type;
    public SpriteRenderer type_back;
    public Text name;
    public Text score;
    public List<GameObject> animal_medals;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < GameLogic.GetCurrentArea() - 1; i++){
            animal_medals[i].SetActive(true);
        }
        name.text = GameLogic.user.user_name;
        score.text = "" + GameLogic.user.score;
        type.text = GameLogic.user.ctype;
        type_back.color = SelectColor();
        avatar.sprite = GameLogic.user.getAvatar();

        back.onClick.AddListener(Back);
        home.onClick.AddListener(Home);
        info.onClick.AddListener(Info);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Color SelectColor(){

        Vector3 ret;
        if(GameLogic.user.ctype.Equals("TIERRA")){
            //Color.RGBToHSV(new Color(233, 95, 0), out ret.x, out ret.y, out ret.z);
            return new Color(233f/255f, 95f/255f, 0);
        }else if(GameLogic.user.ctype.Equals("AIRE")){
            return new Color(254f/255f, 178f/255f, 40f/255f);
        }else if(GameLogic.user.ctype.Equals("AGUA")){
            //Color.RGBToHSV(new Color(0, 153, 177), out ret.x, out ret.y, out ret.z);
            return new Color(0, 153f/255f, 177f/255f);
        }else{
            return new Color(10, 10, 10);
        }
    }

    public void Back(){
        SceneManager.LoadScene(GameLogic.SelectScene());
    }
    public void Home(){
        SceneManager.LoadScene("Main");
    }
    public void Info(){
        
    }

}
