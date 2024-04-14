using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMousPos : MonoBehaviour
{
    Vector3 mousePos;
    Vector2 dir;
    Vector3 pos;
    [SerializeField] float posCamY;
    Camera main;
    float angle;

    private void Start()
    {
        main = Camera.main;
    }


    private void Update()
    {
        AimMouse();
    }

    private void AimMouse()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0;
        cursorPos.y += posCamY;
        Vector2 direction = cursorPos - transform.position;

        transform.right = direction.normalized;
    }
}
