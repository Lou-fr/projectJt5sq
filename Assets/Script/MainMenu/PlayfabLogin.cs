using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using GetLocal;
using System;

public class PlayfabLogin : MonoBehaviour
{
    [SerializeField] private GameObject PlayMenu,LoginMenu,ConError,LogOutPopUp,LogOutButton;
    [SerializeField] private TMP_InputField username,password =default;
    [SerializeField] private TextMeshProUGUI _error,_welcome;
    [SerializeField] private Toggle ConPersistent;
    private string str_username;
    public string SessionTicket;
    private void Awake()
    {
        SelectLanguage.changedlocal += handleChangeWelcome;
    }

    private void handleChangeWelcome(int obj)
    {
        _welcome.text = GetLocalal.GetString("StartScreen", "Welcome") + str_username;
    }
    

    private void OnDestroy()
    {
        SelectLanguage.changedlocal -=handleChangeWelcome;
    }
    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ConError.SetActive(true);
            Button btn1 = GameObject.Find("Ack_sever").GetComponent<Button>();
            btn1.onClick.RemoveAllListeners();
            btn1.onClick.AddListener(OnAcknowledgeButtonConServ);
            LoginMenu.SetActive(false);
        }
        else
        {
            if (PlayerPrefs.HasKey("Username"))
            {
                PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
                {
                    Username = PlayerPrefs.GetString("Username"),
                    Password = PlayerPrefs.GetString("Password"),
                }, _OnLoginSucess, OnLoginError);
            }
        }
    }

    public void SignIn()
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = username.text,
            Password = password.text,
        }, OnLoginSucess, OnLoginError);
    }
    private void OnLoginSucess(LoginResult result)
    {
        if (ConPersistent.isOn)
        {
            PlayerPrefs.SetString("Username", username.text);
            PlayerPrefs.SetString("Password", password.text);
        }
        str_username = username.text;
        handleChangeWelcome(1);
        SessionTicket = result.SessionTicket;
        LoginMenu.SetActive(false);
        PlayMenu.SetActive(true);
    }
    private void _OnLoginSucess(LoginResult result)
    {
        SessionTicket = result.SessionTicket;
        str_username = PlayerPrefs.GetString("Username");
        handleChangeWelcome(1);
        LoginMenu.SetActive(false);
        PlayMenu.SetActive(true);
    }
        private void OnLoginError(PlayFabError error)
    {
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Password");
        _error.gameObject.SetActive(true);
        _error.text = error.ErrorMessage;
    }
    public void OnLogOutConfirmButton()
    {
        SessionTicket = null;
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Password");
        LoginMenu.SetActive(true); LogOutPopUp.SetActive(false);
    }
    public void OnLogoutCancelButton()
    {
        LogOutButton.SetActive(true); PlayMenu.SetActive(true); LogOutPopUp.SetActive(false);
    }
    public void OnAcknowledgeButtonConServ()
    {
        Application.Quit();
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
