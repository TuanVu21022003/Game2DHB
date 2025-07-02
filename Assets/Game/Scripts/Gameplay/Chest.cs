using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private Animator anim;

    [SerializeField] private EnemyReward[] itemBaseGameplays;

    public void Open()
    {
        anim.Play("Open");
        Invoke(nameof(HandleWin), 1f);
    }

    private void HandleWin()
    {
        PopupHubManager.Instance.WinView.Show();
    }
}
