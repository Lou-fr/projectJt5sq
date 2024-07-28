using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Sensivity_Controller : MonoBehaviour
{
    SensivityReader reader;
    private TextMeshProUGUI SensibilityStrenght;
    private Slider SensivitySlider;
    public static float Sensivity {get; private set;} =1;
    public static Action MenuReady =delegate{};

    void Start()
    {
#if !UNITY_STANDALONE
        MenuReady?.Invoke();
#endif
#if UNITY_STANDALONE
        reader = GameObject.FindFirstObjectByType<SensivityReader>().GetComponent<SensivityReader>();
        SensibilityStrenght = reader.textSensivity;
        SensivitySlider = reader.SensivitySlide;
        SensivitySlider.onValueChanged.AddListener(HandleSensivityChange);
        if(PlayerPrefs.HasKey("Sensivity")){Sensivity = (float)Math.Round(PlayerPrefs.GetFloat("Sensivity"),2);SensivitySlider.value = Sensivity;SensibilityStrenght.text =  Sensivity.ToString();Sensivity=Sensivity;}else {SensivitySlider.value = Sensivity;SensibilityStrenght.text =  Sensivity.ToString();}
        Debug.LogWarning(Sensivity,this);
        MenuReady?.Invoke();
#endif
    }
#if UNITY_STANDALONE
    void HandleSensivityChange(float value)
    {
        Sensivity = (float)Math.Round(value, 2);
        PlayerPrefs.SetFloat("Sensivity", value);
        SensibilityStrenght.text = Sensivity.ToString();
    }
#endif
}
