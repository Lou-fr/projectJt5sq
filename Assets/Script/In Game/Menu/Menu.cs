using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject LanguagePanel;
    [SerializeField] GameObject RTTMPopUp;
    [SerializeField] GameObject QuitPopUp;
    static bool GameIsPaused = false;
    static bool BtnSetPressed = false;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || BtnSetPressed == true)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            BtnSetPressed = false;
        }
    }
    public void SettingsMenuBtnPress()
    {
        BtnSetPressed = true;
    }
    public void LanguageMenu()
    {
        LanguagePanel.SetActive(true);
    }
    public void RTMM()
    {
        if (RTTMPopUp.active == true)
        {
            RTTMPopUp.SetActive(false);
        }
        else
        {
            RTTMPopUp.SetActive(true);
        }
        
    }
    public void RTMMConfirm()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        if (QuitPopUp.active == true)
        {
            QuitPopUp.SetActive(false);
        }
        else
        {
            QuitPopUp.SetActive(true);
        }
    }
    public void QuitConfirm()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void Resume()
    {
        QuitPopUp.SetActive(false);
        RTTMPopUp.SetActive(false);
        panel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        panel.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }
}
