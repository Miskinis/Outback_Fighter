using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerSize : IComponentData
    {
        public float3 center;
        public float radius;
        public float height;
    }
    
    public class PlayerSizeComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public Transform forehead;
        public Transform feet;
        public Transform torso;

        public float3 centerOffset = new float3(0f, 0.1f, 0f);
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, this);
            dstManager.AddComponentData(entity, new PlayerSize());
        }
    }
}