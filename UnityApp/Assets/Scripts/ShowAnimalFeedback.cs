using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAnimalFeedback : MonoBehaviour
{
    // Start is called before the first frame update

    public SpriteRenderer rend;
    void Start()
    {
        rend.sprite = GameLogic.animals[GameLogic.selected_animal];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
