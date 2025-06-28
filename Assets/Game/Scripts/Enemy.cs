using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : Character
{

    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    private Character target;
    public Character Target => target;
    private IState currentState;
    private bool isRight = true;
    private void Update()
    {
        if(isDead) return;
        if (currentState !=  null)
        {
            currentState.OnExcute(this);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        this.ChangeState(new IdleState());
        DeActiveAttack();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        
        Destroy(gameObject);
        Destroy(healthBar.gameObject);

    }

    

    public override void OnHit(float damage)
    {
        base.OnHit(damage);
    }

    public override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();
        
    }



    public void ChangeState(IState state)
    {
        if(currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = state;
        if(currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void Moving()
    {
        ChangeAnim("run");
        rb.linearVelocity = transform.right * moveSpeed;
    }

    public void StopMoving()
    {
        ChangeAnim("idle");
        rb.linearVelocity = Vector2.zero;
    }

    public void Attack()
    {
        ChangeAnim("attack");
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.4f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyWall")
        {
            ChangeState(new PartrolState());
            this.target = null;
            ChangeDirection(!isRight);
        }
    }

    public void ChangeDirection(bool isRight)
    {
        this.isRight = isRight;
        transform.rotation = isRight ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(Vector3.up * 180);
    }

    public bool IsTargetInRange()
    {
        if(Target != null && Vector2.Distance(target.transform.position, transform.position) <= attackRange) 
        {

            return true;
        }
        else
        {
            return false;
        }
    }

    internal void SetTarget(Character character)
    {
        Debug.Log("Da va cham");
        this.target = character;
        if(IsTargetInRange()) {
            ChangeState(new AttackState());
        }
        else
        {
            if(Target != null)
            {
                ChangeState(new PartrolState());    

            }
            else
            {
                ChangeState(new IdleState());
            }
        }
    }
}
