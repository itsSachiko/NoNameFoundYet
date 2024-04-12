using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineAttackingState : EnemyBaseState
{
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering attacking mode");
    }

    public override void UpdateState(StateManager enemy)
    {
        float distanceFromTarget;
        enemy.rb.velocity = Vector3.zero;

        distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.playerPrefab.position);

        if (distanceFromTarget > enemy.AttackDistance)
        {
            OnExit(enemy);
        }

    }

    public override void OnExit(StateManager enemy)
    {
        enemy.ChangeState(new ChasingState());
    }
}
