using System.Collections;
using System.Collections.Generic;
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
        for(int i = 0;i< Object.FindObjectsOfType<DontDestroy>().Length;i++)
        {
            if(Object.FindObjectsOfType<DontDestroy>()[i] != this)
            {
                if (Object.FindObjectsOfType<DontDestroy>()[i].ObjectId ==ObjectId)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
