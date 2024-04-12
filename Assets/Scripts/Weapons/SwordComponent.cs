using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordComponent : MonoBehaviour
{
    [HideInInspector] public float dmg;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IHp hp))
        {
            hp.TakeDmg(dmg);
        }
    }
}
