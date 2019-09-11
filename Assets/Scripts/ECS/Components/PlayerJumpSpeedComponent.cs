using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerJumpSpeed : IComponentData
    {
        public float value;

        public PlayerJumpSpeed(float value)
        {
            this.value = value;
        }
    }
    
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class PlayerJumpSpeedComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float jumpSpeed = 3f;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerJumpSpeed(jumpSpeed));
        }
    }
}