using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineEnemy : MonoBehaviour, IHp
{
    public float HP { get; set; }

    public void TakeDmg(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Death();
        }
    }

    public void HpUp(float Heal)
    {
        HP += Heal;
    }

    public void Death()
    {
        transform.parent.TryGetComponent<Spawner>(out var parent);
        parent.AddMine(gameObject);
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
