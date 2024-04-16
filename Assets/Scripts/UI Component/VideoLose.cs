using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoLose : MonoBehaviour
{
    [SerializeField] private VideoPlayer clip;

    private void Start()
    {
        clip.loopPointReached += OnEndVideo;
    }

    private void OnEndVideo(VideoPlayer source)
    {
        SceneManager.LoadScene(0);
    }
}
