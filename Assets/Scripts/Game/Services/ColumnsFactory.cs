using System.Collections.Generic;
using System.Threading;
using Core.Services.Factories;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.MonoBehaviourComponents.LoadingAssets;
using UnityEngine;

namespace Game.Services
{
    public class ColumnsFactory : AssetsFactoryBase
    {
        private const int COLUMNS_CEILING_COUNT = 25;
        // no pool needed, as only position changes
        private readonly List<ColumnComponent> _columns = new();
        
        public async UniTask Init(AssetsConfig assetsConfig, CancellationToken buildCancellationToken)
        {
            ValidateColumnsConfigAsserts(assetsConfig);
            try
            {
                for (var i = 0; i < COLUMNS_CEILING_COUNT; i++)
                {
                    var column = await LoadPrefab<ColumnComponent>(assetsConfig.ColumnPrefab, buildCancellationToken);
                    _columns.Add(column);
                }
            }
            catch
            {
                Clear();
                throw;
            }
        }

        public List<ColumnComponent> GetAllColumns()
        {
            return _columns;
        }

        private void ValidateColumnsConfigAsserts(AssetsConfig assetsConfig)
        {
            Debug.Assert(assetsConfig != null, "AssetsConfig is null");
            Debug.Assert(assetsConfig.ColumnPrefab != null, "Column is null");
        }
    }
}