using _httpRequest;
using Assets.Script.Library.Request;
using Newtonsoft.Json;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
public class LogIn : MonoBehaviour
{
    public string URL = @"http://localhost:5292/auth/log";
    public TMP_InputField UsernameLogin;
    public TMP_InputField PasswordLogin;
    public async void OnLogInButton()
    {
        Debug.Log(UsernameLogin.text);
        Debug.Log(PasswordLogin.text);
        AuthRequest tempRequest = new AuthRequest() { Username = UsernameLogin.text, Password = PasswordLogin.text };
        var token = await HttpClient.Post<AuthResponse>(URL, tempRequest);
        Debug.Log(token.Token);
    }

    public IEnumerator LogInReq(AuthRequest request)
    {
        var uwr = new UnityWebRequest(URL, "POST");
        string jsonDATA =JsonConvert.SerializeObject(request);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonDATA);

        uwr.uploadHandler= (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler=(DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);
        }
    }
}
        