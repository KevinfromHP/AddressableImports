using System.IO;
using ThunderKit.Core.Data;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace ThunderKit.Addressable
{
    public static class AddressableHelper
    {
        public static string SettingsPath => Path.Combine(Addressables.RuntimePath, "settings.json");

        public static bool SettingsExist()
        {
            return File.Exists(Path.Combine(Directory.GetCurrentDirectory(), SettingsPath));
        }

        public static bool CatalogExists(string catalogName = "catalog")
        {
            return File.Exists(Path.Combine(Directory.GetCurrentDirectory(), Addressables.RuntimePath, $"{catalogName}.json"));
        }

        public static string RedirectInternalIDToGame(string internalId)
        {
            var resolvedId = internalId.Replace(Addressables.RuntimePath, Application.streamingAssetsPath + "/aa");
            return Path.Combine(ThunderKitSettings.EditTimePath, resolvedId.Substring(resolvedId.IndexOf("/aa") + 4));
        }
    }
}
