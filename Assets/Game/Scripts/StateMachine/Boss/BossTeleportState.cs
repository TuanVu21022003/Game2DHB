using UnityEngine;

public class BossTeleportState : IBossState
{
    private float delay = 0.55f;
    private float timer;

    public void OnEnter(Boss boss)
    {
        boss.StopMoving();
        boss.TeleportNearPlayer();
        boss.UseTeleport();
        timer = 0f;
    }

    public void OnExecute(Boss boss)
    {
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            if (boss.IsPlayerInRange(boss.meleeRange))
            {
                boss.ChangeState(new BossMeleeAttackState());
            }
            else
            {
                boss.ChangeState(new BossChaseState());

            }
        }
    }

    public void OnExit(Boss boss)
    {
        // VFX or sound stop if needed
    }
}
