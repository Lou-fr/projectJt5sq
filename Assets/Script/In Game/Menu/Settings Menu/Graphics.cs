
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Graphics : MonoBehaviour
{
#if !UNITY_SERVER
    [SerializeField] private GameObject fps_counter;
    [SerializeField] private UniversalAdditionalCameraData CameraData;

#if UNITY_STANDALONE
    private Toggle Vsync, F_S, fps;
    private TMP_Dropdown Dresolution, D_FPS, DAAMode;
    Resolution[] resolutions;
    private List<Resolution> filteredResolution = new List<Resolution>();
    // Start is called before the first frame update
    void Start()
    {
        Vsync = GameObject.Find("Gr_C_Vsync").GetComponentInChildren<Toggle>();
        F_S = GameObject.Find("Gr_C_FS").GetComponentInChildren<Toggle>();
        fps = GameObject.Find("Gr_C_See_FPS").GetComponentInChildren<Toggle>();
        Dresolution = GameObject.Find("Gr_C_Resolution").GetComponentInChildren<TMP_Dropdown>();
        D_FPS = GameObject.Find("Gr_C_FPS").GetComponentInChildren<TMP_Dropdown>();
        DAAMode = GameObject.Find("Gr_C_AA").GetComponentInChildren<TMP_Dropdown>();
        Button aplychange = GameObject.Find("Gr_C_ApChange").GetComponentInChildren<Button>();
        GameObject.Find("Gr_computer(Clone)").SetActive(false);
        aplychange.onClick.AddListener(ApplyChange);
        F_S.isOn = Screen.fullScreen;
        if (QualitySettings.vSyncCount == 0)
        {
            Vsync.isOn = false;
        }
        else
        {
            Vsync.isOn = true;
        }
        resolutions = Screen.resolutions;

        List<string> options = new List<string>();
        int CurrentIndex = 0;
        foreach(Resolution res in resolutions)
        {
            string option = res.width.ToString() + "x" + res.height.ToString();
            if(!options.Contains(option))
            {
                options.Add(option);
                filteredResolution.Add(res);
            }
        }
        for (int i = 0; i < filteredResolution.Count; i++)
        {
            if (filteredResolution[i].width == Screen.currentResolution.width && filteredResolution[i].height == Screen.currentResolution.height)
            {
                CurrentIndex = i;
            }
        }
        Dresolution.AddOptions(options);
        Dresolution.value = CurrentIndex;
        options.Clear();
        CurrentIndex = 0;
        for (AntialiasingMode i = 0;i < AntialiasingMode.TemporalAntiAliasing; i++)
        {
            if(i == CameraData.antialiasing)
            {
                CurrentIndex = (int)i;
            }
        }
        DAAMode.value = CurrentIndex;
        if(Application.targetFrameRate == 60)D_FPS.value = 1;
    }
    public void ApplyChange()
    {
        if (Vsync.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        int resolution = Dresolution.value;
        Screen.SetResolution(filteredResolution[resolution].width, filteredResolution[resolution].height, F_S.isOn);
        fps_counter.SetActive(fps.isOn);
        switch (DAAMode.value)
        {
            case 0:
                CameraData.antialiasing = AntialiasingMode.None;
                break;
            case 1:
                CameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing; 
                break;
            case 2:
                CameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing; 
                break;
            case 3:
                CameraData.antialiasing = AntialiasingMode.TemporalAntiAliasing;
                break;
        }
        if (D_FPS.value == 1)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 30;
        }
    }
#endif
#if UNITY_ANDROID || UNITY_IOS
    private Toggle fps;
    private TMP_Dropdown D_FPS, DAAMode;
    void Start()
    {
        fps = GameObject.Find("Gr_P_See_FPS").GetComponentInChildren<Toggle>();
        D_FPS = GameObject.Find("Gr_P_FPS").GetComponentInChildren<TMP_Dropdown>();
        DAAMode = GameObject.Find("Gr_P_AA").GetComponentInChildren<TMP_Dropdown>();
        Button aplychange = GameObject.Find("Gr_P_ApChange").GetComponentInChildren<Button>();
        GameObject.Find("Gr_phone(Clone)").SetActive(false);
        GameObject.Find("Main_Menu_Panel").SetActive(false);
        int CurrentIndex = 0;
        for (AntialiasingMode i = 0; i < AntialiasingMode.TemporalAntiAliasing; i++)
        {
            if (i == CameraData.antialiasing)
            {
                CurrentIndex = ((int)i);
            }
        }
        DAAMode.value = CurrentIndex;
        aplychange.onClick.AddListener(ApplyPhoneChange);

        if (Application.targetFrameRate == 60) D_FPS.value = 1;
    }
    public void ApplyPhoneChange()
    {
        fps_counter.SetActive(fps.isOn);
        switch (DAAMode.value)
        {
            case 0:
                CameraData.antialiasing = AntialiasingMode.None;
                break;
            case 1:
                CameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                break;
            case 2:
                CameraData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                break;
            case 3:
                CameraData.antialiasing = AntialiasingMode.TemporalAntiAliasing;
                break;
        }
        if (D_FPS.value == 1)
        {
            Application.targetFrameRate = 60;
        }
        else
        {
            Application.targetFrameRate = 30;
        }
    }
#endif
#endif
}
