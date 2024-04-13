using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour, IHp
{
    public EnemyBaseState currentState; //reference dello stato attivo nella state machine 

    [Header("Movement Settings")]
    [SerializeField, Range(1, 500)] public float speed;

    [SerializeField, Range(1, 200)] public float AttackDistance;


    [Header("Player Prefab")]
    [SerializeField] public Transform playerPrefab;

    //[Header("Bullet Prefab")]
    //[SerializeField] public Transform bulletPrefab;


    [HideInInspector] public Vector3 dir;
    [HideInInspector] public Rigidbody rb;

    [HideInInspector] public EnemyPull enemyPull;

    public float HP { get; set; }

    public void ChangeState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    void Start()
    {
        playerPrefab = FindObjectOfType<PlayerHp>().transform;
        currentState = new ChasingState();
        currentState.EnterState(this);
        rb = GetComponent<Rigidbody>();


    }

    void Update()
    {
        currentState.UpdateState(this);
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);

        Gizmos.color = Color.blue;
    }
#endif
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
        
    }

    void Death()
    {
        Debug.Log("uwudeath");
    }

    void IHp.Death()
    {
        throw new System.NotImplementedException();
    }
}
