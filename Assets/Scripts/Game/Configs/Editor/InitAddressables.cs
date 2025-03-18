using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace Game.Configs.Editor
{
    /// <summary>
    /// This class is a workaround for a Unity bug I spotted.
    /// When the asset is about to be imported via unitypackage, the Addressabless checkbox is absent
    /// This might be due wasting refence with current Group, on importer Side.
    /// </summary>
    public class InitAddressablesSetup
    {
        [InitializeOnLoadMethod]
        private static void OnEditorLoad()
        {
            CreateAddressablesSettings();
        }

        private static void CreateAddressablesSettings()
        {
            AddressableAssetSettingsDefaultObject.Settings = AddressableAssetSettings.Create(
                AddressableAssetSettingsDefaultObject.kDefaultConfigFolder,
                AddressableAssetSettingsDefaultObject.kDefaultConfigAssetName, true, true);

            AssetDatabase.SaveAssets();
        }
    }
}