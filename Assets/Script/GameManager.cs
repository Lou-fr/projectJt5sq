using UnityEngine;
using _httpRequest;
using SharedLibrary;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        var player = await HttpClient.Get<Player>("https://localhost:44381/player/500");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
