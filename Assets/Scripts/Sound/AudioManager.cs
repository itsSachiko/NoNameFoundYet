using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        if (!PlayerPrefs.HasKey("music volume"))
        {
            PlayerPrefs.SetFloat("music volume", musicSource.volume);

        }

        if (!PlayerPrefs.HasKey("sfx volume"))
        {
            PlayerPrefs.SetFloat("sfx volume", sfxSource.volume);
        }
    }

    private void Start()
    {
        PlayMusic("menu");
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("sig, designer hai dimenticato di aggiungere il file della musica in inspector");
            return;
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("sig, designer hai dimenticato di aggiungere il file della musica in inspector");
            return;
        }
        else
        {
            Debug.Log("MOOOOOSECA");
            sfxSource.PlayOneShot(s.clip);

        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        //volume = 20 * Mathf.Log10(volume);
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("music volume", musicSource.volume);
    }

    public void SFXVolume(float volume)
    {
        //volume = 20 * Mathf.Log10(volume);
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("sfx volume", sfxSource.volume);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLoadScene;
    }

    private void OnLoadScene(Scene arg0, LoadSceneMode arg1)
    {
        PlayMusic("theme");
    }
}


