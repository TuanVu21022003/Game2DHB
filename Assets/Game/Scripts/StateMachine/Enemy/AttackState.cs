using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    float timer = 0;
    public void OnEnter(EnemyAttack enemy)
    {
        Debug.Log("AttackState");
        enemy.ChangeDirection(enemy.transform.position.x < enemy.Target.transform.position.x);
        enemy.StopMoving();
        enemy.Attack();

    }

    public void OnExcute(EnemyAttack enemy)
    {
        timer += Time.deltaTime;
        if(timer >= enemy.timeDelayAttack)
        {
            enemy.ChangeState(new PartrolState());
        }
    }

    public void OnExit(EnemyAttack enemy)
    {

    }
}
