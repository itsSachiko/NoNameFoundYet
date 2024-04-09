using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : EnemyBaseState
{
    float distanceFromTarget;
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering chasing mode");
    }

    public override void UpdateState(StateManager enemy)
    {
        enemy.dir = enemy.playerPrefab.position - enemy.transform.position;
        enemy.transform.rotation = Quaternion.FromToRotation(enemy.transform.right, enemy.dir);
        enemy.rb.velocity = (enemy.dir.normalized * enemy.speed * Time.deltaTime);

        distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.playerPrefab.position);

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


}
