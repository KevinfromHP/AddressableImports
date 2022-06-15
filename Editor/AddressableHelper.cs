﻿using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace ThunderKit.Addressable
{
    public static class AddressableHelper
    {
        public static string SettingsPath => Path.Combine(Addressables.RuntimePath, "settings.json");

        public static bool SettingsExist()
        {
            return File.Exists(Path.Combine(Application.dataPath, SettingsPath));
        }

        public static bool CatalogExists(string catalogName = "catalog")
        {
            return File.Exists(Path.Combine(Application.dataPath, Addressables.RuntimePath, $"{catalogName}.json"));
        }

        //public static string[] GetCatalogsFromSettings(string settingsPath)
        //{
            
        //}
    }
}
