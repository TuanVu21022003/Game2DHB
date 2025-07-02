using UnityEngine;

public class NinjaEnemy : EnemyAttack
{
    [SerializeField] private GameObject attackArea;

    public override void OnInit()
    {
        base.OnInit();
        DeActiveAttack();
    }

    public override void Attack()
    {
        base.Attack();
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
        attackArea.GetComponent<Collider2D>().enabled = true;
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
        attackArea.GetComponent<Collider2D>().enabled = false;

    }
}
