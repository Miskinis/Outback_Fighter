using Unity.Entities;
using UnityEngine;

namespace ECS.Components
{
    public struct PlayerMoveSpeed : IComponentData
    {
        public float value;

        public PlayerMoveSpeed(float value)
        {
            this.value = value;
        }
    }
    
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class PlayerMoveSpeedComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float moveSpeed = 3f;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerMoveSpeed(moveSpeed));
        }
    }
}