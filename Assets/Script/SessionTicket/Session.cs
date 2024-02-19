using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour
{
    [SerializeField] private PlayfabLogin playfabLogin;
    public static string Sessionticket;
    // Start is called before the first frame update
    void Start()
    {
        Sessionticket = playfabLogin.SessionTicket;
        DontDestroyOnLoad(this.gameObject);
    }
}
