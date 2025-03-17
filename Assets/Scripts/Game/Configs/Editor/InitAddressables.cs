using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace Game.Configs.Editor
{
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