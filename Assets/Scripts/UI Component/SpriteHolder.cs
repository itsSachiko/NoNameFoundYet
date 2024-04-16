using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteHolder : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    [HideInInspector]public Image spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<Image>();
    }
}
