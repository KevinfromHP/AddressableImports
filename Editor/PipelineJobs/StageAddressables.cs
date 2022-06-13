using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ThunderKit.Core.Attributes;
using System.Threading.Tasks;
using ThunderKit.Core.Data;
using ThunderKit.Core.Manifests.Datum;
using ThunderKit.Core.Paths;
using ThunderKit.Core.Pipelines;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Build;
using ThunderKit.Addressable.Manifest;
using ThunderKit.Addressable.Builders;

namespace ThunderKit.Addressable.PipelineJobs
{

    //TODO: Documentation for this
    [PipelineSupport(typeof(Pipeline)), RequiresManifestDatumType(typeof(AddressableSettings))]
    public class StageAddressables : PipelineJob
    {
        [Tooltip("The name of the catalog file that will be produced. Path Reference compatible. If left empty, will default to \"catalog\".")]
        [PathReferenceResolver]
        public string catalogName = "<ManifestName>";

        [Tooltip("The key of the catalog, used for loading the bundle from the settings.json file. Path Reference compatible. Cannot be named \"AddressablesMainContentCatalog\".")]
        [PathReferenceResolver]
        public string catalogID = "Addressables<ManifestName>CatalogContent";

        [Tooltip("Uses Addressables Path resolving. Use it to point to a static string property within your mod's assembly. It should point to the directory of your catalog (not the path). This should not be left as the default.")]
        public string localLoadDirectory = "{UnityEngine.AddressableAssets.Addressables.RuntimePath}";

        [HideInInspector]
        public BuildType buildType = BuildType.Default;

        public override Task Execute(Pipeline pipeline)
        {
            AssetDatabase.SaveAssets();
            var manifests = pipeline.Manifests;
            var ads = new List<AddressableSettings>();
            for (int i = 0; i < manifests.Length; i++)
                ads.Add(manifests[i].Data.OfType<AddressableSettings>().FirstOrDefault());


            string catName = !string.IsNullOrEmpty(catalogName) ? PathReference.ResolvePath(catalogName, pipeline, this).Replace(" ", "") : "catalog";
            var builderInput = new AddressablesDataBuilderInput(AddressableAssetSettingsDefaultObject.Settings)
            {
                RuntimeCatalogFilename = $"{catName}.json",
            };
            string id = PathReference.ResolvePath(catalogID, pipeline, this).Replace(" ", "");

            ModdedBuildScriptBase buildScript;
            switch(buildType)
            {
                default:
                    buildScript = new ModdedBuildScriptPackedMode(catName, id, localLoadDirectory, pipeline);
                    break;
            }
            var result = buildScript.BuildData<AddressableAssetBuildResult>(builderInput);

            return Task.CompletedTask;
        }

        public enum BuildType
        {
            Default
        }

    }
}
