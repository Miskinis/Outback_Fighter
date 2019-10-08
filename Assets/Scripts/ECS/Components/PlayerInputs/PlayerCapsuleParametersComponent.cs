using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerCapsuleParameters : IComponentData
    {
        public float3 standCenter;
        public float3 crouchCenter;
        public float standRadius;
        public float crouchRadius;
        public float standHeight;
        public float crouchHeight;

        public PlayerCapsuleParameters(float3 standCenter, float3 crouchCenter, float standRadius, float crouchRadius, float standHeight, float crouchHeight)
        {
            this.standCenter = standCenter;
            this.crouchCenter = crouchCenter;
            this.standRadius = standRadius;
            this.crouchRadius = crouchRadius;
            this.standHeight = standHeight;
            this.crouchHeight = crouchHeight;
        }
    }
    
    public class PlayerCapsuleParametersComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float3 standCenter = new float3(0f, 0.45f, 0f);
        public float3 crouchCenter = new float3(0f, 0.2f, 0.1f);
        public float standRadius = 0.2f;
        public float crouchRadius = 0.2f;
        public float standHeight = 0.93f;
        public float crouchHeight = 0.2f;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new PlayerCapsuleParameters(standCenter, crouchCenter, standRadius, crouchRadius, standHeight, crouchHeight));
        }
    }
}