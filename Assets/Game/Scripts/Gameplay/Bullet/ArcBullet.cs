using UnityEngine;

public class ArcBullet : Bullet
{
    [SerializeField] private float arcHeight = 2f;
    [SerializeField] private EffectBase efBoomPrefab;

    public void OnInit(Character character, IAttacker attacker, float damage, Vector2 target, float height)
    {
        base.OnInit(character, attacker, damage);
        rb.gravityScale = 1f; // bật gravity
        arcHeight = height;
        // Tính vận tốc ban đầu để bay theo cung
        Vector2 velocity = CalculateArcVelocity(transform.position, target, arcHeight);
        rb.linearVelocity = velocity;
    }

    private Vector2 CalculateArcVelocity(Vector2 start, Vector2 end, float arcHeight)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y);

        float displacementY = end.y - start.y;
        Vector2 displacementXZ = new Vector2(end.x - start.x, 0);

        // Tính vận tốc dọc cần có để lên tới đỉnh cao arcHeight
        float velocityY = Mathf.Sqrt(2 * gravity * arcHeight);
        float timeToPeak = velocityY / gravity;

        float totalHeight = arcHeight + displacementY;
        float timeToFall = Mathf.Sqrt(2 * Mathf.Abs(totalHeight) / gravity);

        float totalTime = timeToPeak + timeToFall;

        float velocityX = displacementXZ.x / totalTime;

        return new Vector2(velocityX, velocityY);
    }

    private void Update()
    {
        if (rb.linearVelocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Ground"))
        {
            OnDespawn();
        }
    }

    public override void OnDespawn()
    {
        Instantiate(efBoomPrefab, transform.position, Quaternion.identity);
        base.OnDespawn();
    }
}
