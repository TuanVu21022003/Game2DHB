using UnityEngine;

public class BossChaseState : IBossState
{
    public void OnEnter(Boss boss)
    {
        boss.ChangeAnim("run");
    }

    public void OnExecute(Boss boss)
    {
        if (boss.player == null)
        {
            boss.ChangeState(new BossIdleState());
            return;
        }

        boss.FaceToPlayer();

        if (boss.CanUseUltimate())
        {
            boss.ChangeState(new BossUltimateSkillState());
            return;
        }

        if (boss.IsPlayerInRange(boss.meleeRange))
        {
            boss.ChangeState(new BossMeleeAttackState());
        }
        else if (boss.currentPhase != BossPhase.Phase1 && boss.IsPlayerInRange(boss.rangedRange))
        {
            boss.ChangeState(new BossRangedAttackState());
            
        }
        else
        {
            boss.MoveToPlayer();

            if (boss.CanTeleport() && Vector2.Distance(boss.transform.position, boss.player.position) > boss.distanceTeleport)
            {
                boss.ChangeState(new BossTeleportState());
            }
        }
    }

    public void OnExit(Boss boss)
    {
        boss.StopMoving();
    }
}
