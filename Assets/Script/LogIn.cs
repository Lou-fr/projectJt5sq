using _httpRequest;
using Assets.Script.Library.Request;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
public class LogIn : MonoBehaviour
{
    private void Start()
    {
        AuthResponse token = loadSaved();
        if (token == null)
        {
            Debug.Log("No first connection register");
        }
        else
        {
            Debug.Log("Token found" + token.Token);
        }
    }
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
        if (token.Token != null)
        {
            SaveToken(token);
            Debug.Log("Connection succes and token saved");
        }
        else
        {
            Debug.LogError("Password or Username incorect");
        }
    }
    public static void SaveToken(AuthResponse response)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.info";
        FileStream stream = new FileStream(path, FileMode.Create);
        Debug.Log(path);
        
        bf.Serialize(stream,response);
        stream.Close();
    }
    public static AuthResponse loadSaved()
    {
        string path = Application.persistentDataPath + "/player.info";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);
            AuthResponse data = bf.Deserialize(stream) as AuthResponse;
            return data;
        }
        else
        {
            return null;
        }
    }
}
        