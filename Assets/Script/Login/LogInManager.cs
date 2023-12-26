using Login.Library.Request;
using Login.Library.Resonses;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public class LogInManager : MonoBehaviour
{
    public GameObject PlayButton;
    public GameObject LogOutConfirm;
    public GameObject ConnectionMenu;
    public GameObject LogoutButton;
    public TextMeshProUGUI ErrorConnection;
    public string URL = @"https://localhost:7196/auth/log";
    public TMP_InputField Username;
    public TMP_InputField Password;
    // Start is called before the first frame update
    void Start()
    {
        AuthResponse token = TokenManager.TokenManager.LoadToken();
        if (token != null)
        {
            ConnectionMenu.SetActive(false); LogoutButton.SetActive(true); PlayButton.SetActive(true);
        }
    }
    public void OnLoginButton()
    {
        AuthRequest TempRequest = new AuthRequest() { Username = Username.text, Password = Password.text};
        var Token = LogInRequest(TempRequest);
        if (Token != null)
        {
            if (Token !=null)
            {
                TokenManager.TokenManager.SaveToken(Token);
                ConnectionMenu.SetActive(false);LogoutButton.SetActive(true);PlayButton.SetActive(true);
            }
        }
    }
    public void OnLogoutButton()
    {
        ConnectionMenu.SetActive(false); LogoutButton.SetActive(false); PlayButton.SetActive(false);LogOutConfirm.SetActive(true);

    }
    public void OnLogOutConfirmButton()
    {
        TokenManager.TokenManager.UnloadToken();
        ConnectionMenu.SetActive(true);LogOutConfirm.SetActive(false);
    }
    public void OnLogoutCancelButton()
    {
        LogoutButton.SetActive(true); PlayButton.SetActive(true); LogOutConfirm.SetActive(false);
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
