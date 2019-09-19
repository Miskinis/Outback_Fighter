using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Combat
{
    public struct Health : IComponentData
    {
        public ushort value;

        public Health(ushort value)
        {
            this.value = value;
        }
    }

    public struct PreviousHealth : IComponentData
    {
        public ushort value;

        public PreviousHealth(ushort value)
        {
            this.value = value;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class HealthComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public ushort health = 100;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new Health(health));
            dstManager.AddComponentData(entity, new PreviousHealth(health));
        }
    }
}