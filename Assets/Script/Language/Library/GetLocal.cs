using UnityEngine;
using UnityEngine.Localization.Settings;

namespace GetLocal
{
    public static class GetLocalal
    {
        public static string GetString(string tableName, string entryName)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(tableName, entryName);
        }
    }
}