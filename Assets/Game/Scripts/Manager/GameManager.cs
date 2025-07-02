using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public GameplayScriptable GameplayData;
    [SerializeField] private Transform listEnemy;
    [SerializeField] private Transform chest;

    private void Start()
    {
        chest.gameObject.SetActive(false);
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
        chest.gameObject.SetActive(true);
        return false;
    }
}
