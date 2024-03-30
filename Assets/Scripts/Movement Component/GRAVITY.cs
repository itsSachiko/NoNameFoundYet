using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GRAVITY : MonoBehaviour // WHAT IS THAT MELODY?
{
    [Header("how strong is the gravity: ")]
    [Tooltip("9.81 is the default cause earth")]
    public float GRAVITATIONALPULL = 9.81f;

    [Header("the mass of the player: ")]
    [Tooltip("default: 1 cause why not")]
    public float MASS = 1f;

    [Header("how fast we rotate twoards the walls")]
    [Tooltip("value should be lower then 2")]
    public float OrientSpeed = 1;

    [Header("The origin of the gravity")]
    [Tooltip("this should be the sphere at the center")]
    public Transform PLANET;
}