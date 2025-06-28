using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private float speed = 800;
    
    [SerializeField] private float jumpForce = 400;
    [SerializeField] private Transform kunaiPoint;
    [SerializeField] private Kunai kunaiPrefab;
    
    [SerializeField] private float kunaiDamage = 10;
    private bool isGrounded;
    private float horizontal;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isThrow = false;
    private bool isDeath = false;
    private int coin = 0;
    private Vector3 savePoint;

    
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGround();

        horizontal = Input.GetAxisRaw("Horizontal");
        if (isDead)
        {
            return;
        }
        if (isAttack)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (isThrow)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (isGrounded)
        {
            if(isJumping)
            {
                return;
            }

            
            
            if(Input.GetKeyDown(KeyCode.Space) )
            {
                Jump();
            }

            
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                ChangeAnim("idle");
                
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                Throw();
            }

        }

        else
        {
            if (rb.linearVelocity.y < 0)
            {
                isJumping = false;
                ChangeAnim("fall");
                
            }
        }
        if(Mathf.Abs(horizontal) > 0.1f)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }


        
    }

    public override void OnInit()
    {
        base.OnInit();
        isDeath = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }
    public override void OnHit(float damage)
    {
        base.OnHit(damage);
    }

    public override void OnDeath()
    {
        base.OnDeath();
    }

    private bool CheckGround()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.05f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.05f, layerGround);
        return hit.collider != null;
    }

    private void Attack() {
        isAttack = true;
        ChangeAnim("attack");
        Invoke(nameof(ResetAttack), 0.4f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.4f);
    }

    private void ResetAttack()
    {
        isAttack = false;
    }

    private void Throw()
    {
        isThrow = true;
        ChangeAnim("throw");
        Invoke(nameof(ResetThrow), 0.4f);
        Instantiate(kunaiPrefab, kunaiPoint.position, kunaiPoint.rotation).OnInit(this, kunaiDamage);
    }

    private void ResetThrow()
    {
        isThrow = false;
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Coin") {
            Destroy(collision.gameObject);
            ++coin;
            Debug.Log("Ban da nhan duoc " + coin + " coin");
        }
        if(collision.tag == "DeathZone")
        {
            hp = 0;
            ChangeAnim("die");
            Debug.Log("Ban da die ");
            Invoke(nameof(OnInit), 1.5f);
        }
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }
}
