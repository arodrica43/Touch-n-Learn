using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class People : MonoBehaviour
{

    [JsonProperty("face_keypoints_2d")]
    public Single[] keypoints { get; set; }

  
    // Update is called once per frame
    void Update()
    {

    }
}
