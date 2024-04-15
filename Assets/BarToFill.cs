using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarToFill : MonoBehaviour
{
    [SerializeField] Bars myBar;
    [SerializeField] Image myImage;

    [SerializeField, Tooltip("if the bar is allready full and needs to be emptyed")]
    bool isEmpty;


    private void OnEnable()
    {
        StartCoroutine(CheckBar());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator CheckBar()
    {
        WaitForSeconds wait = new( Time.fixedDeltaTime);
        while (true)
        {
            FillThatBar(myBar);
            yield return wait;
        }
    }

    private void FillThatBar(Bars bar)
    {

        float value = Mathf.Lerp(0, 1, bar.actualBar / bar.fullBar);

        if (isEmpty) // if is full go from 1 to 0 (ex. if value == 1f then the new value would be 9f)
        {
            value = 1 - value;
        }

        myImage.fillAmount = value;

    }
}
