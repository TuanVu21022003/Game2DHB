﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EnemyAttack : EnemyBase, IAttacker
{
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] public float timeDelayAttack;
    [SerializeField] public Transform enemyWallPrefab;
    [SerializeField] public float wallRange;

    private Transform enemyWallL;
    private Transform enemyWallR;

    private IState currentState;
    private bool isRight = true;

    public Character Target { get; set; }

    private void Update()
    {
        HelperUtils.DrawCircleLine(transform.position, attackRange, 50, Color.red, Time.deltaTime);
        if (isDead) return;
        if (currentState != null)
        {
            currentState.OnExcute(this);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        enemyWallL = Instantiate(enemyWallPrefab, transform.position + Vector3.left * wallRange, Quaternion.identity);
        enemyWallR = Instantiate(enemyWallPrefab, transform.position + Vector3.right * wallRange, Quaternion.identity);
        this.ChangeState(new IdleState());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();

        Destroy(gameObject);
        Destroy(healthBar.gameObject);
        Destroy(enemyWallL.gameObject);
        Destroy(enemyWallR.gameObject);
    }
    

    public override void OnDeath()
    {
        ChangeState(null);
        base.OnDeath();

    }

    public void ChangeState(IState state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = state;
        if (currentState != null)
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
        if(Target == null) return;
        if(Target.isDead) return;
        AudioManager.Instance.PlaySFX("Attack");
        ChangeAnim("attack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyWall")
        {
            if(currentState is AttackState)
            {
                return;
            }
            this.Target = null;
            ChangeState(new PartrolState());
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
        if (Target != null && Vector2.Distance(Target.transform.position, transform.position) <= attackRange)
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
        this.Target = character;
        if (IsTargetInRange())
        {
            ChangeState(new AttackState());
        }
        else
        {
            if (Target != null)
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
