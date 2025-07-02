using UnityEngine;
using UnityEngine.Events;

public class EnemyBase : Character
{
    [SerializeField] private EnemyReward[] rewards;

    public override void OnHit(float damage, Character attacker, UnityAction<EnemyReward[]> actionDeath)
    {
        base.OnHit(damage, attacker, actionDeath);
        if (isDead)
        {
            actionDeath?.Invoke(rewards);
            OnSpawnItemBase(attacker.transform);
        }
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
        Destroy(gameObject);
        Destroy(healthBar.gameObject);
        Debug.LogError(GameManager.Instance.CheckIsEnemyExist());
    }

    public override void OnDeath()
    {
        base.OnDeath();
        collider.enabled = false; // Disable collider to prevent further interactions
    }

    private void OnSpawnItemBase(Transform attacker)
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i] != null)
            {
                Vector2 posStart = HelperUtils.GetRandomPosition2D(transform.position, 1f);
                switch (rewards[i].type)
                {
                    case ItemType.Gold:
                        for (int j = 0; j < rewards[i].amount; j++)
                        {
                            posStart = HelperUtils.GetRandomPosition2D(transform.position, 1f);
                            ItemBaseGameplay itemCoin = Instantiate(GameManager.Instance.GameplayData.CoinPrefab, posStart, Quaternion.identity);
                            itemCoin.OnInit(attacker, 1);
                        }
                        break;
                    case ItemType.HP:
                        ItemHPGameplay itemHP = Instantiate(GameManager.Instance.GameplayData.HPPrefab, posStart, Quaternion.identity);
                        itemHP.OnInit(attacker, rewards[i].amount);
                        break;
                    default:
                        Debug.LogWarning("Unknown item type: " + rewards[i].type);
                        break;
                }

            }
        }
    }
}
