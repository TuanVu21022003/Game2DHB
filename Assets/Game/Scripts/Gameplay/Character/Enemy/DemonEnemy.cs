using UnityEngine;
using UnityEngine.Events;

public class DemonEnemy : EnemyAttack
{
    [SerializeField] private PursueBullet bullet;
    public override void Attack()
    {
        base.Attack();
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        Vector3 posStart = transform.position + direction;
        Instantiate(bullet, posStart, Quaternion.identity)
            .OnInit(this, this, damage);
    }

    public override void OnHit(float damage, Character attacker, UnityAction<EnemyReward[]> actionDeath)
    {
        base.OnHit(damage, attacker, actionDeath);
        ChangeAnim("hurt");
    }
}
