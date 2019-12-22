using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Data : MonoBehaviour
{
    // Start is called before the first frame update
    [JsonProperty("people")]
    public People[] people { get; set; }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
