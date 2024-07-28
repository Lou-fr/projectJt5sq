using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public string ObjectId;
    void Awake()
    {
        ObjectId = name + transform.position.ToString();
    }
    void Start()
    {
        for (int i = 0; i < FindObjectsByType<DontDestroy>(FindObjectsSortMode.None).Length; i++)
        {
            if (FindObjectsByType<DontDestroy>(FindObjectsSortMode.None)[i] != this)
            {
                if (FindObjectsByType<DontDestroy>(FindObjectsSortMode.None)[i].ObjectId == ObjectId)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
