using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, IHit
{
    [SerializeField] private CharacterType characterType;
    public CharacterType CharacterType => characterType;
    [SerializeField] private Animator anim;
    [SerializeField] protected GameObject hitVFX;
    [SerializeField] protected CombatText combatTextPrefab;
    
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

    public virtual void OnHit(float damage, UnityAction<EnemyReward[]> actionDeath)
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
