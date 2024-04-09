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
        enemy.rb.velocity = Vector3.zero;

    }

    public override void OnExit(StateManager enemy)
    {
        enemy.ChangeState(new AttackingState());
    }
}
