using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoseView : UIBaseView
{

    public override void Show()
    {
        base.Show();
        AudioManager.Instance.PlaySFX("Lose");
    }
    public void TouchGoHome()
    {

    }

    public void TouchRetry()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
