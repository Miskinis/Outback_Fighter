using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Combat
{
    public struct MaxHealth : IComponentData
    {
        public float value;

        public MaxHealth(float value)
        {
            this.value = value;
        }
    }
    
    public class MaxHealthComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float maxHealth = 100f;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MaxHealth(maxHealth));
        }
    }
}