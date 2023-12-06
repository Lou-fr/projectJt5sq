using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        Debug.Log(UnityServices.State);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
