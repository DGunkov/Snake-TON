using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoExit : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _button_text;
    [SerializeField]
    Grow Grow;

    private float time_to_exit;
    private void OnEnable()
    {
        time_to_exit = 5;
    }
    void Start()
    {
        time_to_exit = 5;
    }

    void Update()
    {
        if(time_to_exit > 0)
        {
            time_to_exit -= Time.deltaTime;
            _button_text.text = "Exit (" + time_to_exit.ToString("0") + ")";
        }
        else
        {
            Grow.PressExit();
        }
    }
}
