using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bildboarding : MonoBehaviour
{
    Camera Cam;
    Vector3 dir;

    private void Awake()
    {
        Cam = Camera.main;
    }

    private void Update()
    {

        dir = Cam.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);

    }
}
