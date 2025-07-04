using UnityEngine;

public class BossIdleState : IBossState
{
    private float timer;
    private float idleDuration;

    public void OnEnter(Boss boss)
    {
        boss.StopMoving();
        boss.ChangeAnim("idle");
        idleDuration = Random.Range(1.5f, 2.5f);
        timer = 0f;
    }

    public void OnExecute(Boss boss)
    {
        timer += Time.deltaTime;

        if (boss.player != null && boss.IsPlayerInRange(boss.detectRange))
        {
            boss.ChangeState(new BossChaseState());
        }
        else if (timer >= idleDuration)
        {
            boss.ChangeState(new BossChaseState()); // fallback: always patrol/chase after idle
        }
    }

    public void OnExit(Boss boss)
    {
        // Nothing needed here yet
    }
}
