using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Istate
{
    void OnEnter(Enemy enemy);
    void OnExit(Enemy enemy);

    void OnExcute(Enemy enemy);
}
