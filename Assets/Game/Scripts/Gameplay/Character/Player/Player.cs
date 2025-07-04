using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character, IAttacker
{
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private float speed = 800;
    [SerializeField] protected GameObject attackArea;
    [SerializeField] private float jumpForce = 400;
    [SerializeField] private Transform kunaiPoint;
    [SerializeField] private Kunai kunaiPrefab;

    [SerializeField] private float kunaiDamage = 10;
    [SerializeField] private float timDelayKunai = 0.5f;
    [SerializeField] private Transform coinEffect;
    [SerializeField] private Transform hpEffect;

    [SerializeField] private int countHeart = 3;

    private bool isGrounded;
    private float horizontal;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isThrow = false;
    private int coin = 0;
    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            OnChangeCoin();
        }
    }

    public int CountHeart
    {
        get { return countHeart; }
        set
        {
            countHeart = value;
            OnChangeHeart();
        }
    }
    private Vector3 savePoint;


    public Character Target { get; set; }

    private ParticleSystem[] coinPaticles;


    // Start is called before the first frame update
    private void Awake()
    {
        ParticleSystem[] particles = coinEffect.GetComponentsInChildren<ParticleSystem>();
    }

    public override void Start()
    {
        base.Start();
        if(PopupHubManager.Instance.GameplayView != null)
        {
            PopupHubManager.Instance.GameplayView.OnInit(CountHeart, Coin);

        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGround();
        anim.SetFloat("isGround", isGrounded ? 1 : 0);

        horizontal = Input.GetAxisRaw("Horizontal");
        if (isDead)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if(!isGrounded)
        {
            if (rb.linearVelocity.y < 0)
            {
                isJumping = false;
                if (!isAttack && !isThrow)
                {
                    ChangeAnim("fall");

                }

            }
        }
        
        if (isThrow || isAttack)
        {
            if (!isJumping)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                return;

            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Throw();
            return;
        }
        if (isJumping == false && isGrounded)
        {
            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                ChangeAnim("idle");

            }

        }
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }



    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }

    public override void OnInit()
    {
        base.OnInit();
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        
        if (countHeart == 0)
        {
            Debug.LogError("Ban da het mang, game over!");
            PopupHubManager.Instance.LoseView.Show();
            return;
        }
        OnInit();
    }

    public override void OnDeath()
    {
        base.OnDeath();
        --CountHeart;
    }

    private bool CheckGround()
    {
        Vector3 startPos = transform.position + Vector3.down * 0.9f;
        Debug.DrawLine(startPos, startPos + Vector3.down * 0.12f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, 0.12f, layerGround);
        return hit.collider != null;
    }

    public void Attack()
    {
        if (isAttack)
        {
            return;
        }
        AudioManager.Instance.PlaySFX("Attack");
        isAttack = true;
        ChangeAnim("attack");
        Invoke(nameof(ResetAttack), 0.4f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.4f);
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
        attackArea.GetComponent<Collider2D>().enabled = true;
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);
        attackArea.GetComponent<Collider2D>().enabled = false;

    }

    private void ResetAttack()
    {
        isAttack = false;
    }

    public void Throw()
    {
        if(isThrow)
        {
            return;
        }
        isThrow = true;
        ChangeAnim("throw");
        AudioManager.Instance.PlaySFX("Throw");
        Invoke(nameof(ResetThrow), timDelayKunai);
        Invoke(nameof(SpawnKunai), 0.2f);
    }

    private void SpawnKunai()
    {
        Instantiate(kunaiPrefab, kunaiPoint.position, kunaiPoint.rotation).OnInit(this, this, kunaiDamage);
    }

    private void ResetThrow()
    {
        isThrow = false;
    }

    public void Jump()
    {
        if(!isGrounded)
        {
            return;
        }
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
        AudioManager.Instance.PlaySFX("Jump");
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Item")
        {
            ItemBaseGameplay item = collision.GetComponent<ItemBaseGameplay>();
            if (item != null)
            {
                ReceiveItem(item);
                item.OnDespawn();
            }
        }
        if (collision.tag == "DeathZone")
        {
            hp = 0;
            ChangeAnim("die");
            Debug.Log("Ban da die ");
            OnDeath();
            Invoke(nameof(OnDespawn), 1f);
        }
        if (collision.tag == "Chest")
        {
            Chest chest = collision.GetComponent<Chest>();
            chest.Open();
        }
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    public void ReceiveEnemyReward(EnemyReward[] enemyRewards)
    {
        foreach (var reward in enemyRewards)
        {
            Debug.Log($"Received {reward.amount} {reward.type} from enemy.");
        }
    }

    private void ReceiveItem(ItemBaseGameplay itemBase)
    {
        switch (itemBase.type)
        {
            case ItemType.Gold:
                Coin += itemBase.amount;
                Debug.Log($"Received {itemBase.amount} gold. Total coins: {coin}");
                EffectCoin();
                AudioManager.Instance.PlaySFX("ReceiveCoin");
                break;
            case ItemType.HP:
                hp += itemBase.amount * this.maxHp / 100;
                if (hp > maxHp)
                {
                    hp = maxHp;
                }
                healthBar.SetNewHP(hp);
                EffectHP();
                AudioManager.Instance.PlaySFX("ReceiveHP");
                Debug.Log($"Received {itemBase.amount} HP. Current HP: {hp}");
                break;
        }
    }

    private void EffectCoin()
    {
        if (coinPaticles == null || coinPaticles.Length == 0)
        {
            coinPaticles = coinEffect.GetComponentsInChildren<ParticleSystem>();
        }
        foreach (var particle in coinPaticles)
        {
            particle.Play();
        }
    }

    private void EffectHP()
    {
        hpEffect.gameObject.SetActive(true);
    }

    private void OnChangeCoin()
    {
        PopupHubManager.Instance.GameplayView.UpdateCoin(coin);
    }

    private void OnChangeHeart()
    {
        PopupHubManager.Instance.GameplayView.UpdateHeart(countHeart);
    }
}
