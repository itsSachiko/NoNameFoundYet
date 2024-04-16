using System;
using System.Collections;
using UnityEngine;

public class PlayerHp : MonoBehaviour, IHp
{
    public Bars HpBar;

    public static Action lose;

    [SerializeField] SpriteRenderer Estriperenderer;
    [SerializeField] float hitFlashSeconds = 0.3f;

    [SerializeField] float AtWhatHP = 50;

    public static Action Nightmare;

    public float HP
    {
        get => HpBar.actualBar;
        set => SetHp(value);
    }

    void SetHp(float value)
    {
        //StartCoroutine(HitFlash(hitFlashSeconds));
        
        HpBar.actualBar = value;
        
    }

    IEnumerator HitFlash(float wait)
    {
        Color Startcolor = Estriperenderer.color;
        Estriperenderer.color = Color.red;
        yield return new WaitForSeconds(wait);
        Estriperenderer.color = Startcolor;
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
       

        HP-= damage;
        if(HP <= 0)
        {
            Death();
        }
        else if(HP < AtWhatHP)
        {
            Nightmare?.Invoke();
        }
    }

    public void Death()
    {
        //anim morte
        lose?.Invoke();
    }
}
