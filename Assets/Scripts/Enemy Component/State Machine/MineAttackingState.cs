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
        distanceFromTarget = Vector2.Distance(enemy.transform.position, enemy.playerPrefab.position);

        if (distanceFromTarget > enemy.AttackDistance)
        {
            Debug.Log("gdioughoighdiosgh");
            OnExit(enemy);
        }

        if (distanceFromTarget < melee.range)
        {
            Debug.Log("hiiii");
            //animazione e fa danno
            enemy.anim.SetBool("isRunning", false);
            enemy.anim.SetBool("isAttacking", true);
            enemy.StartCoroutine(enemy.MineTimer(melee));
            
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
