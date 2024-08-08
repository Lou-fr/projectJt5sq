using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BleizEntertainment
{
    public class Menu : MonoBehaviour
    {
        public static Action OnRTMM = delegate { };
        public static Action<bool> LobbyUIOpen = delegate { };
        [SerializeField] private GameObject panel, languagePanel, cntrlParentPanel, graphicParentPanel, RTTMPopUp, quitPopUp, phoneController, friendUi, LobbyUi;
        private GameObject cntrlPanel;
        private GameObject graphicPanel;
        private CinemachineInputProvider cinemachinInput;
        PlayerInput _input;
        static bool GameIsPaused = false;
        static bool BtnSetPressed = false;
#if !UNITY_SERVER

        public void Start()
        {
#if UNITY_STANDALONE
            graphicPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Gr_computer"), graphicParentPanel.transform);
            cntrlPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Cntrl_computer"), cntrlParentPanel.transform);
            cntrlPanel.SetActive(true);
            cinemachinInput = GetComponent<CinemachineInputProvider>();
#endif
#if UNITY_ANDROID || UNITY_IOS
        GraphicPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Gr_phone"), GraphicParentPanel.transform);
        CntrlPanel = Instantiate(Resources.Load<GameObject>("Prefabs/Cntrl_phone"), CntrlParentPanel.transform);
        PhoneController = Instantiate(Resources.Load<GameObject>("Prefabs/PhoneControll"));
        Button button = GameObject.Find("ParameterOnPhone").GetComponentInChildren<Button>();
        button.onClick.AddListener(SettingsMenuBtnPress);
        CntrlPanel.SetActive(false);
#endif
            _input = FindAnyObjectByType<PlayerInput>().GetComponent<PlayerInput>();
        }
        void Awake()
        {
            Sensivity_Controller.MenuReady += OnMenuReady;
            //ClientRelayManager.ServerChange += handleServerChange;
            //LobbyManager.KickFromLobby += handleServerChange;
        }

        private void OnMenuReady()
        {
            Sensivity_Controller.MenuReady -= OnMenuReady;
            panel.SetActive(false);
        }

        void OnDestroy()
        {
            //ClientRelayManager.ServerChange -= handleServerChange;
            //LobbyManager.KickFromLobby -= handleServerChange;
        }

        private void handleServerChange()
        {
            _input = null;
        }

        public void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame || BtnSetPressed == true)
            {
                if (_input is null)
                {
                    _input = FindAnyObjectByType<PlayerInput>().GetComponent<PlayerInput>();
                }
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
            languagePanel.SetActive(true);
            if (graphicPanel.activeSelf || cntrlPanel.activeSelf)
            {
                graphicPanel.SetActive(false);
                cntrlPanel.SetActive(false);
            }
        }
        public void GraphicMenu()
        {
            graphicPanel.SetActive(true);
            if (languagePanel.activeSelf || cntrlPanel.activeSelf)
            {
                languagePanel.SetActive(false);
                cntrlPanel.SetActive(false);
            }
        }
        public void ControlMenu()
        {
            cntrlPanel.SetActive(true);
            if (graphicPanel.activeSelf || languagePanel.activeSelf)
            {
                graphicPanel.SetActive(false);
                languagePanel.SetActive(false);
            }
        }
        public void RTMM()
        {
            if (RTTMPopUp.activeSelf == true)
            {
                RTTMPopUp.SetActive(false);
            }
            else
            {
                RTTMPopUp.SetActive(true);
                if (quitPopUp.activeSelf == true)
                {
                    quitPopUp.SetActive(false);
                }
            }

        }
        public void RTMMConfirm()
        {
            GameIsPaused = false;
            Time.timeScale = 1f;
            _input = null;
            SceneManager.LoadScene(0);
            OnRTMM?.Invoke();
        }
        public void Quit()
        {
            if (quitPopUp.activeSelf == true)
            {
                quitPopUp.SetActive(false);
            }
            else
            {
                quitPopUp.SetActive(true);
                if (RTTMPopUp.activeSelf == true)
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
        public void LobbyUI()
        {
            if (panel.activeSelf is true) panel.SetActive(false);
            if (friendUi.activeSelf is true) friendUi.SetActive(false);
            if (_input is null) _input = FindAnyObjectByType<PlayerInput>().GetComponent<PlayerInput>();
            if (LobbyUi.activeSelf is false) { LobbyUi.SetActive(true); LobbyUIOpen?.Invoke(true); _input.enabled = false; if (phoneController is not null) phoneController.SetActive(false); }
            else { LobbyUi.SetActive(false); LobbyUIOpen?.Invoke(false); _input.enabled = true; if (phoneController is not null) phoneController.SetActive(true); }

#if UNITY_ANDROID || UNITY_IOS

#endif
        }
        public void FriendUI()
        {
            if (panel.activeSelf is true) panel.SetActive(false);
            if (LobbyUi.activeSelf is true) LobbyUi.SetActive(false);
            if (_input is null) _input = FindAnyObjectByType<PlayerInput>().GetComponent<PlayerInput>();
            if (friendUi.activeSelf is false) { friendUi.SetActive(true); _input.enabled = false; if (phoneController is not null) phoneController.SetActive(false); }
            else { friendUi.SetActive(false); _input.enabled = true; if (phoneController is not null) phoneController.SetActive(true); }
#if UNITY_ANDROID || UNITY_IOS

#endif
        }

        void Resume()
        {
            if (quitPopUp.activeSelf == true || RTTMPopUp.activeSelf == true)
            {
                quitPopUp.SetActive(false);
                RTTMPopUp.SetActive(false);
            }
#if UNITY_ANDROID || UNITY_IOS
        PhoneController.SetActive(true );
#endif
            panel.SetActive(false);
            _input.enabled = true;
            GameIsPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Pause()
        {
            if (friendUi.activeSelf is true) friendUi.SetActive(false);
            if (LobbyUi.activeSelf is true) LobbyUi.SetActive(false);
            panel.SetActive(true);
            _input.enabled = false;
            GameIsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

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
}