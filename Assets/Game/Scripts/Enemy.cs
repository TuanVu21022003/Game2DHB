using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Enemy : Character, IAttacker
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private EnemyReward[] rewards;
    [SerializeField] public float timeDelayAttack;
    private IState currentState;
    private bool isRight = true;

    public Character Target { get; set; }

    private void Update()
    {
        DrawCircleLine(transform.position, attackRange, 50, Color.red, Time.deltaTime);
        if (isDead) return;
        if (currentState !=  null)
        {
            currentState.OnExcute(this);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        this.ChangeState(new IdleState());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        
        Destroy(gameObject);
        Destroy(healthBar.gameObject);

    }

    

    public override void OnHit(float damage, UnityAction<EnemyReward[]> actionDeath)
    {
        base.OnHit(damage, actionDeath);
        if(isDead)
        {
            actionDeath?.Invoke(rewards);
        }
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

    public virtual void Attack()
    {
        ChangeAnim("attack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyWall")
        {
            ChangeState(new PartrolState());
            this.Target = null;
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
        if(Target != null && Vector2.Distance(Target.transform.position, transform.position) <= attackRange) 
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
        Debug.LogError("Da va cham");
        this.Target = character;
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

    void DrawCircleLine(Vector3 center, float radius, int segments, Color color, float duration)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0, 0); // Bắt đầu từ trục X dương

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius, 0);
            Debug.DrawLine(prevPoint, nextPoint, color, duration);
            prevPoint = nextPoint;
        }
    }
}
