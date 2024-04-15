using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIComponent : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float chance;
    float roll;

    [SerializeField] Bars playerHP;
    [SerializeField] int loseSceneNumber;

    [SerializeField] Image HpBar;
    [SerializeField] Image waveBar;

    [SerializeField] SpriteHolder[] starArray;

    private int starCounter;

    [Header("Panels")]
    [SerializeField] public GameObject winPanel;
    [SerializeField] public GameObject losePanel;
    [SerializeField] public GameObject pausePanel;
    [SerializeField] public GameObject optionPanel;
    [SerializeField] public GameObject chooseWeaponPanel;

    public static Action onClickedWeapon;
    public static Action<Wave> onNewWave;
    public static Action<Bars> onBarUse;

    private void OnEnable()
    {
        PlayerHp.lose += onGameOver;
        Spawner.onWin += onWin;
        Spawner.onLastWave += OnlastWave;
        onBarUse += UpdateBar;
        onNewWave += OnNewWave;
    }

    private void OnNewWave(Wave currentWave)
    {
        float duration = currentWave.waitNextSpawn * currentWave.enemies.Length;
        StartCoroutine(WaveBarUpdate(duration));
    }

    IEnumerator WaveBarUpdate(float duration)
    {
        float currentTime = 0.1f;
        float lerpedValue;
        while (currentTime < duration)
        {
            lerpedValue = Mathf.MoveTowards(0, 1, currentTime / duration);
            waveBar.fillAmount = 1 - lerpedValue;
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    private void UpdateBar(Bars bar)
    {
        bar.barToFill.fillAmount = playerHP.actualBar / playerHP.fullBar;
    }


    void OnlastWave()
    {
        onStarActivated();
        OnChooseWeapon();
    }

    private void OnChooseWeapon()
    {
        chooseWeaponPanel.SetActive(true);
    }

    private void onStarActivated()
    {

        if (starCounter > starArray.Length - 1)
        {
            return;
        }

        starArray[starCounter].spriteRenderer.sprite = starArray[starCounter].onSprite;
        starCounter++;
    }

    public void onOption()
    {
        //tutto il code che si fa nelle opzioni, panel on se viene schiacciato
        //on option gestito come unity event
    }

    public void ClickedWeapon()
    {
        // put a script in the () and then get the weapon :)
        onClickedWeapon?.Invoke();
    }


    public void onReturnOnMainMenu()
    {
        SceneManager.LoadScene(0);

        //da dare alla funzione onclick del bottone del return to main menù :DD

        //anche questo gestito come unity event
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
