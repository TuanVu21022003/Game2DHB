using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWinView : UIBaseView
{

    public override void Show()
    {
        base.Show();
        AudioManager.Instance.PlaySFX("Win");
    }
    public void TouchGoHome()
    {

    }

    public void TouchNextLevel()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
