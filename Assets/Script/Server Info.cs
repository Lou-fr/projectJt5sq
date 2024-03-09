using BleizEntertainment.Multiplayer;
using UnityEngine;

namespace BleizEntertainment.Multiplayer
{
    public class Session : MonoBehaviour
    {
        public static string Ip;
        public static int port;
        // Start is called before the first frame update
        void Start()
        {
            Ip = PlayerMultiplayerRequester.Ip;
            port = PlayerMultiplayerRequester.Port;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}