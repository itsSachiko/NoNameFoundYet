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
        Vector3 cursorPos = main.ScreenToWorldPoint(Input.mousePosition);

        if (Physics.Raycast(cursorPos, main.transform.forward, out RaycastHit hit))
        {
            pos = hit.point;
            dir = hit.point - transform.position;
            transform.right = dir;
            Debug.DrawRay(transform.position, dir * 10, Color.red);

        }

        //cursorPos.z = 0;
        //cursorPos.y += posCamY;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(pos, 3);
    }
#endif
}
