using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;


public class SelectLanguage : MonoBehaviour
{
    private bool active = false;
    public TMP_Dropdown dropdown;
    public void OnSubmit()
    {
        ChangeLocale(dropdown.value);
    }
    public void ChangeLocale(int LocaleID)
    {
        if (active == true) return;
        StartCoroutine(SetLocale(LocaleID));
    }
    IEnumerator SetLocale(int _LocaleID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_LocaleID];
        active = false;
    }
}
