using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Models : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [Serializable]
    public class User
    {
        public int user_id;
        public string user_name = "USERNAME";
        public int score = 0;
        public int lvl = 1;
        public string ctype = "AIRE";
        public int mode = 2;
        public int avatar = 0;
        public string bag = "0000000000000000000000000000000000000000000000000000000000000000";
        public string stimuli_history = "";
        public string answers = "";
        public List<List<Vector3>> stimulis = new  List<List<Vector3>>();
        public  List<List<Vector3>> results = new  List<List<Vector3>>();

        public bool AccessBag(int i, int j){
            char v = bag[8*i + j];
            if(v == '1'){
                return(true);
            }else{
                return(false);
            }
        }

        public void setCType(int avt){
            int val = avt%3;
            avatar = avt;
            switch(val){
                case 0:
                    ctype = "TIERRA";
                    break;
                case 1:
                    ctype = "AIRE";
                    break;
                case 2:
                    ctype = "AGUA";
                    break;
            }
        }

        public void setCType(){
            int val = avatar%3;
            switch(val){
                case 0:
                    ctype = "TIERRA";
                    break;
                case 1:
                    ctype = "AIRE";
                    break;
                case 2:
                    ctype = "AGUA";
                    break;
            }
        }

        public Sprite getAvatar(){
           
           return(Resources.Load<Sprite>("Images/avatars/" + avatar));
        }


        public void BagUpdate(int i, int j, bool v){
            string bg = "";
            for(int x = 0; x < bag.Length;x++){
                if(x == 8*i + j){
                    if(v){
                        bg += '1';
                    }else{
                        bg += '0';
                    }
                }else{
                    bg += bag[x];
                }
            }
            bag = bg;
        }

    }

        
    [Serializable]
    public class Leaderboard
    {
        public int escore;
        public int ascore;
        public int wscore;
    }

    [Serializable]
    public class UserList
    {
        public List<string> users;
    }

    [Serializable]
    public class Credentials
    {
        //Hardcoded credentials should be accessed or modified only by a developer with its own credentials
        public string aws_key_id = "AKIAILTSEQ4KD75A6PDA";
        public string aws_key_id_pwd = "VomaHSy/cUOjxv093u+ZASCi/2p6tFA06WRvVWT1";

    }
}
