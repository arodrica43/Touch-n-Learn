using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LevelMaker : MonoBehaviour
{

    public List<Vector2> positions = new List<Vector2>();
    public Button play;
	private Text text;
	private List<Text> texts = new List<Text>();
 	public GameObject canvas;
    public static Vector2 nextPos = new Vector2(0,0);
    private int count = 1;

    void Start()
    {

        LoadLevelPositions();


        
       
        for(int i=0; i < positions.Count ; i++)
        {

	        if (i < 30)
	        {
		        CreateButton(i);
	        }
	        else
	        {
		       // CreateAnimal(i - 29);
	        }

        }
       
     
    }

	void Update()
    {
		
		foreach(Text txt in texts){
			
			float aspect = Camera.main.aspect;
            if(aspect <= 1){
                txt.transform.localScale = new Vector3(aspect, aspect,0)*1.1f; 
            }else{
                txt.transform.localScale = new Vector3(1/aspect, 1/aspect,0)*1.1f; 
            }
		}
	
		
       
     
    }


	public void CreateAnimal(int count)
	{
		nextPos = positions[count + 29];
		GameObject animal = Instantiate(Resources.Load<GameObject>("Prefabs/Animal"), nextPos, Quaternion.identity);
	
		
    
		//Vector3 pos = level.transform.position;
		//GameObject lvl_id = Instantiate(Resources.Load<GameObject>("Prefabs/Title"), new Vector3(nextPos.x + 100, nextPos.y + 100, 0) , Quaternion.identity);
		
		//Canvas level_id = level.GetComponent<Canvas>();
		//Debug.Log(lvl_id);	

		
		//txt.text = "" + count;
		//int val = 30 * (GameLogic.GetCurrentArea() - 1) + count - 1;
		int val = count + 5 * (GameLogic.GetCurrentArea() - 1);
		animal.name = "A" + val;
	}

	public void CreateButton(int count)
    {
	    
	    int val = 30*(GameLogic.GetCurrentArea() - 1) + count + 1;

        nextPos = positions[count];
        GameObject level = Instantiate(Resources.Load<GameObject>("Prefabs/Circle"), nextPos, Quaternion.identity);
                // find canvas
        //canvas = GameObject.Find("Canvas");
			// clone your prefab
			//level.transform.SetParent(canvas.transform);

		Vector3 pos = canvas.transform.position;
		//Vector3 scale = level.transform.lossyScale;
		
		Vector3 loc = Camera.main.ViewportToScreenPoint(Camera.main.WorldToViewportPoint( new Vector3(nextPos.x,nextPos.y,0))) ;
        text = Instantiate(Resources.Load<Text>("Prefabs/Title"), loc, Quaternion.identity);
			// set canvas as parent to the text
		//int val = 30*(GameLogic.GetCurrentArea() - 1) + count + 1;
        text.text = ""  + val;
		text.transform.localScale = canvas.transform.localScale;
        text.transform.SetParent(canvas.transform);

		texts.Add(text);
		  
    
		//Vector3 pos = level.transform.position;
		//GameObject lvl_id = Instantiate(Resources.Load<GameObject>("Prefabs/Title"), new Vector3(nextPos.x + 100, nextPos.y + 100, 0) , Quaternion.identity);
		
		//Canvas level_id = level.GetComponent<Canvas>();
		//Debug.Log(lvl_id);	

		
		//txt.text = "" + count;
		//int val = 30 * (GameLogic.GetCurrentArea() - 1) + count - 1;
        level.name = "" + val;
		//level_id.text = "" + count;
        
	}
      
    
      
    public static void Play()
    {
		SceneManager.LoadScene(GameLogic.SelectScene());
       
    }

    public void LoadLevelPositions()
    {

		TextAsset file = Resources.Load<TextAsset>("Metadata/area" + GameLogic.GetCurrentArea());
		
		// Split the text into an array of strings, cutting wherever there's a tab.
        string[] numberStrings = file.text.Split('\n'); 
		string[] vect = new string[2];
		List<Vector2> psts = new List<Vector2>();

		for(int i = 0; i < numberStrings.Length; i++){
	
			if( numberStrings[i].Split(' ').Length == 2){
				vect[0] = numberStrings[i].Split(' ')[0];
                vect[1] = numberStrings[i].Split(' ')[1];
                positions.Add(new Vector2(float.Parse(vect[0]),float.Parse(vect[1])));

			}
			
		}
       
    }

   


}
