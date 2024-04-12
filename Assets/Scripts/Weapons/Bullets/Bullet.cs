using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float dmg;
    [HideInInspector] public float speed;
    [HideInInspector] public bool iexplode;
    [HideInInspector] public float expRange;
    [HideInInspector] public float expDamage;
    [HideInInspector] public Ranged parent;
    [HideInInspector] public Transform gunPoint;
    [SerializeField] LayerMask notHitbyExplosion;
    [SerializeField] InputComponent mouse;

    IHp hp;

    public Vector3 dir;
    Rigidbody rb;

    private void OnEnable()
    {
        if (gunPoint)
            dir = gunPoint.forward;
    }

    public void GiveBullet()
    {
        parent.bulletPool.GetBullet(transform);

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = speed * Time.fixedDeltaTime * dir.normalized;
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
                }
                //IO VADO A CAGARE :3
            }
        }

    }
}
