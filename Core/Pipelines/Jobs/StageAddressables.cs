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

namespace ThunderKit.Core.Pipelines.Jobs
{

    //TODO: Documentation for this
    [PipelineSupport(typeof(Pipeline)), RequiresManifestDatumType(typeof(AddressableSettings))]
    public class StageAddressables : PipelineJob
    {
        [Tooltip("The name of the file that will be produced. Path Reference compatible. If left empty, will default to \"catalog.json\"")]
        [PathReferenceResolver]
        public string catalogName = "<ManifestName>.json";

        [Tooltip("The key of the catalog, used for loading the bundle from the settings.json file. Path Reference compatible. Cannot be named \"AddressablesMainContentCatalog\".")]
        [PathReferenceResolver]
        public string catalogID = "<ManifestName>CatalogContent";

        [Tooltip("Uses Addressables Path resolving. Use it to point to a static string property within your mod's assembly. It should point to the directory of your catalog. This should not be left as the default.")]
        public string localLoadPath = "{UnityEngine.AddressableAssets.Addressables.RuntimePath}";

        public bool OverrideManifest;
        public Pipeline targetpipeline;
        public override async Task Execute(Pipeline pipeline)
        {
            AssetDatabase.SaveAssets();
            var manifests = pipeline.Manifests;
            var ads = new List<AddressableSettings>();
            for (int i = 0; i < manifests.Length; i++)
            {
                var adsData = manifests[i].Data.OfType<AddressableSettings>().FirstOrDefault();
                if (adsData)
                    ads.Add(adsData);
            }

            foreach (var adsData in ads)
            {
                
            }

        }
    }
}
