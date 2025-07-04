using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void OnEnter(EnemyAttack enemy);
    void OnExit(EnemyAttack enemy);

    void OnExcute(EnemyAttack enemy);
}
