using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Video;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class keypoints : MonoBehaviour
{
    public Button record;
    private VideoPlayer vp;
    public static int count = 0;
    public static int click_count = 0;
    private ulong num_frames;
    private Dictionary<int,List<Vector2>> kps = new Dictionary<int,List<Vector2>>();
    private List<GameObject> current_kps = new List<GameObject>();
    public static int curr_spk = 1;
    public static int curr_word = 0;
    public static int curr_speed = 100;
    private Dictionary<Vector2,VideoClip> videos = new Dictionary<Vector2,VideoClip>();
    private int keyp_mode = 0; // todelete
    public static bool paused = true;
    public Camera cam;
    public Button play_mask;
   
    void Start()
    {
        play_mask.onClick.AddListener(VPlay);
        record.onClick.AddListener(Record);
        vp = gameObject.GetComponent<VideoPlayer>();
        UpdateStimuli();
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (Math.Floor(vp.time) < 2){double v = 4 - Math.Floor(vp.time*3);}
            if (count == 0){vp.Prepare(); vp.Play();}
            if (vp.frame >= 0){moveKps((int)vp.frame);}
            count++;
        }
        else{vp.Pause();}
    }

    string findex(int i)
    {
        string result;
        if (i < 10){result = "00" + i.ToString();}
        else if (i < 100){result = "0" + i.ToString();}
        else{result = i.ToString();}
        return result;
    }

    void renderKps(int i)
    {   
        foreach (Vector2 p in kps[i])
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/LipsTouch"), p, Quaternion.identity);
            current_kps.Add(go);
        }
    }

    void moveKps(int i)
    {
        int j = 0;
        foreach (GameObject kp in current_kps)
        {
            Vector2 next_p = kps[i][j];   
            kp.transform.position = new Vector2(next_p.x, next_p.y);
            j++;
        }
    }

    public void Record(){
        click_count++;
        UpdateStimuli();
    }
     public void VPlay(){
        paused = false;
        play_mask.interactable = false;
        GameLogic.stimulated = true;
    }

    public void UpdateStimuli()
    {
        
        if(click_count % 2 == 0){
            paused = true;
            play_mask.interactable = true;
            GameLogic.stimulated = false;
            kps.Clear();
            foreach(GameObject kp in current_kps){Destroy(kp);}
            current_kps.Clear();
            Vector3 key =  GameLogic.SelectStimuli_coord();
            GameLogic.current_stimulis.Enqueue("" + key.x + key.y + key.z);
            vp.clip = GameLogic.SelectAVStimuli(key);
            
            double origh = vp.clip.height; double origw = vp.clip.width;
            double ratio = 1.1*(origw / origh);
            double h = 2f * cam.orthographicSize; double w = h * cam.aspect;
            double vh, vw;
            vh = h;  vw = ratio * vh;
            
            string path;
            for (int i = 0; i < (int)vp.clip.frameCount; i++)
            {
                path = "data/" + GameLogic.types[(int)key.x] + "/" + GameLogic.phonemes[(int)key.y] + "/" + (int)key.z + "/keypoints/vid_000000000";
                List<Vector2> points = GameLogic.dc.parseJSON(path + findex(i));
                for (int j = 0; j < points.Count; j++)
                {
                    Vector2 v = points[j];
                    v.x = (float)(vw / 2) * v.x;
                    v.y = (float)(vh / 2) * v.y;
                    points[j] = v;
                }
                kps.Add(i, points);  
            }
            renderKps(0);
            count = 0;
        }
    }

}
