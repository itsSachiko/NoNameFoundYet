using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        PlayerHp.lose += onGameOver;
        Spawner.onWin += onWin;
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
