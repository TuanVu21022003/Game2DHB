using UnityEngine;
using UnityEngine.SceneManagement;

public class UIWinView : UIBaseView
{
    public void TouchGoHome()
    {

    }

    public void TouchNextLevel()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
