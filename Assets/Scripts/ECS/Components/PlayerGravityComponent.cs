using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerGravity : IComponentData
    {
        public float value;

        public PlayerGravity(float value)
        {
            this.value = value;
        }
    }
    
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class PlayerGravityComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float gravity = 9.81f;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerGravity(gravity));
        }
    }
}