using System.Collections.Generic;
using Game.MonoBehaviourComponents.LoadingAssets;
using UnityEngine;

namespace Game.MonoBehaviourComponents
{
    public interface IGroundHitHandler
    {
        void OnGroundHit(ColumnComponent finalizedColumn, ColumnComponent rightColumn);
        void Dispose();
    }
    public class ColumnPositioner : IGroundHitHandler
    {
        private const float INIT_POSITION_X = -12;
        private const float INIT_POSITION_Y = -3;
        private const float X_DRAG_FACTOR = 1.1f;
        private List<ColumnComponent> _columns;
        
        public void Init(List<ColumnComponent> columns)
        {
            _columns = columns;
            PlaceColumnsStartPositions();
        }
        
        public void OnGroundHit(ColumnComponent finalizedColumn, ColumnComponent rightColumn)
        {
            finalizedColumn.SetPosition(rightColumn.transform.position + Vector3.right); 
        }

        public void Dispose()
        {
            _columns.Clear();
        }

        public Vector3 GetFirstColumnPosition()
        {
           return _columns[0].transform.position;
        }

        public Vector3 GetLastColumnPosition()
        {
            return _columns[^1].transform.position;
        }

        private void PlaceColumnsStartPositions()
        {
            for (var index = 0; index < _columns.Count; index++)
            {
                ColumnComponent column = _columns[index];
                column.SetPosition(new Vector2(INIT_POSITION_X + (index * X_DRAG_FACTOR), INIT_POSITION_Y));
                column.SetRotation(Quaternion.identity);
                column.Activate();
            }
        }
    }
}