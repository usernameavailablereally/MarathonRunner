using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Services.Loaders.Configs
{
    // ScriptableObject makes possible to easily edit parameters inside Unity 
    [CreateAssetMenu(fileName = "MatchConfig", menuName = "Configs/MatchConfig")]
    [Serializable]
    public class MatchConfig : ScriptableObject
    {
        public AssetReference[] ItemPrefabs;
    }
}