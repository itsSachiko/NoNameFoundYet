using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BarsToText : MonoBehaviour
{
    public Bars bar;
    public TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        text.text = $"{(int)bar.actualBar}/{(int)bar.fullBar}";
        if(bar.actualBar <= 50f)
        {
            text.color = Color.yellow;
        }
    }
}
