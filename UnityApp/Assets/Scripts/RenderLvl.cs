using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RenderLvl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // find canvas
        GameObject canvas = GameObject.Find("Canvas");
// clone your prefab
        Text text = Instantiate(Resources.Load<Text>("Prefabs/Title"), new Vector3(10, 10, 10), Quaternion.identity);
// set canvas as parent to the text

        text.text = "" + GameLogic.user.lvl;
        text.transform.SetParent(canvas.transform);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
