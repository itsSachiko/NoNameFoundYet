using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAttackingState : EnemyBaseState
{
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering attacking mode");
    }

    public override void UpdateState(StateManager enemy)
    {
        float distanceFromTarget;

        if (!enemy.isDashing)
        {
            enemy.dir = enemy.playerPrefab.position - enemy.transform.position;

            float angle = Mathf.Atan2(enemy.dir.y, enemy.dir.x) * Mathf.Rad2Deg;
            enemy.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            enemy.rb.velocity = (enemy.dir.normalized * enemy.dashSpeed * Time.deltaTime);
        }
        else
        {
           
            enemy.rb.velocity = Vector3.zero;
        }


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
