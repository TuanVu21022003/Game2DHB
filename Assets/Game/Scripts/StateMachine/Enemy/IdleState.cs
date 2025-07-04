using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private float timer;
    private float randomTime;
    public void OnEnter(EnemyAttack enemy)
    {
        timer = 0;
        enemy.StopMoving();
        randomTime = Random.Range(2f, 4f);
    }

    public void OnExcute(EnemyAttack enemy)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            enemy.ChangeState(new PartrolState());

        } 
    }

    public void OnExit(EnemyAttack enemy)
    {

    }
}
