using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Services.Factories;
using Game.Configs;
using Game.MonoBehaviourComponents.Objects;
using NUnit.Framework;

namespace Game.Services
{
    public class ColumnsFactory : AssetsFactoryBase
    {
        private const int COLUMNS_CEILING_COUNT = 25;
        // no pool needed, as only position changes
        private readonly List<ColumnComponent> _columns = new();
        
        public async Task Init(AssetsConfig assetsConfig, CancellationToken buildCancellationToken)
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
            Assert.IsNotNull(assetsConfig, "AssetsConfig is null");
            Assert.IsNotNull(assetsConfig.ColumnPrefab, "Column is null");
        }
    }
}