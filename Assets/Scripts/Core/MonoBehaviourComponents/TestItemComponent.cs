using Core.Services.Factories;
using UnityEngine;

namespace Core.MonoBehaviourComponents
{
    public class TestItemComponent : MonoBehaviour, IItem
    {
        public void Deactivate()
        {
            throw new System.NotImplementedException();
        }
    }
}
