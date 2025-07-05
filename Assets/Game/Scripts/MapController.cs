using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private Transform listEnemy;
    [SerializeField] private Door door;
    [SerializeField] private Transform posSpawnBoss;
    [SerializeField] public Transform posPlayer;

    private Boss boss;

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
        boss = Instantiate(GameManager.Instance.GameplayData.BossPrefab, posSpawnBoss.position, Quaternion.identity);
        boss.SetPlayer(player.transform);
    }

    public void SpawnChest(Vector3 pos)
    {
        Chest chest = Instantiate(GameManager.Instance.GameplayData.ChestPrefab, pos, Quaternion.identity);
    }
}
