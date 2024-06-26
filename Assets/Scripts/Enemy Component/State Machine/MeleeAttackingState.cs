using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackingState : EnemyBaseState
{
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering attacking mode");
        enemy.anim.SetBool("isRunning", false);
        enemy.anim.SetBool("isAttacking", true);
    }

    public override void UpdateState(StateManager enemy)
    {
        
        float distanceFromTarget;
        enemy.rb.velocity = Vector3.zero;

        distanceFromTarget = Vector2.Distance(enemy.transform.position, enemy.playerPrefab.position);



        enemy.StartMeleeAttack();

        if (distanceFromTarget > enemy.AttackDistance)
        {
            OnExit(enemy);
        }

    }

    public override void OnExit(StateManager enemy)
    {
        enemy.ChangeState(new ChasingState());
    }

    public override void OnCollision(StateManager enemy, Collider collider)
    {

    }
}
