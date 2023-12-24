using _httpRequest;
using Assets.Script.Library.Request;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TMPro;
using LoadAndSaveToken;
using UnityEngine;
using UnityEngine.Networking;
public class LogIn : MonoBehaviour
{
    public GameObject ConectionMenu;
    public GameObject PlayButton;
    public GameObject LogOutButton;
    void Start()
    {
        AuthResponse token = LoadAndSaveToken.LoadAndSaveToken.LoadSaved();
        if (token == null)
        {
            Debug.Log("No first connection register");
        }
        else
        {
            Debug.Log("Token found : " + token.Token);
            ConectionMenu.SetActive(false);
            PlayButton.SetActive(true);
            LogOutButton.SetActive(true);
        }
    }
    public GameObject ErrorConection;
    public TextMeshProUGUI _ErrorConection;
    public string URL = @"http://localhost:5292/auth/log";
    public TMP_InputField UsernameLogin;
    public TMP_InputField PasswordLogin;
    public void OnLogInButton()
    {
        Debug.Log(UsernameLogin.text);
        Debug.Log(PasswordLogin.text);  
        AuthRequest tempRequest = new AuthRequest() { Username = UsernameLogin.text, Password = PasswordLogin.text };
        AuthResponse token = LogInRequest(tempRequest);
        Debug.Log(token);
        if (token != null)
        {
            if (token.Token != null)
            {
                LoadAndSaveToken.LoadAndSaveToken.SaveToken(token);
                Debug.Log("Connection succes and token saved");
                ConectionMenu.SetActive(false);
                PlayButton.SetActive(true);
                LogOutButton.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Error see on line function connection string ");
        }
    }
    public void OnLogOutButton()
    {
        LoadAndSaveToken.LoadAndSaveToken.LogOut();
        ConectionMenu.SetActive(true);
        PlayButton.SetActive(false);
        LogOutButton.SetActive(false);
    }
    public AuthResponse LogInRequest(AuthRequest request)
    {
        var uwr = new UnityWebRequest(URL, "POST");
        var jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
        uwr.uploadHandler = new UploadHandlerRaw(jsonData);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SendWebRequest();
        return JsonConvert.DeserializeObject<AuthResponse>(uwr.downloadHandler.text);
        
    }
}
        