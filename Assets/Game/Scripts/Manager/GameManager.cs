using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public GameplayScriptable GameplayData;
    [SerializeField] private Transform listEnemy;
    [SerializeField] private Door door;
    [SerializeField] private Transform posSpawnBoss;

    private Boss boss;

    private void Start()
    {
        AudioManager.Instance.PlayMusic("MainTheme");
    }

    public bool CheckIsEnemyExist()
    {
        if (listEnemy == null)
        {
            Debug.LogError("List Enemy is null");
            return false;
        }
        Debug.LogError(listEnemy.childCount);
        if (listEnemy.childCount > 1) return true;
        door.OnDown();
        return false;
    }

    public void SpawnBoss(Character player)
    {
        boss = Instantiate(GameplayData.BossPrefab, posSpawnBoss.position, Quaternion.identity);
        boss.SetPlayer(player.transform);
    }

    public void SpawnChest(Vector3 pos)
    {
        Chest chest = Instantiate(GameplayData.ChestPrefab, pos, Quaternion.identity);
    }
}
