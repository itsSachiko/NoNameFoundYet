using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float dmg;
    [HideInInspector] public float speed;
    [HideInInspector] public bool iexplode;
    [HideInInspector] public float expRange;
    [HideInInspector] public float expDamage;
    [HideInInspector] public Ranged parent;
    [SerializeField] LayerMask notHitbyExplosion;

    IHp hp;

    Vector3 dir;
    Rigidbody rb;

    public void GiveBullet()
    {
        parent.bulletPool.GetBullet(transform);
    }

    private void Start()
    {
        dir = Camera.main.ScreenToViewportPoint(Input.mousePosition) - transform.position;
    }

    private void FixedUpdate()
    {
        rb.velocity = speed * Time.fixedDeltaTime * dir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out hp))
        {
            hp.TakeDmg(dmg);
        }
        else if (iexplode)
        {
            foreach (Collider x in Physics.OverlapSphere(transform.position, expRange, notHitbyExplosion))
            {
                if (x.TryGetComponent(out hp))
                {
                    hp.TakeDmg(expDamage);
                } //IO VADO A CAGARE :3
            }
        }

    }
}
