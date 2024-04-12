using UnityEngine;

public class PlayerHp : MonoBehaviour, IHp
{
    public Bars HpBar;
    public float HP
    {
        get => HpBar.actualBar;
        set => HP = HpBar.actualBar;
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
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
