using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Combat
{
    public struct Health : IComponentData
    {
        public short value;

        public Health(short value)
        {
            this.value = value;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class HealthComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public short health = 100;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new Health(health));
            dstManager.AddComponentData(entity, new PreviousHealth(health));
        }
    }
}