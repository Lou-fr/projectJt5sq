using System;
using BleizEntertainment.RebindUI;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset actions;
    public static Action uppdateBinding = delegate {};

    public void OnEnable()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);
    }

    void Awake()
    {
        RebindActionUI.UpdateBindings += updatebidings;
    }
    void OnDestroy()
    {
        RebindActionUI.UpdateBindings -= updatebidings;
    }

    private void updatebidings()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        uppdateBinding?.Invoke();
    }

    /*public void OnDisable()
    {
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }*/
}
