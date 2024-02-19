using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.UI;
using GetLocal;

public class PlayfabLogin : MonoBehaviour
{
    [SerializeField] private GameObject PlayMenu,LoginMenu,ConError,LogOutPopUp,LogOutButton;
    [SerializeField] private TMP_InputField username,password =default;
    [SerializeField] private TextMeshProUGUI _error,_welcome;
    [SerializeField] private Toggle ConPersistent;
    public string SessionTicket;
    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ConError.SetActive(true);
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
        string temp = GetLocalal.GetString("StartScreen", "Welcome");
        _welcome.text = temp + username.text;
        SessionTicket = result.SessionTicket;
        LoginMenu.SetActive(false);
        PlayMenu.SetActive(true);
    }
    private void _OnLoginSucess(LoginResult result)
    {
        string temp = GetLocalal.GetString("StartScreen", "Welcome");
        _welcome.text = temp + PlayerPrefs.GetString("Username");
        SessionTicket = result.SessionTicket;
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
