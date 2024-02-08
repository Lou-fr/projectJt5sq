using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using Login.Library.Resonses;

namespace TokenManage
{
    public class TokenVerification : MonoBehaviour
    {
        private static string URL = @"https://5.48.12.31:7196/player/auth";
        public static (bool succes, string content, UnityWebRequest.Result Response) VerifToken(Tokens token)
        {
            var uwr = new UnityWebRequest(URL, "POST");
            uwr.certificateHandler = (CertificateHandler)new BypassCertificate();
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("Authorization", "Bearer " + token.Token);
            uwr.SendWebRequest();
            while (!uwr.isDone) Thread.Sleep(10);
            if (uwr.result == UnityWebRequest.Result.Success) return (true, uwr.downloadHandler.text, uwr.result);
            return (false, "", uwr.result);
        }
    }
}
