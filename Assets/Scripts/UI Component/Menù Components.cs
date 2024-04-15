using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenùComponents : MonoBehaviour
{
    public GameObject optionPanel;
    public GameObject creditsPanel;

    bool isOptionPanelActive = false;
    bool isCreditsPanelActive = false;

    InputComponent inputComponent;

    private void Start()
    {
        inputComponent = FindAnyObjectByType<InputComponent>();
    }
    public void OnPlay()
    {
        InputComponent.onOff -= OnOption;
        InputComponent.onOff -= OnCredits;
        AudioManager.Instance.musicSource.Stop();
        SceneManager.LoadScene(1);
    }

    public void OnOption()
    {
        inputComponent.UpdateOnOffCounter();
        isOptionPanelActive = !isOptionPanelActive;
        optionPanel.SetActive(isOptionPanelActive);
        if (InputComponent.onOffCounter > 1)
        {
            InputComponent.onOff -= OnOption;
            InputComponent.onOff -= OnCredits;
        }

        InputComponent.onOff += OnOption;
        InputComponent.onOff -= OnCredits;
    }

    public void OnCredits()
    {
        inputComponent.UpdateOnOffCounter();
        isCreditsPanelActive = !isCreditsPanelActive;
        creditsPanel.SetActive(isCreditsPanelActive);

        if (InputComponent.onOffCounter > 1)
        {
            InputComponent.onOff -= OnOption;
            InputComponent.onOff -= OnCredits;
        }

        InputComponent.onOff += OnCredits;
        InputComponent.onOff -= OnOption;
    }

    public void OnReturnToMainMenu()
    {
        optionPanel.SetActive(false);
        creditsPanel.SetActive(false);

    }

    private void OnEnable()
    {
        InputComponent.onOff += OnOption;
        InputComponent.onOff += OnCredits;
    }

    private void OnDisable() 
    {
        InputComponent.onOff -= OnOption;
        InputComponent.onOff -= OnCredits;
    }
}
