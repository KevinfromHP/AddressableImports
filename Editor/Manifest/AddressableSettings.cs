using ThunderKit.Core.Manifests;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace ThunderKit.Addressable.Manifest
{
    //TODO: Documentation
    public class AddressableSettings : ManifestDatum
    {
        [Header("Note: Building multiple addressable catalogs is not supported with the Addressables system.")]
        public AddressableAssetSettings addressableAssetSettings;
    }
}
