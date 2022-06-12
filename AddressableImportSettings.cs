using System;
using System.IO;
using ThunderKit.Core.Data;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UIElements;
using System.Linq;
using ThunderKit.Core.Attributes;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets;

namespace ThunderKit.Addressable.AddressableImports
{

    public class AddressableImportSettings : ThunderKitSetting
    {
        public static event EventHandler AddressablesInitialized;

        [InitializeOnLoadMethod]
        public static void OnLoad()
        {
            Addressables.InternalIdTransformFunc = RedirectInternalIdsToGameDirectory;
            CompilationPipeline.compilationStarted -= ClearSelectionIfUnsavable;
            CompilationPipeline.compilationStarted += ClearSelectionIfUnsavable;
            InitializeAddressables();
        }

        static string RedirectInternalIdsToGameDirectory(IResourceLocation location)
        {
            var resourceLocator = Addressables.ResourceLocators.Where(locator => locator.Keys.Contains(location)).SingleOrDefault(); // this really shouldn't need the default
            //Debug.Log(Addressables.ResourceLocators.Count());
            switch (location.ResourceType)
            {
                case var t when t == typeof(IAssetBundleResource):
                    var iid = location.InternalId;


                    var path = iid.Substring(iid.IndexOf("/aa") + 4);
                    path = Path.Combine(ThunderKitSettings.EditTimePath, path);
                    return path;
                default:
                    var result = location.InternalId;
                    return result;
            }
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

        static void InitializeAddressables()
        {
            //if (!AddressableHelper.SettingsExist())
            //{
            //    Debug.LogWarning("settings.json file not found. Generating...");
            //    var db = ScriptableObject.CreateInstance<BuildScriptFastMode>();
            //    db.BuildData<AddressableAssetBuildResult>(new AddressablesDataBuilderInput(AddressableAssetSettingsDefaultObject.Settings));
            //}

            //var ao = Addressables.InitializeAsync();
            //ao.WaitForCompletion();

            ImportAdditionalCatalogs();

            //AddressablesInitialized?.Invoke(null, EventArgs.Empty);
            
        }
        private static void ImportAdditionalCatalogs()
        {
            //var gameRuntimeDataLocation = new ResourceLocationBase("GameRuntimeData", ThunderKitSettings.EditTimePath, typeof(JsonAssetProvider).FullName, typeof(UnityEngine.AddressableAssets.Initialization.ResourceManagerRuntimeData));
            //Addressables.LoadResourceLocationsAsync
        }

    }
}
