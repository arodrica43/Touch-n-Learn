using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpdateLogic : MonoBehaviour
{   
	public Button close;
    public RectTransform canvas;
    public GameObject dress_canvas;
    public GameObject cam;
    public List<GameObject> blockers;
    public Text txt1;
    public Text txt2;

    private bool blocking = true;

	
	//private List<Reward> rewards;
    // Start is called before the first frame update
    void Start()
    {
        close.onClick.AddListener(Close);
        //canvas.transform.localScale = new Vector3(canvas.transform.localScale.x,canvas.transform.localScale.y,1 );
		/*next.onClick.AddListener(Next);
    	home.onClick.AddListener(Home);		
		home.transform.position = new Vector2(Screen.width/4, Screen.height/4);
		next.transform.position = new Vector2(3*Screen.width/4, Screen.height/4);
		text.transform.position = new Vector2(Screen.width/2, 3*Screen.height/4);
		text.text = "Score " + GameLogic.user.score + "/3";
        GameLogic.user.lvl++;
		
        GameObject rw_instance = Instantiate(Resources.Load<GameObject>("Prefabs/Reward"), new Vector2(Screen.width/2,Screen.height/2), Quaternion.identity);
        rw_instance.transform.SetParent(canvas.transform);
       
        rw_instance.GetComponent<Reward>().txt_quant.text = "" + GameLogic.user.score;
        //        Text n = GameObject.Find("Text");
        //rw_instance.transform.SetParent(scrollable.transform);
    */
        //canvas.transform.position = new Vector3(100,100,-100);
       
        
    }

    // Update is called once per frame
    void Update()
    {
    	
        //canvas.transform.localScale = new Vector3(canvas.transform.localScale.x,canvas.transform.localScale.y,1);   
    }

	public void Close()
    {
        if(GameLogic.current_scene == "Profile"){
            SceneManager.LoadScene("Profile");
        }
        if(GameLogic.dress_selected){
            if(GameLogic.user.lvl % 30 == 0 && blocking){
                blockers[0].SetActive(false);
                blockers[1].SetActive(true);
                blockers[2].SetActive(true);
                blockers[3].SetActive(true);
                blockers[4].SetActive(true);
                txt1.text = "" + GameLogic.user.lvl;
                txt2.text = "" + GameLogic.user.score + " pts";
                blocking = false;
            }else{
                
                GameLogic.user.lvl++;
                GameLogic.user.score++; // Here we update score and lvl if dressing
                //GameLogic.hard_perception = true;
                if(GameLogic.user.score >= 24 && GameLogic.user.score <30){
                    GameLogic.hard_perception = true;
                }else{
                    GameLogic.hard_perception = false;
                }
                /*if(GameLogic.mode == GameLogic.Mode.AVT && GameLogic.user.lvl > 61 && GameLogic.user.lvl <= 180){
                    Debug.Log("Opening Scene: Main");
                    Debug.Log("Data -> Mode: " + GameLogic.mode + ", Level: " + GameLogic.user.lvl);
                    GameLogic.dressing = false;
                    dress_canvas.SetActive(false);
                    cam.SetActive(true);
                }else{*/
                //SceneManager.UnloadSceneAsync("Feedback");
                //Debug.Log("Opening Scene: Main");
                Debug.Log("Data -> Mode: " + GameLogic.mode + ", Level: " + GameLogic.user.lvl);
        
                GameLogic.dressing = false;
                if((GameLogic.user.lvl == 91 || GameLogic.user.lvl == 151 || GameLogic.user.lvl == 241) && !blocking){
                    SceneManager.LoadScene("Puzzle");
                }else{
                     SceneManager.LoadScene(GameLogic.SelectScene());
                } 
            }

        }
    }

	/*public void Next()
    {
		Debug.Log("Opening Scene: " + GameLogic.SelectScene());
		Debug.Log("Data -> Mode: " + GameLogic.mode + ", Level: " + GameLogic.user.lvl);
        SceneManager.LoadScene(GameLogic.SelectScene());
        
    }*/

   
}
