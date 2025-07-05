using UnityEngine;

public class TouchManager : Singleton<TouchManager>
{
    [SerializeField] private GameObject panel;
    private void Start()
    {
        
    }

    public void Active(bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
    }

    public void MovePlayer(int horizontal)
    {
        GameManager.Instance.Player.SetMove(horizontal);
    }

    public void JumpPlayer()
    {
        GameManager.Instance.Player.Jump();
    }

    public void AttackPlayer()
    {
        GameManager.Instance.Player.Attack();
    }

    public void ThrowPlayer()
    {
        GameManager.Instance.Player.Throw();
    }
}
