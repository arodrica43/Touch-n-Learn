using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;


public class DataController : MonoBehaviour
{
    public Data data;
    string dataFileName;
    // Start is called before the first frame update
    public List<Vector2> parseJSON(string path) // n shoul be a number in the format 000 ... 999
    {
						//data/speaker1/keypoints/38_B_DASS_62_000000000" + n
        
        List <Vector2> points = new List<Vector2>();
        int count = 0;
        Vector2 tmp_pt = new Vector2();
		int[] kps = new int[]{61,63,65,67};
        TextAsset file = Resources.Load<TextAsset>(path +"_keypoints");
        string dataAsJson = file.ToString();
        data = JsonConvert.DeserializeObject<Data>(dataAsJson);
        
        //Debug.Log(kp.people[0].keypoints[0])
        Single[] raw_data = data.people[0].keypoints;
        for (int i = 3*48; i < 69*3 -1; i++) {
			
            if (i % 3 == 0)
            {
                tmp_pt = new Vector2();
                tmp_pt.x = raw_data[i];
            }
            else if (i % 3 == 1)
            {
                tmp_pt.y = raw_data[i];
            }
            else
            {
				if(i - 2 == 50*3 || i - 2 == 52*3 || i - 2 == 58*3  || i - 2  == 56*3 )
                {
    			        points.Add(tmp_pt);
                    
		        }
            }
        }
        
        return points;

        
    }

 

    
}
