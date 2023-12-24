using Login.Library.Request;
using Login.Library.Resonses;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class LogInManager : MonoBehaviour
{
    public GameObject ConnectionMenu;
    public GameObject LogoutButton;
    public TextMeshProUGUI ErrorConnection;
    public string URL = @"https://localhost:7196/auth/log:";
    public TMP_InputField Username;
    public TMP_InputField Password;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnLoginButton()
    {
        AuthRequest TempRequest = new AuthRequest() { Username = Username.text, Password = Password.text};
        var Token = LogInRequest(TempRequest);
        if (Token != null)
        {
            if (Token !=null)
            {
                Debug.Log(Token.Token);
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
            Debug.Log("auth receive and token transmision");
            return JsonConvert.DeserializeObject<AuthResponse>(uwr.downloadHandler.text);
        }
        else
        {
            ErrorConnection.gameObject.SetActive(true);
            if (uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogWarning(uwr.downloadHandler.text);
                ErrorConnection.text = uwr.downloadHandler.text;
            }
            else
            {
                Debug.LogError(uwr.error);
                ErrorConnection.text = uwr.error;
            }
            return null;
        }
    }
}
