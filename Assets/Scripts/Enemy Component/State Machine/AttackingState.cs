using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : EnemyBaseState
{
    public override void EnterState(StateManager enemy)
    {
        enemy.anim.SetBool("isRunning", false);
        enemy.anim.SetBool("isAttacking", true);
        Debug.Log("entering attacking mode");
    }

    public override void UpdateState(StateManager enemy)
    {
        float distanceFromTarget;
        enemy.rb.velocity = Vector3.zero;

        distanceFromTarget = Vector2.Distance(enemy.transform.position, enemy.playerPrefab.position);

        enemy.StartCoroutine(enemy.ShootCooldown(enemy.myWeapon.realoadTime));

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

