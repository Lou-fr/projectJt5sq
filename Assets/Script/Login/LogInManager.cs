using Login.Library.Request;
using Login.Library.Resonses;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using TokenManager;
using TokenVerfication;
using GetLocal;
public class LogInManager : MonoBehaviour
{
    public TextMeshProUGUI _Username;
    public GameObject ErrorTokenPopup;
    public GameObject PlayButton;
    public GameObject LogOutConfirm;
    public GameObject ConnectionMenu;
    public GameObject LogoutButton;
    public UnityEngine.UI.Toggle StayLogIn;
    public TextMeshProUGUI ErrorConnection;
    public string URL = @"https://localhost:7196/auth/log";
    public TMP_InputField Username;
    public TMP_InputField Password;
    // Start is called before the first frame update
    void Start()
    {
        TokenLoader.UnloadTempToken();
        AuthResponse token = TokenLoader.LoadToken();
        if (token != null)
        {
            var (succes, _username) = TokenVerification.VerifToken();
            if ((succes) == (true))
            {
                string temp = GetLocalal.GetString("StartScreen", "Welcome");
                _Username.text = temp+_username;
                _Username.gameObject.SetActive(true);
                ConnectionMenu.SetActive(false); LogoutButton.SetActive(true); PlayButton.SetActive(true);
            }
            else
            {
                ConnectionMenu.SetActive(false); ErrorTokenPopup.SetActive(true); _Username.gameObject.SetActive(false);
            }
        }
    }
    public void OnAcknowledgeButton()
    {
        TokenLoader.UnloadToken();
        ConnectionMenu.SetActive(true); ErrorTokenPopup.SetActive(false);
    }
    public void OnLoginButton()
    {
        AuthRequest TempRequest = new AuthRequest() { Username = Username.text, Password = Password.text};
        var Token = LogInRequest(TempRequest);
        if (Token != null)
        {
            if (Token.Token !=null)
            {
                if (StayLogIn.isOn)
                {
                    TokenLoader.SaveToken(Token); 
                    string temp = GetLocalal.GetString("StartScreen", "Welcome");
                    _Username.text = temp + Username.text;
                    _Username.gameObject.SetActive(true);ConnectionMenu.SetActive(false); LogoutButton.SetActive(true); PlayButton.SetActive(true);
                }
                else
                {
                    TokenLoader.SaveTokenTemp(Token);
                    string temp = GetLocalal.GetString("StartScreen", "Welcome");
                    _Username.text = temp + Username.text;
                    _Username.gameObject.SetActive(true);ConnectionMenu.SetActive(false); LogoutButton.SetActive(true); PlayButton.SetActive(true);
                }
                
            }
        }
    }
    public void OnLogoutButton()
    {
        ConnectionMenu.SetActive(false); LogoutButton.SetActive(false); PlayButton.SetActive(false);LogOutConfirm.SetActive(true); _Username.gameObject.SetActive(false);

    }
    public void OnLogOutConfirmButton()
    {
        TokenLoader.UnloadTempToken();
        TokenLoader.UnloadToken();
        ConnectionMenu.SetActive(true); LogOutConfirm.SetActive(false);
    }
    public void OnLogoutCancelButton()
    {
        LogoutButton.SetActive(true); PlayButton.SetActive(true); LogOutConfirm.SetActive(false); _Username.gameObject.SetActive(true);
    }
    public AuthResponse LogInRequest(AuthRequest request)
    {
        var uwr = new UnityWebRequest(URL,"POST");
        var JsonData= Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(JsonData);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SendWebRequest();
        while (!uwr.isDone) Thread.Sleep(10);
        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.responseCode);
            ErrorConnection.gameObject.SetActive(false);
            return JsonConvert.DeserializeObject<AuthResponse>(uwr.downloadHandler.text);
        }
        else
        {
            ErrorConnection.gameObject.SetActive(true);
            if (uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning(uwr.responseCode);
                ErrorConnection.text = uwr.downloadHandler.text;
            }
            else
            {
                Debug.LogError(uwr.responseCode);
                ErrorConnection.text = uwr.error;
            }
            return null;
        }
    }
}
