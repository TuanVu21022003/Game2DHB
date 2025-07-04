using UnityEngine;

public class BossMeleeAttackState : IBossState
{
    private float attackDelay = 1.2f;
    private float timer;

    public void OnEnter(Boss boss)
    {
        boss.PerformMeleeAttack();
        timer = 0f;
    }

    public void OnExecute(Boss boss)
    {
        timer += Time.deltaTime;

        if (timer >= attackDelay)
        {
            boss.ChangeState(new BossChaseState());
        }
    }

    public void OnExit(Boss boss)
    {
        // Clean up or reset if needed
    }
}
