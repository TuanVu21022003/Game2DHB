using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, IHit
{
    [SerializeField] private CharacterType characterType;
    public CharacterType CharacterType => characterType;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Animator anim;
    [SerializeField] protected GameObject hitVFX;
    [SerializeField] protected CombatText combatTextPrefab;

    [SerializeField] protected float damage;
    [SerializeField] protected float maxHp;
    [SerializeField] protected HealthBar healthBarPrefab;
    public float Damage => damage;
    protected HealthBar healthBar;
    // Start is called before the first frame update
    protected float hp;
    private string currentAnim;
    protected new CapsuleCollider2D collider;

    public bool isDead => hp <= 0;
    public virtual void Start()
    {
        collider = GetComponent<CapsuleCollider2D>();
        OnInit();
    }

    public virtual void OnInit()
    {
        healthBar = Instantiate(healthBarPrefab);
        hp = maxHp;
        healthBar.OnInit(maxHp, transform, characterType);
    }

    public virtual void OnDespawn()
    {

    }

    public virtual void OnDeath()
    {
        Debug.Log("Nhan vat da chet");
        ChangeAnim("die");
        AudioManager.Instance.PlaySFX("Die");
        rb.linearVelocity = Vector2.zero;
        Invoke(nameof(OnDespawn), 1f);
    }

    public virtual void OnHit(float damage, Character attacker, UnityAction<EnemyReward[]> actionDeath)
    {
        if (!isDead)
        {
            hp -= damage;
            if (isDead)
            {
                hp = 0;
                OnDeath();
            }

            healthBar.SetNewHP(hp);
            Instantiate(combatTextPrefab, transform.position + Vector3.up * 2, Quaternion.identity).OnInit(damage);
            EffectDamage();
        }
    }

    public void EffectDamage()
    {
        Instantiate(hitVFX, transform.position, transform.rotation);
    }

    public void ChangeAnim(string animname)
    {
        if (currentAnim != animname)
        {
            anim.ResetTrigger(animname);
            currentAnim = animname;
            anim.SetTrigger(currentAnim);
        }
    }

    public float GetHeight()
    {
        if (collider != null)
        {
            return collider.size.y * transform.localScale.y;
        }
        else
        {
            Debug.LogWarning("Collider is not assigned on " + gameObject.name);
            return 0;
        }

    }
}

public enum CharacterType
{
    Player,
    Enemy
}
