using Login.Library.Resonses;
using UnityEngine;

public class Test : MonoBehaviour
{
    private TokenManager tokenManager;
    private Tokens token;
    // Start is called before the first frame update
    void Start()
    {
        tokenManager = FindAnyObjectByType<TokenManager>();
        token = tokenManager.token;
        Debug.Log(token.Token);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
