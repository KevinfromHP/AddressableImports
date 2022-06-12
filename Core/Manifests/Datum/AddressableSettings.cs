using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace ThunderKit.Core.Manifests.Datum
{
    //TODO: Documentation
    public class AddressableSettings : ManifestDatum
    {
        [Header("Note: Building multiple addressable catalogs is not supported with the Addressables system.")]
        public AddressableAssetSettings addressableAssetSettings;
    }
}
