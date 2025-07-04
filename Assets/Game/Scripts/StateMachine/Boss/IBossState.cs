using UnityEngine;

public interface IBossState
{
    void OnEnter(Boss boss);
    void OnExecute(Boss boss);
    void OnExit(Boss boss);
}
