using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WeaponChoice : MonoBehaviour
{
    [SerializeField] Image sprite;
    [SerializeField] TextMeshProUGUI tmp_Description;
    [SerializeField] Weapons[] weapons;

    int random;

    public Action<Ranged> ChooseRanged;
    public Action<Melee> ChooseMelee;
    [HideInInspector] public Ranged range;
    [HideInInspector] public Melee melee;
    PlayerWeapons playerWeapons;

    private void Awake()
    {
        Spawner.onLastWave += LastWave;
    }
    private void OnEnable()
    {

        ChooseMelee += playerWeapons.GetMeleeWeapon;
        ChooseRanged += playerWeapons.GetRangedWeapon;
    }
    private void OnDisable()
    {
        ChooseMelee -= playerWeapons.GetMeleeWeapon;
        ChooseRanged -= playerWeapons.GetRangedWeapon;
    }
    private void LastWave()
    {
        random = Random.Range(0, weapons.Length);
        Weapons weapon = weapons[random];
        range = weapon as Ranged;
        melee = weapon as Melee;

        if (range == playerWeapons.rangeWeapon || melee == playerWeapons.meleeWeapon)
        {
            LastWave();
            return;
        }

        sprite.sprite = weapon.weaponImg;
        tmp_Description.text = weapon.description;
    }

    public void OnClick()
    {
        if (range)
        {
            ChooseRanged?.Invoke(range);
        }
        else if (melee)
        {
            ChooseMelee?.Invoke(melee);
        }
    }
}
