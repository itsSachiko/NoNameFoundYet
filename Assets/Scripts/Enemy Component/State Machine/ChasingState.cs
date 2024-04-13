using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : EnemyBaseState
{
    float distanceFromTarget;
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering chasing mode");
        enemy.anim.SetBool("isRunning", true);
        enemy.anim.SetBool("isAttacking", false);
    }

    public override void UpdateState(StateManager enemy)
    {
        enemy.dir = enemy.playerPrefab.position - enemy.transform.position;
        float angle = Mathf.Atan2(enemy.dir.y, enemy.dir.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        enemy.rb.velocity = (enemy.dir.normalized * enemy.speed * Time.deltaTime);

        distanceFromTarget = Vector2.Distance(enemy.transform.position, enemy.playerPrefab.position);

        if (distanceFromTarget <= enemy.AttackDistance)
        {
            OnExit(enemy);
        }
    }

    public override void OnExit(StateManager enemy)
    {
        if (enemy.CompareTag("Ranged"))
        {
            enemy.ChangeState(new AttackingState());
        }

        else if (enemy.CompareTag("Mine"))
        {
            enemy.ChangeState(new MineAttackingState());
        }

        else if (enemy.CompareTag("Melee"))
        {
            enemy.ChangeState(new MeleeAttackingState());
        }

        else if (enemy.CompareTag("Charger"))
        {
            enemy.ChangeState(new ChargerAttackingState());
        }
    }

    public override void OnCollision(StateManager enemy, Collider collider)
    {

    }


}
