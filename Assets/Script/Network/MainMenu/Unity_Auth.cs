using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class Unity_Auth : MonoBehaviour
{
    async void Awake()
	{
        int i = 0;
		try
		{
			await UnityServices.InitializeAsync();
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}
        if (PlayerPrefs.HasKey("StayLogin")){ i = PlayerPrefs.GetInt("StayLogin");}
        if (AuthenticationService.Instance.IsSignedIn){OnSucess?.Invoke();return;}
        if(AuthenticationService.Instance.SessionTokenExists && i == 1 )
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            OnSucess?.Invoke();
        }
	}
    public static Action OnSucess = delegate {};
    public static Action<int,string> OnError = delegate {};
    public static Action<PlayerInfo> OnInfoSucess = delegate {};
    public static async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username,password);
            OnSucess?.Invoke();
            Debug.Log("Sign In Succes");
        }
        catch (AuthenticationException ex)
        {
            OnError?.Invoke(1,ex.ToString());
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            OnError?.Invoke(2,ex.ToString());
        }
    }
    public static async Task GetPlayerInfo()
    {
        try
        {
            PlayerInfo player = await AuthenticationService.Instance.GetPlayerInfoAsync();
            OnInfoSucess?.Invoke(player);
            
        }catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
    public static async Task<PlayerInfo> _GetPlayerInfo()
    {
        try
        {
            PlayerInfo player = await AuthenticationService.Instance.GetPlayerInfoAsync();
            return player;
            
        }catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }
}
