using BleizEntertainment.Multiplayer;
using Mirror;
using PlayFab;
using PlayFab.MultiplayerModels;
using PlayFab.Networking;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject panel, LanguagePanel, CntrlParentPanel, GraphicParentPanel, RTTMPopUp, QuitPopUp, PhoneController;
    private GameObject CntrlPanel;
    private GameObject GraphicPanel;
    BleizInputManager _input;
    static bool GameIsPaused = false;
    static bool BtnSetPressed = false;
#if !UNITY_SERVER

    public void Start()
    {
#if UNITY_STANDALONE
        GraphicPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Gr_computer"), GraphicParentPanel.transform);
        CntrlPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Cntrl_computer"), CntrlParentPanel.transform);
        CntrlPanel.SetActive(false);
#endif
#if UNITY_ANDROID || UNITY_IOS
        GraphicPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Gr_phone"), GraphicParentPanel.transform);
        CntrlPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Cntrl_phone"), CntrlParentPanel.transform);
        PhoneController = Instantiate(Resources.Load<GameObject>("Prefabs/PhoneControll"));
        Button button = GameObject.Find("ParameterOnPhone").GetComponentInChildren<Button>();
        button.onClick.AddListener(SettingsMenuBtnPress);
        CntrlPanel.SetActive(false);
#endif
        _input = FindAnyObjectByType<BleizInputManager>().GetComponent<BleizInputManager>();
    }
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
        if(_input is null)
        {
            _input = FindAnyObjectByType<BleizInputManager>().GetComponent<BleizInputManager>();
        }
    }
    public void SettingsMenuBtnPress()
    {
        BtnSetPressed = true;
    }
    public void LanguageMenu()
    {
        LanguagePanel.SetActive(true);
        if (GraphicPanel.activeSelf|| CntrlPanel.activeSelf)
        {
            GraphicPanel.SetActive(false);
            CntrlPanel.SetActive(false);
        }
    }
    public void GraphicMenu()
    {
        GraphicPanel.SetActive(true);
        if (LanguagePanel.activeSelf || CntrlPanel.activeSelf)
        {
            LanguagePanel.SetActive(false);
            CntrlPanel.SetActive(false) ;
        }
    }
    public void ControlMenu()
    {
        CntrlPanel.SetActive(true) ;
        if(GraphicPanel.activeSelf || LanguagePanel.activeSelf)
        {
            GraphicPanel.SetActive(false);
            LanguagePanel.SetActive(false);
        }
    }
    public void RTMM()
    {
        if (RTTMPopUp.activeSelf== true)
        {
            RTTMPopUp.SetActive(false);
        }
        else
        {
            RTTMPopUp.SetActive(true);
            if(QuitPopUp.activeSelf == true)
            {
                QuitPopUp.SetActive(false);
            }
        }
        
    }
    public void RTMMConfirm()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        BleizNetworkManager.Instance.StopClient();
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        if (QuitPopUp.activeSelf == true)
        {
            QuitPopUp.SetActive(false);
        }
        else
        {
            QuitPopUp.SetActive(true);
            if (RTTMPopUp.activeSelf ==true)
            {
                RTTMPopUp.SetActive(false);
            }
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
        if (QuitPopUp.activeSelf == true || RTTMPopUp.activeSelf == true)
        {
            QuitPopUp.SetActive(false);
            RTTMPopUp.SetActive(false);
        }
#if UNITY_ANDROID || UNITY_IOS
        PhoneController.SetActive(true );
#endif
        panel.SetActive(false);
        _input.enabled = true;
        GameIsPaused = false;
    }

    void Pause()
    {
        panel.SetActive(true);
        _input.enabled = false;
        GameIsPaused = true;

#if UNITY_ANDROID || UNITY_IOS
        PhoneController.SetActive(false);
#endif
    }
#endif
#if UNITY_SERVER
    private void Start()
    {
        Destroy(GameObject.Find("Primary_Canva"));        
    }
#endif
}
