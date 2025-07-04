using UnityEngine;

public class BossRangedAttackState : IBossState
{
    private float attackDelay = 1.8f;
    private float timer;

    public void OnEnter(Boss boss)
    {
        boss.PerformRangedAttack();
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
        // Optional VFX cleanup
    }
}
