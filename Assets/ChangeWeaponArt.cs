using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeaponArt : MonoBehaviour
{
    [SerializeField, Tooltip("True = ranged \n"+"False = Melee")] bool Ranged_Melee;
    [SerializeField] Image myImage;

    //public static Action<Sprite> RangedCallBack;
    //public static Action<Sprite> MeleeCallBack;

    private void OnEnable()
    {
        if (Ranged_Melee)
        {
            //RangedCallBack += ChangeImage;
        }
        else
        {
            //MeleeCallBack += ChangeImage;
        }
    }

    private void OnDisable()
    {
        if (Ranged_Melee)
        {
            //RangedCallBack -= ChangeImage;
        }
        else
        {
            //MeleeCallBack -= ChangeImage;
        }
    }

    private void ChangeImage(Sprite image)
    {
        myImage.sprite = image;
    }
}
