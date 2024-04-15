using UnityEngine;
using UnityEngine.SceneManagement;

public class UIComponent : MonoBehaviour
{
    
    [SerializeField, Range(0, 1)] float chance;
    float roll;
    

    [SerializeField] Bars playerHP;
    [SerializeField] int loseSceneNumber;

    [SerializeField] SpriteHolder[] starArray;
    
    private int starCounter;

    [Header("Pannels:")]
    [SerializeField] public GameObject pausePanel;
    [SerializeField] public GameObject optionPanel;
    [SerializeField] public GameObject losePanel;
    [SerializeField] public GameObject winPanel;
    [SerializeField] public GameObject ChooseWeaponCanvas;

    private void OnEnable()
    {
        PlayerHp.lose += onGameOver;
        Spawner.onWin += onWin;
        //Spawner.onLastWave += OnStarActivated;
        Spawner.onLastWave += OnLastWave;
    }

    private void OnDisable()
    {
        PlayerHp.lose -= onGameOver;
        Spawner.onWin -= onWin;
        //Spawner.onLastWave += OnStarActivated;
        Spawner.onLastWave -= OnLastWave;
    }
    private void OnStarActivated()
    {

        if (starCounter > starArray.Length - 1)
        {
            return;
        }

        starArray[starCounter].spriteRenderer.sprite = starArray[starCounter].onSprite;
        starCounter++;
    }

    void OnLastWave()
    {
        OnStarActivated();
        OnChooseWeapon();
    }

    void OnChooseWeapon()
    {

    }

    public void onOption()
    {
        //tutto il code che si fa nelle opzioni, panel on se viene schiacciato
        //on option gestito come unity event
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
