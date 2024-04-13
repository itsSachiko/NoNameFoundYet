using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineAttackingState : EnemyBaseState
{
        float distanceFromTarget;
        
        Melee melee;
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering attacking mode");

        melee = (Melee)enemy.myWeapon;
    }

    public override void UpdateState(StateManager enemy)
    {
        enemy.rb.velocity = Vector3.zero;
        distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.playerPrefab.position);

        if (distanceFromTarget > enemy.AttackDistance)
        {
            OnExit(enemy);
        }

        if (distanceFromTarget < melee.range)
        {
            //animazione e fa danno
            Collider[] enemiesHit = Physics.OverlapSphere(enemy.transform.position, melee.range);

            foreach (Collider collider in enemiesHit)
            {
                if (collider.TryGetComponent(out IHp hp))
                {
                    hp.TakeDmg(enemy.damage);
                }
            }
        }

        else
        {
            //animazione per farlo andare sotto
        } 

    }

    public override void OnExit(StateManager enemy)
    {
        enemy.ChangeState(new ChasingState());
    }

    public override void OnCollision(StateManager enemy)
    {

    }
}
