using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartrolState : IState
{
    private float timer;
    private float randomTime;
    public void OnEnter(Enemy enemy)
    {
        randomTime = Random.Range(3f, 6f);
        timer = 0;
    }

    public void OnExcute(Enemy enemy)
    {
        timer += Time.deltaTime;

        if(enemy.Target != null)
        {
            enemy.ChangeDirection(enemy.transform.position.x < enemy.Target.transform.position.x);
            if(enemy.IsTargetInRange())
            {
                enemy.ChangeState(new AttackState());
            }
            else
            {
                enemy.Moving();
            }
        }

        else
        {
            if (timer < randomTime)
            {
                enemy.Moving();
            }
            else
            {
                enemy.ChangeState(new IdleState());
            }
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}
