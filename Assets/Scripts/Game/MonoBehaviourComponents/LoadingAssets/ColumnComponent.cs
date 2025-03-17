using UnityEngine;

namespace Game.MonoBehaviourComponents.LoadingAssets
{
   public class ColumnComponent : MonoBehaviour
   {
      [SerializeField] private Transform _transform;

      public void SetPosition(Vector3 position)
      {
         _transform.position = position;
      }

      public void SetRotation(Quaternion identity)
      {
         _transform.rotation = identity;
      }

      public void Activate()
      {
         _transform.gameObject.SetActive(true);
      }
   }
}
