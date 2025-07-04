using UnityEngine;

public enum BossPhase { Phase1, Phase2, Phase3 }

public class Boss : EnemyBase, IAttacker
{
    [Header("Boss Settings")]
    public Transform player;
    public float moveSpeed = 3.5f;
    public float meleeRange = 1.5f;
    public float rangedRange = 6f;
    public float detectRange = 10f;
    [SerializeField] private GameObject attackArea;
    [SerializeField] public float distanceTeleport = 5f;
    [SerializeField] public float distancePlayer = 3f;
    [SerializeField] public int countBulletUtimate = 3;
    [SerializeField] public int heightBulletUtimate = 1;

    [Header("Skill Prefabs")]
    [SerializeField] private Bullet bossBulletPrefab;
    [SerializeField] private ArcBullet bossArcBulletPrefab;
    [SerializeField] private Transform ThrowPoint;

    [Header("Cooldowns & Limits")]
    public int maxTeleportCount = 3;
    public int maxUltimateCount = 1;
    public float teleportCooldown = 5f;
    public float ultimateCooldown = 10f;

    private int teleportUsed = 0;
    private int ultimateUsed = 0;
    private float teleportTimer = 0;
    private float ultimateTimer = 0;

    private IBossState currentState;
    public BossPhase currentPhase = BossPhase.Phase1;
    public float damagePerRangeAttack = 10f;
    public float damagePerUltimate = 15f;

    private float hpPercent => hp / maxHp;

    public Character Target { get; set; }

    private void Update()
    {
        HelperUtils.DrawCircleLine(transform.position, meleeRange, 50, Color.red, Time.deltaTime);
        HelperUtils.DrawCircleLine(transform.position, rangedRange, 50, Color.red, Time.deltaTime);
        if (isDead) return;

        currentState?.OnExecute(this);

        UpdatePhase();
        teleportTimer += Time.deltaTime;
        ultimateTimer += Time.deltaTime;
    }

    public void ChangeState(IBossState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }

    public override void OnInit()
    {
        base.OnInit();
        Target = player.GetComponent<Character>();
        this.ChangeState(new BossIdleState());
        DeActiveAttack();
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
        Target = player.GetComponent<Character>();
    }

    private void UpdatePhase()
    {
        if (currentPhase == BossPhase.Phase1 && hpPercent <= 0.7f)
        {
            currentPhase = BossPhase.Phase2;
            Debug.Log("Boss entered Phase 2!");
        }
        else if (currentPhase == BossPhase.Phase2 && hpPercent <= 0.4f)
        {
            currentPhase = BossPhase.Phase3;
            Debug.Log("Boss entered Phase 3!");
        }
    }

    public bool IsPlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, player.position) <= range;
    }

    public void MoveToPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(dir.x * moveSpeed, rb.linearVelocity.y);
        ChangeAnim("run");
    }

    public void StopMoving()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void PerformMeleeAttack()
    {
        StopMoving();
        ChangeAnim("attack");
        // Damage logic nằm ở animation event hoặc trigger
        StartCoroutine(HelperUtils.DelayCoroutine(0.3f, () =>
        {
            ActiveAttack();
            Invoke(nameof(DeActiveAttack), 0.5f); // Giả sử tấn công kéo dài 0.5 giây

        }));
    }

    public void PerformRangedAttack()
    {
        StopMoving();
        ChangeAnim("throw");
        Instantiate(bossBulletPrefab, ThrowPoint.position, ThrowPoint.rotation).OnInit(this, this, damagePerRangeAttack);
    }

    public void PerformUltimate()
    {
        StopMoving();
        ChangeAnim("throw");
        int index = -countBulletUtimate/2; // Giả sử bắn từ giữa ra ngoài
        for (int i = 0;i < countBulletUtimate; i++)
        {
            Vector3 targetPosition = player.position + new Vector3((i + index) * 0.8f, 0f, -Target.GetHeight()/2);
            ArcBullet bullet = Instantiate(bossArcBulletPrefab, ThrowPoint.position, ThrowPoint.rotation);
            bullet.OnInit(this, this, damagePerUltimate, targetPosition, heightBulletUtimate);
        }
    }

    public void TeleportNearPlayer()
    {
        ChangeAnim("teleport_hide");
        Invoke(nameof(SetPosAfterTeleport), 0.5f);
        rb.bodyType = RigidbodyType2D.Kinematic; // Đặt Rigidbody thành Static để không bị ảnh hưởng bởi vật lý trong thời gian teleport
        collider.enabled = false; // Tắt collider trong thời gian teleport
    }

    private void SetPosAfterTeleport()
    {
        float distanceHeight = (this.GetHeight() - Target.GetHeight())/2;
        Vector3 offset = (Random.value > 0.5f ? Vector3.left * distancePlayer : Vector3.right * distancePlayer) + new Vector3(0, distanceHeight, 0);
        transform.position = player.position + offset;
        ChangeAnim("teleport_show");
        rb.bodyType = RigidbodyType2D.Dynamic; // Trả lại Rigidbody về trạng thái Dynamic
        collider.enabled = true; // Bật lại collider sau khi teleport
        
    }

    public bool CanTeleport()
    {
        return teleportUsed < maxTeleportCount && teleportTimer >= teleportCooldown;
    }

    public bool CanUseUltimate()
    {
        return ultimateUsed < maxUltimateCount && ultimateTimer >= ultimateCooldown && currentPhase == BossPhase.Phase3;
    }

    public void UseTeleport()
    {
        teleportUsed++;
        teleportTimer = 0;
    }

    public void UseUltimate()
    {
        ultimateUsed++;
        ultimateTimer = 0;
    }

    public override void OnDeath()
    {
        base.OnDeath();
        StopMoving();
        ChangeAnim("die");
        GameManager.Instance.SpawnChest(transform.position); // Kích hoạt rương khi boss chết
        rb.bodyType = RigidbodyType2D.Kinematic; // Đặt Rigidbody thành Static để không bị ảnh hưởng bởi vật lý khi chết
    }

    public void FaceToPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;
        Quaternion targetRotation;

        if (direction.x > 0)
            targetRotation = Quaternion.Euler(0, 0, 0);        // Nhìn sang phải
        else
            targetRotation = Quaternion.Euler(0, 180, 0);      // Nhìn sang trái

        transform.rotation = targetRotation;
    }


    public void Attack()
    {
        throw new System.NotImplementedException();
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
}
