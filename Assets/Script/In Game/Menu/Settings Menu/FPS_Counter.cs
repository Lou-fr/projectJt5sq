using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    private float fps;
    [SerializeField] private TextMeshProUGUI FPS;
    private void Start()
    {
        InvokeRepeating("GetFps", 1, 0.5f);
    }
    void GetFps()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        FPS.text = fps.ToString();
    }
}
