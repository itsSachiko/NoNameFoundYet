using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("how fast it moves twoards the player")] 
    float camSpeed;
    [SerializeField, Tooltip("distance to player in the z axis")]
    float zOffset = -10;

    [Header("Rotation")]
    [SerializeField, Tooltip("how fast it rotates twoards the player")] 
    float turnSpeed;




    

    Transform player;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerComponent>().transform;
    }

    private void Update()
    {
        RotateToPlayer();
        MoveToPlayer();
    }

    void RotateToPlayer()
    {
        Quaternion orientationDir = Quaternion.RotateTowards(transform.rotation,player.rotation, Time.deltaTime * turnSpeed);
        transform.rotation = orientationDir; //Quaternion.Slerp(transform.rotation, orientationDir, Time.deltaTime * turnSpeed);
    }

    void MoveToPlayer()
    {
        Vector2 dir = (Vector2)transform.position - (Vector2)player.position;

        Vector3 pos = transform.position;
        pos = camSpeed * Time.deltaTime * dir;
        pos.z += zOffset;

        transform.position = pos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(Application.isPlaying)
        {
            Gizmos.DrawRay(transform.position, (Vector2)transform.position - (Vector2)player.position);
        }
    }
#endif

}
