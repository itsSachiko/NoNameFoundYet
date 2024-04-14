using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIComponent : MonoBehaviour
{
    [SerializeField] public GameObject winPanel;
    [SerializeField, Range(0,1)] float chance;
    float roll;
    [SerializeField] public GameObject losePanel;

    [SerializeField] Bars playerHP;
    [SerializeField] int loseSceneNumber;

    [SerializeField] SpriteHolder[] starArray;

    private int starCounter;

    private void Start()
    {
        PlayerHp.lose += onGameOver;
        Spawner.onWin += onWin;
        Spawner.onLastWave += onStarActivated;
    }

    private void onStarActivated()
    {

        if(starCounter > starArray.Length - 1)
        {
            return;
        }

        starArray[starCounter].spriteRenderer.sprite = starArray[starCounter].onSprite;
        starCounter++;
    }

    private void Update()
    {
        if (playerHP.actualBar <= 0)
        {
            onGameOver();
        }
    }
    public void onWin()
    {  
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }

    public void tryAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void onGameOver()
    {
       
        Time.timeScale = 0f;
        roll = Random.Range(0, 1f);
        
        if (roll < chance)
        {
            SceneManager.LoadScene(loseSceneNumber);
        }

        else
        {
            losePanel.SetActive(true);
        }  
    }


}
