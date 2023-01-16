using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emojy : MonoBehaviour
{
    internal float _time_to_off = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if(_time_to_off > 0)
        {
            _time_to_off -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
