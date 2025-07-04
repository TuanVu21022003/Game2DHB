using UnityEngine;

public class CheckJoinDoor : MonoBehaviour
{
    [SerializeField] Door door;
    private Collider2D collider2D;
    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            door.OnUp();
            Player player = collision.GetComponent<Player>();
            player.SavePoint();
            GameManager.Instance.SpawnBoss(player);
            collider2D.enabled = false; // Disable the collider to prevent further triggers
        }
    }

    
}
