using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : Istate
{
    private float timer;
    private float randomTime;
    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        enemy.StopMoving();
        randomTime = Random.Range(2f, 4f);
    }

    public void OnExcute(Enemy enemy)
    {
        timer += Time.deltaTime;
        if (timer > randomTime)
        {
            enemy.ChangeState(new PartrolState());

        } 
    }

    public void OnExit(Enemy enemy)
    {

    }
}
