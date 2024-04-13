using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMousPos : MonoBehaviour
{
    Vector3 mousePos;
    Vector3 dir;
    Vector3 pos;
    Camera main;
    float angle;
    public float cameraYOffset = 30;
    Ray ray;

    private void Start()
    {
        main = Camera.main;
    }


    private void Update()
    {
        //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        //pos = transform.position;
        //mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        ////#if UNITY_EDITOR
        ////        Debug.Log("diomerda");
        ////        mousePos = main.ViewportToWorldPoint(Input.mousePosition);
        ////#endif

        //dir = (-mousePos + pos).normalized;
        //angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        ////angle -= 90;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        AimMouse();
    }



    private void AimMouse()
    {
        mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mousePos.y += cameraYOffset;
        dir = mousePos - transform.position;
        dir.z = 0;
        transform.right = dir.normalized;

        //float angle = Mathf.Atan2(dir.y, dir.z) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);

        //avevo anche provato
        // transform.rotation = quaternion.angleAxis(angle, v3.forward) e anche quaternion.euler(new(0, 0, angle))
    }

    private void OnDrawGizmos()
    {
        if(mousePos == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(mousePos, 3f);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,dir);
    }
}

        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
        // avevo anche provato 
        // transform.rotation = quaternion.angleAxis(angle,v3.forward) e anche quaternion.euler(new(0,0,angle))