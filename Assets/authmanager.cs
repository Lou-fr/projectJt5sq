using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;

public class authmanager : MonoBehaviour
{
    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void SignIn()
    {
        await signInAnonymous();
    }
    async Task signInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in succes as" + AuthenticationService.Instance.PlayerId);
        }
        catch(AuthenticationException ex) 
        {
            Debug.LogException(ex);
        }
    }
}
