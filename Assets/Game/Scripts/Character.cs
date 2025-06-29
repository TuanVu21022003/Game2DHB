using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterType characterType;
    [SerializeField] private Animator anim;
    [SerializeField] protected GameObject hitVFX;
    [SerializeField] protected CombatText combatTextPrefab;
    [SerializeField] protected GameObject attackArea;
    [SerializeField] protected float damage;
    [SerializeField] protected float maxHp;
    [SerializeField] protected HealthBar healthBarPrefab;
    public float Damage => damage;
    protected HealthBar healthBar;
    // Start is called before the first frame update
    protected float hp;
    private GameObject hitObject;

    private string currentAnim;

    public bool isDead => hp <= 0;
    private void Start()
    {
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
        Invoke(nameof(OnDespawn), 1f);
    }

    public virtual void OnHit(float damage)
    {
        if(!isDead)
        {
            hp -= damage;
            if(isDead)
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
        hitObject = Instantiate(hitVFX, transform.position, transform.rotation);
        Invoke(nameof(DestroyHit), 0.5f);
    }

    public void DestroyHit()
    {
        Destroy(hitObject);
    }

    public void ActiveAttack()
    {
        attackArea.SetActive(true);
        attackArea.GetComponent<Collider2D>().enabled = true;
    }

    public void DeActiveAttack()
    {
        attackArea.SetActive(false);
        attackArea.GetComponent<Collider2D>().enabled = false;

    }


    protected void ChangeAnim(string animname)
    {
        if (currentAnim != animname)
        {
            anim.ResetTrigger(animname);
            currentAnim = animname;
            anim.SetTrigger(currentAnim);
        }
    }
}

public enum CharacterType
{
    Player,
    Enemy
}
