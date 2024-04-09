using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMousPos : MonoBehaviour
{
    Vector3 mousePos;
    Vector2 dir;
    Vector3 pos;
    [SerializeField] float speed;
    Camera main;
    float angle;

    private void Start()
    {
        main = Camera.main;
    }


    private void Update()
    {
        pos = transform.position;
        mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - pos;
        dir = dir.normalized;
        angle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);

    }
}
