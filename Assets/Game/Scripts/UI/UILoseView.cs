using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoseView : UIBaseView
{
    public void TouchGoHome()
    {

    }

    public void TouchRetry()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
