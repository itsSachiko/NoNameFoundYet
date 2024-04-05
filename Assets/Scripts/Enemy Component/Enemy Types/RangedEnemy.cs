using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour, IHp
{
    public float HP { get ; set; }

    public void TakeDmg(float damage)
    {
        HP -= damage;
    }

    public void HpUp(float Heal)
    {
        HP += Heal;
    }

    public void Death()
    {
        transform.parent.TryGetComponent<Spawner>(out var parent);
        parent.AddRanged(gameObject);
    }
}
