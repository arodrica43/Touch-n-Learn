﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Video;
using System.IO;

public static class GameLogic
{
    public enum Mode
    {
        A,
        AV,
        AVT
    };
	
    public static DataController dc =  new DataController();
	public static Models.User user = new Models.User();
    public static int NUM_TYPES = 20;
    public static int REWARD_PACK = 2;
    //public static int current_level = 1;
    public static Mode mode = Mode.AV;
    public static bool built = false;
    //public static Dictionary<int,List<bool>> bag = new Dictionary<int,List<bool>>();  //filled on feedback scene, render in Carousel
	public static Dictionary<int,Dictionary<int,int[]>> reward_dict = new  Dictionary<int,Dictionary<int,int[]>>();  //filled on feedback scene, render in Carousel
	//public static string user_name = "USERNAME";
	//public static int current_score;	
	public static bool initialized = false;
	public static bool data_init = false;
	public static List<Vector2> positions = new List<Vector2>();
	//public static int current_rw_type = 0;
	//public static bool bag_changed = false;
	public static int stimuli_change_count = 0;
	public static int stimuli_play_count = 0;
	public static int stimuli_save_count = 0;
	public static int feedback_count = 0;
	public static int selected_animal = 0;
	public static bool update_stimuli = false;
	public static bool hard_perception = false;
	public static bool dressing = false;
	public static bool closing = false;
	public static bool dress_selected = false;
	public static bool answering = false;
	public static bool stimulated = false;
	public static int abx_dragged = 0; 
	public static string[] phonemes = new[] {"AE", "AA", "AO", "EH", "ER", "IH", "IY", "AO_R", "UW", "UH", "AH"};
	public static string[] types = new[] {"cv", "cvc", "word"};
	public static Dictionary<Vector3,VideoClip> videos = new Dictionary<Vector3,VideoClip>();
	public static Dictionary<Vector3,AudioClip> audios = new Dictionary<Vector3,AudioClip>();
	public static Dictionary<Vector2,Sprite> dresses = new Dictionary<Vector2,Sprite>();  //filled on feedback scene, render in Carousel
	public static Dictionary<Vector2,Texture> dress_icons = new Dictionary<Vector2,Texture>();  //filled on feedback scene, render in Carousel
	public static List<Sprite> animals = new List<Sprite>();
	public static Dictionary<Vector3,List<List<Vector2>>> keypoints = new Dictionary<Vector3,List<List<Vector2>>>();
    public static  List<Vector3> special_keys = new List<Vector3>();
	public static string current_scene;
	public static Queue<string> current_stimulis = new Queue<string>();
	public static string llpath;
	//public static List<Models.User> users = new List<Models.User>();
	public static int num_users;
	public static void InitData()
    {
		InitFeedbackAnimalLayers();
		
		for(int i = 1; i <= 8; i++){
			animals.Add(Resources.Load<Sprite>("Images/animals/A" + i + "/a" + i));
			List<bool> list = new List<bool>(new bool[8]);
			for(int j = 0; j < 8; j++){
				//list[j] = false;
				Vector2 key = new Vector2(i,j);
				//The following is done once per app run
				int val = j + 1;
				dresses[key] =  Resources.Load<Sprite>("Images/animals/A" + i + "/d" + val);		
				dress_icons[key] =  Resources.Load<Texture>("Images/animals/A" + i + "/i" + val);
				//user.BagUpdate(i-1,j,false);
			}
			//user.bag[i - 1].Add(i,list);
		}

		for(int i = 0; i < types.Length; i++){
			for(int j = 0; j < phonemes.Length; j++){
				//string long_path =  Application.dataPath + "/Resources/data/" + types[i] + "/" + phonemes[j] + "/";
				//Debug.Log(long_path);
				//llpath = long_path;
				int k = 1;//Directory.GetFiles(long_path, "*", SearchOption.TopDirectoryOnly).Length;
				string path = "data/" + types[i] + "/" + phonemes[j] + "/" + k ;
				VideoClip next_video = Resources.Load<VideoClip>(path +  "/vid"); 
				AudioClip next_audio = Resources.Load<AudioClip>(path +  "/aud"); 
				while(next_video != null && next_audio != null){
					Vector3 v = new Vector3(i,j,k);
					videos.Add(v,next_video);
					audios.Add(v,next_audio);
					k++;
					path = "data/" + types[i] + "/" + phonemes[j] + "/" + k ;
					next_video = Resources.Load<VideoClip>(path +  "/vid"); 
					next_audio = Resources.Load<AudioClip>(path +  "/aud"); 
				}				
			}
		}
    }


	public static void InitUser(Models.User usr)
    {
		closing = false;
		user = usr;
		mode = ParseMode(user.mode);
		Debug.Log("Im here " + mode);
		user.setCType();
		abx_dragged = 0; 
		feedback_count = (user.lvl - 1) % 6;
		//hard_perception = true;
		//this should be modified so hard_perception iff 2 | 3 in user.answers
		if(GameLogic.user.score >= 24 && GameLogic.user.score <30){
            GameLogic.hard_perception = true;
        }else{
            GameLogic.hard_perception = false;
        }
		//Debug.Log(user.bag);
		LoadLevelPositions();
        Debug.Log("Initializing Game Logic...");
		initialized = true;
    }

	public static Mode ParseMode(int i){
		Mode m;
		switch(i){
			case 0:
				return(Mode.A);
			case 1:
				return(Mode.AV);		
			case 2:
				return(Mode.AVT);
				break;
			default:
				return(Mode.AVT);
				break;
		}
		
	}

    public static string SelectScene()
    {

        return Select(user.lvl);
       
    }

	public static Vector3 SelectStimuli_coord()
    {
		
       //Random rnd = new Random();
        int i = UnityEngine.Random.Range(0,3);
        int j = UnityEngine.Random.Range(0,11);
        int k = UnityEngine.Random.Range(1,3);
        Vector3 key = new Vector3(i,j,k);

		//user.stimuli_history += "" + i + string.Format("{0:00}",j) + k;
		//current_stimulis.Enqueue(key);
        //Debug.Log("" + key[0] + key[1] + key[2]);
        return key;       
    }


	public static VideoClip SelectAVStimuli()
    {
        return GameLogic.videos[SelectStimuli_coord()];       
    }

	public static VideoClip SelectAVStimuli(Vector3 key)
    {
        return GameLogic.videos[key];       
    }

	public static AudioClip SelectAStimuli()
    {
        return GameLogic.audios[SelectStimuli_coord()];       
    }

	public static AudioClip SelectAStimuli(Vector3 key)
    {
        return GameLogic.audios[key];       
    }


	public static int GetCurrentArea(){
		//Debug.Log((GameLogic.user.lvl + 1) / 30 + 1);
		return (GameLogic.user.lvl - 1) / 30 + 1;
	}

	 public static string SelectScene(int k)
    {

        return Select(k);
       
    }

	private static string Select(int lvl){

		string scene = "";

        //if(lvl <= 1){
          //  scene = "SameDifTest";
        //}
        if(lvl <= 30){
			if(!hard_perception){ // We should see the score of the user
				scene = "PerceptionAX";
			}else{
				scene = "PerceptionABX";
			}
            
        }
        else if(lvl <= 60){
            scene = "ProductionA";
        }
        else if(lvl <= 180){
            scene = "Production";
            switch(mode){
                case Mode.A:
                    scene += "A"; 
                    break;
                case Mode.AV:
                    scene += "AV"; 	
                    break;
                case Mode.AVT:
                    scene += "AVT"; 
                    break;
            }
        }else if(lvl <= 210){
            if(!hard_perception){
				scene = "PerceptionAX";
			}else{
				scene = "PerceptionABX";
			}
        }
        else if(lvl <= 240){
            scene = "ProductionA";
        }else{
            scene = "Main";
        }

		current_scene = scene; 
		selected_animal = (lvl - 1) / 30;
		//Debug.Log("Selecting Scene: " + scene);
		//Debug.Log("Data -> Mode: " + mode + ", Level: " + user.lvl);
		//Debug.Log(GameLogic.user.bag);
		
		return scene;

	}


	public static void LoadRewardAssignation()
    {

		TextAsset ffile = Resources.Load<TextAsset>("Metadata/food_assignation");
		TextAsset cfile = Resources.Load<TextAsset>("Metadata/complement_assignation");
		
		// Split the text into an array of strings, cutting wherever there's a tab.
        string[] numberStrings = ffile.text.Split('\n'); 
		
		for(int i = 0; i < numberStrings.Length; i++){
				reward_dict.Add(i,new Dictionary<int,int[]>());
				for(int j = 0; j < numberStrings[i].Split(' ').Length; j++){
					if(j == 0){
						reward_dict[i].Add(0, new int[numberStrings[i].Split(' ').Length]);
					}
					reward_dict[i][0][j] = (int.Parse(numberStrings[i].Split(' ')[j]));
					Debug.Log(i + "," + 0 + "," + j + " ->" + reward_dict[i][0][j]);
				}
		}

		numberStrings = cfile.text.Split('\n'); 
		
		for(int i = 0; i < numberStrings.Length; i++){
				for(int j = 0; j < numberStrings[i].Split(' ').Length; j++){
					if(j == 0){
						reward_dict[i].Add(1, new int[numberStrings[i].Split(' ').Length]);
					}
					reward_dict[i][1][j] = (int.Parse(numberStrings[i].Split(' ')[j]));reward_dict[i][1][j] = (int.Parse(numberStrings[i].Split(' ')[j]));
					Debug.Log(i + "," + 1 + "," + j + " ->" + reward_dict[i][1][j]);
				}	
		}

    }

	public static void LoadLevelPositions()
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


    public static bool DragRewardDist(Vector3 position)//, int c_kind)
    {
		Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;


	
		Vector3 loc = new Vector2(width/3,0);//Camera.main.ViewportToScreenPoint(Camera.main.WorldToViewportPoint( positions[i + 29]) );
		//Debug.Log(Math.Abs(loc.x - position.x) + Math.Abs(loc.y - position.y));
		//Debug.Log(Math.Abs(loc.y - position.y));

		if (Math.Abs(loc.x - position.x) + Math.Abs(loc.y - position.y) < 50)
		{
			/*foreach(int k in reward_dict[i - 1 + 5*(GameLogic.GetCurrentArea() - 1)][current_rw_type]){
				//Debug.Log("" + i + c_kind + k);
					if(c_kind == k && GameLogic.bag[current_rw_type][k] >= GameLogic.REWARD_PACK){
						return true;
					}*/
					 return true;
			//	}
			
		}
				
			//for(int j = 0; j < reward_dict[i - 34][current_rw_type].Length; j++){
			//	if(c_kind == reward_dict[i - 34][current_rw_type][j]){
			//		 return true;
			//	}
			//}
			
		
	

	    return false;
    }

	public static string findex(int i)
    {
        string result;
        if (i < 10)
        {
            result = "00" + i.ToString();
        }
        else if (i < 100)
        {
            result = "0" + i.ToString();
        }
        else
        {
            result = i.ToString();
        }

        return result;
    }



	public static void InitFeedbackAnimalLayers(){
       
	  Debug.Log("Adding special keys for dress layers...");
	
      special_keys.Add(new Vector3(0,1,5));
	  special_keys.Add(new Vector3(1,1,0));
	  special_keys.Add(new Vector3(2,1,1));
	  special_keys.Add(new Vector3(2,1,2));
	  

      special_keys.Add(new Vector3(1,2,5));
      special_keys.Add(new Vector3(1,2,1));
      special_keys.Add(new Vector3(3,2,6));

      special_keys.Add(new Vector3(1,3,0));
      special_keys.Add(new Vector3(2,3,3));
      special_keys.Add(new Vector3(3,3,5));

      special_keys.Add(new Vector3(1,4,3));
      special_keys.Add(new Vector3(2,4,7));
      special_keys.Add(new Vector3(3,4,4));

      special_keys.Add(new Vector3(0,6,0));
      special_keys.Add(new Vector3(1,6,3));
      special_keys.Add(new Vector3(1,6,5));
      special_keys.Add(new Vector3(2,6,2));

      special_keys.Add(new Vector3(1,7,2));
      special_keys.Add(new Vector3(1,7,7));
      special_keys.Add(new Vector3(2,7,1));
      special_keys.Add(new Vector3(2,7,3));

      special_keys.Add(new Vector3(1,8,3));
      special_keys.Add(new Vector3(1,8,7));
      special_keys.Add(new Vector3(21,8,0));
    }


  
}