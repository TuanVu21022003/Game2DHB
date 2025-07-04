using UnityEngine;

public class PursueBullet : Bullet
{
    private bool isPursuing = true;
    private void Update()
    {
        if(attacker.Target && isPursuing)
        {
            Vector3 direction = (attacker.Target.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
            // Tính góc xoay
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Gán rotation (nếu kunai hướng trục X)
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else if (attacker.Target == null)
        {
            isPursuing = false;
        }
    }
}
