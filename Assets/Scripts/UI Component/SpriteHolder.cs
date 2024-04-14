using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHolder : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    [HideInInspector]public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
