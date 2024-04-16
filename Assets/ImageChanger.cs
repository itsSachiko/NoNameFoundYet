using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    public Sprite nightmareMode;
    public Image myImg;

    private void Start()
    {
        PlayerHp.Nightmare += Nightmere;
    }

    void Nightmere()
    {
        myImg.sprite = nightmareMode;
    }
}
