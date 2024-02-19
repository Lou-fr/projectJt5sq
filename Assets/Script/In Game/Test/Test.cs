using UnityEngine;

public class Test : MonoBehaviour
{
    private string sessionTicket;
    // Start is called before the first frame update
    void Start()
    {
        Session session = new Session();
        sessionTicket = Session.Sessionticket;
        Debug.Log(sessionTicket);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
