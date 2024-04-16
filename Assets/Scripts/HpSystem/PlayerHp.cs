using System;
using System.Collections;
using UnityEngine;

public class PlayerHp : MonoBehaviour, IHp
{
    public Bars HpBar;

    public static Action lose;

    [SerializeField] SpriteRenderer Estriperenderer;
    [SerializeField] float hitFlashSeconds = 0.3f;

    public float HP
    {
        get => HpBar.actualBar;
        set => SetHp(value);
    }

    void SetHp(float value)
    {
        StartCoroutine(HitFlash(hitFlashSeconds));
        //HP = value;
        HpBar.actualBar = value;
        Debug.Log(HP);
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
