
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Graphics : MonoBehaviour
{
    [SerializeField] Toggle Vsync, F_S;
    [SerializeField] TMP_Dropdown Dresolution;
    [SerializeField] TMP_Dropdown DAAMode;
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
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        Dresolution.AddOptions(options);
        Dresolution.value = currentResolutionIndex;
        options.Clear();
        for (int i = 0;i < 4; i++)
        {
            if(i == QualitySettings.antiAliasing)
            {
                DAAMode.value = i;
            }
        }
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
    }
}
