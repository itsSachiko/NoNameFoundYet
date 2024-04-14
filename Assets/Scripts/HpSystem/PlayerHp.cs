using System;
using UnityEngine;

public class PlayerHp : MonoBehaviour, IHp
{
    public Bars HpBar;

    public static Action lose;

    public float HP
    {
        get => HpBar.actualBar;
        set => SetHp(value);
    }

    void SetHp(float value)
    {
        //HP = value;
        HpBar.actualBar = value;
        Debug.Log(HP);
    }

    public void HpUp(float Heal)
    {
        HP += Heal;
        if(HP> HpBar.fullBar)
        {
            HP = HpBar.fullBar;
        }
    }

    public void TakeDmg(float damage)
    {
        Debug.Log("i'm hurting");

        HP-= damage;
        if(HP <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        //anim morte
        lose?.Invoke();
    }
}
