using GetLocal;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject ConMenu, PlayMenu, LogOutPopUp;
    [SerializeField] private TextMeshProUGUI ErrorLogin,WelcomeText;
    [SerializeField] private TMP_InputField username, password;
    [SerializeField] private Button Login,Play,LogOutCancel,LogOut,LogOutConfirm;
    [SerializeField] private Toggle LoginSty;
    private string _username;
    private void Awake()
    {
        Unity_Auth.OnSucess += handleSucessLogin;
        SelectLanguage.changedlocal += ChangeWelcomeText;
        Unity_Auth.OnError += handleErrorLogin;
        Unity_Auth.OnInfoSucess += GetPlayerInfo;
        Login.onClick.AddListener(OnLoginBtn);
        LogOut.onClick.AddListener(OnlogOutBtn);
        LogOutCancel.onClick.AddListener(OnlogOutBtn);
        LogOutConfirm.onClick.AddListener(OnLogOutConfirmBtn);
    }
    private void OnDestroy()
    {
        SelectLanguage.changedlocal -= ChangeWelcomeText;
        Unity_Auth.OnSucess -= handleSucessLogin;
        Unity_Auth.OnError -= handleErrorLogin;
        Unity_Auth.OnInfoSucess -= GetPlayerInfo;
        Login.onClick.RemoveListener(OnLoginBtn);
        LogOut.onClick.RemoveListener(OnlogOutBtn);
        LogOutCancel.onClick.RemoveListener(OnlogOutBtn);
        LogOutConfirm.onClick.RemoveListener(OnLogOutConfirmBtn);
    }

    private void ChangeWelcomeText()
    {
        if(_username is null)
        {
            Unity_Auth.GetPlayerInfo();
            return;
        }
        string temp = GetLocalal.GetString("StartScreen", "Welcome") + _username;
        WelcomeText.text = temp;
    }

    private void GetPlayerInfo(PlayerInfo info)
    {
        _username = info.Username;
        ChangeWelcomeText();
    }

    void OnLoginBtn()
    {
        Unity_Auth.SignInWithUsernamePasswordAsync(username.text,password.text);
    }
    void OnlogOutBtn()
    {
        if (LogOutPopUp.activeSelf is true) LogOutPopUp.SetActive(false);
        else LogOutPopUp.SetActive(true);
    }
    void OnLogOutConfirmBtn()
    {
        AuthenticationService.Instance.SignOut();
        AuthenticationService.Instance.ClearSessionToken();
        _username = null;
        OnlogOutBtn();
        ConMenu.SetActive(true);
        PlayMenu.SetActive(false);
    }
    private void handleErrorLogin(int errorId, string Error)
    {
        switch(errorId)
        {
            case 1:
                ErrorLogin.gameObject.SetActive(true);
                ErrorLogin.text = Error;
                break;
        }
    }

    private void handleSucessLogin()
    {
        if (LoginSty.isOn is false )PlayerPrefs.SetInt("StayLogin",0);
        else PlayerPrefs.SetInt("StayLogin",1);
        ConMenu.SetActive(false);
        PlayMenu.SetActive(true);
        ChangeWelcomeText();
    }

}
