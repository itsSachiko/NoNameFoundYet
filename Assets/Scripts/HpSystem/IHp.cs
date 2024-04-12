using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHp
{
    public float HP { get; set; }

    public void TakeDmg(float damage);
    public void HpUp(float Heal);
    public void Death();
}
