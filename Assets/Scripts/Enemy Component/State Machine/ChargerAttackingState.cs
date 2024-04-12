using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerAttackingState : EnemyBaseState
{
    float distanceFromTarget;
    float dashTimer;
    float delayTimer;
    Vector3 dashVelocity; 
    public override void EnterState(StateManager enemy)
    {
        Debug.Log("entering attacking mode");
        enemy.dir = enemy.playerPrefab.position - enemy.transform.position;
        
        float angle = Mathf.Atan2(enemy.dir.y, enemy.dir.x) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        dashVelocity = Vector3.zero;

    }

    public override void UpdateState(StateManager enemy)
    {
        if (enemy.dashDelay > delayTimer)
        {
            enemy.rb.velocity = Vector3.zero;   
            delayTimer += Time.deltaTime;
            enemy.dir = enemy.playerPrefab.position - enemy.transform.position;
            float angle = Mathf.Atan2(enemy.dir.y, enemy.dir.x) * Mathf.Rad2Deg;
            enemy.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            dashVelocity = (enemy.dir.normalized * enemy.dashSpeed);
        }

        else if (enemy.isDashing == false)
        {
            
            if (enemy.timer < enemy.dashCooldown)
            {
                enemy.timer += Time.deltaTime;
            }

            else
            {
                enemy.timer = 0;

                enemy.dir = enemy.playerPrefab.position - enemy.transform.position;
                enemy.rb.velocity = dashVelocity;
                float angle = Mathf.Atan2(enemy.dir.y, enemy.dir.x) * Mathf.Rad2Deg;
                enemy.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                enemy.isDashing = true;
                delayTimer = 0f;

            }
        }
        else
        {
            enemy.rb.velocity = dashVelocity;
            if (dashTimer < enemy.dashDuration)
            {
                dashTimer += Time.deltaTime;
            }
            else
            {
                enemy.rb.velocity = Vector3.zero;
                dashTimer = 0f;
                enemy.isDashing = false;
            }
        }

        distanceFromTarget = Vector3.Distance(enemy.transform.position, enemy.playerPrefab.position);

        if (distanceFromTarget > enemy.AttackDistance && enemy.isDashing == false) 
        {
            OnExit(enemy);
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
