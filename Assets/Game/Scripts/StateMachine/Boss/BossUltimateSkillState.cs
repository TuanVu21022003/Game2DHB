using UnityEngine;

public class BossUltimateSkillState : IBossState
{
    private float chargeTime = 2f;
    private float timer;

    public void OnEnter(Boss boss)
    {
        boss.StopMoving();
        boss.PerformUltimate();
        boss.UseUltimate();
        timer = 0f;

        // TODO: Hiện warning vùng nguy hiểm hoặc spawn telegraph
    }

    public void OnExecute(Boss boss)
    {
        timer += Time.deltaTime;

        if (timer >= chargeTime)
        {
            boss.ChangeState(new BossChaseState());
        }
    }

    public void OnExit(Boss boss)
    {
        // Clean up warning or effects
    }
}
