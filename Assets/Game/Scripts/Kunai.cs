using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speedKunai = 1.5f;

    private Character characterOwn;
    private float damage = 0f;

    public void OnInit(Character character, float damage)
    {
        rb.linearVelocity = transform.right * speedKunai;
        Invoke(nameof(OnDespawn), 4.0f);
        this.characterOwn = character;
        this.damage = damage;
    }

    public void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Debug.Log("Kunai da trung");
            
            OnDespawn();
            collision.GetComponent<Character>().OnHit(damage);
        }
    }
}
