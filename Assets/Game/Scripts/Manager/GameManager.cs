using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public GameplayScriptable GameplayData;
    public MapController Map;
    public Player Player;
    public CameraFollow CameraFollow;
    private void Start()
    {
        AudioManager.Instance.PlayMusic("MainTheme");
        SpawnMap();
        SpawnPlayer();
        TouchManager.Instance.Active(true);
        CameraFollow = FindObjectOfType<CameraFollow>();
        if (CameraFollow != null)
        {
            CameraFollow.SetTarget(Player.transform);
        }
    }

    private void SpawnMap()
    {
        MapController mapPrefab = Resources.Load<MapController>($"Maps/Map_{GameConstants.Level}");
        Map = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
    }

    private void SpawnPlayer()
    {
        Player playerPrefab = GameplayData.PlayerPrefab;
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not set in GameplayData");
            return;
        }
        Player = Instantiate(playerPrefab, Map.posPlayer.position, Quaternion.identity);
    }
}
