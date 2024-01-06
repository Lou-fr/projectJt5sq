using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;


public class SelectLanguage : MonoBehaviour
{
    private bool active = false;
    public TMP_Dropdown dropdown;
    IEnumerator Start()
    {
        int selected = 0;
        yield return LocalizationSettings.SelectedLocale;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
            {
                selected = i;
            }
            dropdown.value = selected;
        }
    }
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
