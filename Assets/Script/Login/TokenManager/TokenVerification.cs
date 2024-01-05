using Newtonsoft.Json;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using TokenManager;
using Login.Library.Resonses;

public class TokenVerification : MonoBehaviour
{
    private static string URL = @"https://localhost:7196/player/auth";
    public static bool VerifToken()
    {
        AuthResponse token =  TokenLoader.LoadToken();
        var uwr = new UnityWebRequest(URL, "POST");
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Authorization", "Bearer " + token.Token);
        uwr.SendWebRequest();
        while (!uwr.isDone) Thread.Sleep(10);
        if (uwr.result == UnityWebRequest.Result.Success) return true;
        return false;
    }
}
