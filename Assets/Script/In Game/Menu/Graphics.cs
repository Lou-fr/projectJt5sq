
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Graphics : MonoBehaviour
{
    [SerializeField] Toggle Vsync, F_S, fps;
    [SerializeField] TMP_Dropdown Dresolution, D_FPS, DAAMode;
    [SerializeField] GameObject fps_counter;
    [SerializeField] UniversalAdditionalCameraData CameraData;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
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
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
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
                CurrentIndex = ((int)i);
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
        Resolution resolution = resolutions[Dresolution.value];
        Screen.SetResolution(resolution.width, resolution.height, F_S.isOn);
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
}
