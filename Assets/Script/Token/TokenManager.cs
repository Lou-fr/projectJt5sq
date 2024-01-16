using Login.Library.Resonses;
using UnityEngine;
public class TokenManager : MonoBehaviour
{
    [SerializeField] TempTokenManager.TokenManager manager;
    public Tokens token;
    // Start is called before the first frame update
    void Start()
    {
        token = manager.token;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
