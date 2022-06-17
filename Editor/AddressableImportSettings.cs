using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThunderKit.Addressable.Builders;
using ThunderKit.Core.Data;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.Initialization;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UIElements;

namespace ThunderKit.Addressable
{

    public class AddressableImportSettings : ThunderKitSetting
    {
        public static event EventHandler AddressablesInitialized;

        public static List<IResourceLocator> GameResourceLocators;

        [InitializeOnLoadMethod]
        public static void OnLoad()
        {
            InitializeAddressables();
            CompilationPipeline.compilationStarted -= ClearSelectionIfUnsavable;
            CompilationPipeline.compilationStarted += ClearSelectionIfUnsavable;
            Addressables.InternalIdTransformFunc += RedirectInternalIdsToGameDirectory;
        }

        static string RedirectInternalIdsToGameDirectory(IResourceLocation location)
        {
            if (location.ResourceType == typeof(IAssetBundleResource) && GameResourceLocators != null)
            {
                var gameLocators = GameResourceLocators;
                if (gameLocators.Any(gameLocator => gameLocator.Keys.Contains(location.PrimaryKey)))
                    return AddressableHelper.RedirectInternalIDToGame(location.InternalId);
            }
            return location.InternalId; ;
        }

        private static void ClearSelectionIfUnsavable(object obj)
        {
            if (!Selection.activeObject) return;
            if (Selection.activeObject.hideFlags.HasFlag(HideFlags.DontSave))
                Selection.activeObject = null;
        }

        public override void CreateSettingsUI(VisualElement rootElement)
        {
            base.CreateSettingsUI(rootElement);
            rootElement.Add(new Button(InitializeAddressables) { text = "Reload" });
        }

        //[MenuItem("Tools/Initialize Addressables")]
        static void InitializeAddressables()
        {
            if (!AddressableHelper.SettingsExist())
            {
                Debug.LogWarning("settings.json file not found. Generating...");
                var buildScript = new ModdedBuildScriptPackedMode("catalog", "ModdedCatalogContent", "{UnityEngine.AddressableAssets.Addressables.RuntimePath}", null);
                buildScript.BuildData<AddressableAssetBuildResult>(new AddressablesDataBuilderInput(AddressableAssetSettingsDefaultObject.Settings));
            }
            PlayerPrefs.SetString(Addressables.kAddressablesRuntimeDataPath, Addressables.RuntimePath + "/settings.json");

            var ao = Addressables.InitializeAsync();
            ao.WaitForCompletion();

            ImportAdditionalCatalogLocations();

            AddressablesInitialized?.Invoke(null, EventArgs.Empty);
        }

        private static void ImportAdditionalCatalogLocations()
        {
            var tkSettings = GetOrCreateSettings<ThunderKitSettings>();

            var resourceLocators = new List<IResourceLocator>();
            foreach (var catalogLocation in JsonUtility.FromJson<ResourceManagerRuntimeData>(File.ReadAllText(tkSettings.AddressableAssetsSettings)).CatalogLocations)
            {
                var iid = Addressables.ResolveInternalId(catalogLocation.InternalId.Replace("{UnityEngine.AddressableAssets.Addressables.RuntimePath}", "{ThunderKit.Core.Data.ThunderKitSettings.EditTimePath}"));
                //This needs testing with remote catalogs, but that's annoying to set up.
                var ao = Addressables.LoadContentCatalogAsync(iid);
                var locator = ao.WaitForCompletion();
                resourceLocators.Add(locator);
            }
            GameResourceLocators = resourceLocators;
        }
    }
}
