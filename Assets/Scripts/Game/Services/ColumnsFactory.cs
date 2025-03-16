using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Services.Factories;
using Game.Configs;
using Game.MonoBehaviourComponents;
using NUnit.Framework;
using UnityEngine;

namespace Game.Services
{
    public class ColumnsFactory : AssetsFactoryBase
    {
        private const int COLUMNS_CEILING_COUNT = 20;
        // no pool needed
        private readonly List<ColumnComponent> _columns = new();
        
        public async Task Init(MatchConfig matchConfig, CancellationToken buildCancellationToken)
        {
            ValidateColumnsConfigAsserts(matchConfig);
            try
            {
                for (var i = 0; i < COLUMNS_CEILING_COUNT; i++)
                {
                    var column = await LoadPrefab<ColumnComponent>(matchConfig.ColumnPrefab, buildCancellationToken);
                    _columns.Add(column);
                }
                Debug.Log($"Loaded {_columns.Count} columns");
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
        
        public ColumnComponent GetColumnByIndex(int index)
        {
            ColumnComponent column = _columns[index]; 
            return column;
        }

        private void ValidateColumnsConfigAsserts(MatchConfig matchConfig)
        {
            Assert.IsNotNull(matchConfig, "MatchConfig is null");
            Assert.IsNotNull(matchConfig.ColumnPrefab, "Column is null");
        }
    }
}