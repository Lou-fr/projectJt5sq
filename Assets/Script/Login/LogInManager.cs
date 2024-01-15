using Login.Library.Request;
using Login.Library.Resonses;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using TokenManage;
using GetLocal;
using TempTokenManager;
public class LogInManager : MonoBehaviour
{
    public bool LogInSucces = false;
    [SerializeField] TextMeshProUGUI _Username;
    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject ConnectionMenu;
    [SerializeField] GameObject LogoutButton;
    [SerializeField] UnityEngine.UI.Toggle StayLogIn;
    [SerializeField] TextMeshProUGUI ErrorConnection;
    public AuthResponse token;
    private string URL = @"https://localhost:7196/auth/log";
    [SerializeField] TMP_InputField Username;
    [SerializeField] TMP_InputField Password;
    // Start is called before the first frame update
    void Start()
    {
        token = TokenLoader.LoadToken();
        if (token != null)this.gameObject.SetActive(false); this.enabled = false;
    }
    public void OnLoginButton()
    {
        AuthRequest TempRequest = new AuthRequest() { Username = Username.text, Password = Password.text};
        token = LogInRequest(TempRequest);
        if (token != null)
        {
            if (token.Token !=null)
            {
                LogInSucces = true;
                string temp = GetLocalal.GetString("StartScreen", "Welcome");
                _Username.text = temp + Username.text;
                _Username.gameObject.SetActive(true); ConnectionMenu.SetActive(false); LogoutButton.SetActive(true); PlayButton.SetActive(true);
                if (StayLogIn.isOn)
                {
                    TokenLoader.SaveToken(token); 
                }
                this.gameObject.SetActive(false);
                this.enabled = false;
            }
        }
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
