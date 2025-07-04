using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float speed = 1.5f;
    [SerializeField] protected float timeDestroy = 4f;

    protected Character characterOwn;
    protected IAttacker attacker;
    private float damage = 0f;

    public virtual void OnInit(Character character, IAttacker attacker, float damage)
    {
        rb.linearVelocity = transform.right * speed;
        Invoke(nameof(OnDespawn), timeDestroy);
        this.characterOwn = character;
        this.damage = damage;
        this.attacker = attacker;
    }

    public virtual void OnDespawn()
    {
        
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if(character != null && character.CharacterType == characterOwn.CharacterType)
        {
            return; // Khong lam gi neu la dong minh
        }
        if (collision.tag == "Enemy")
        {
            AudioManager.Instance.PlaySFX("Hit");

            Debug.Log("Bullet da trung");
            
            OnDespawn();
            Player player = characterOwn as Player;
            collision.GetComponent<IHit>().OnHit(damage, characterOwn, player.ReceiveEnemyReward);
        }
        else if (collision.tag == "Player")
        {
            AudioManager.Instance.PlaySFX("Hit");

            OnDespawn();
            collision.GetComponent<IHit>().OnHit(damage, characterOwn);
        }
    }
}
