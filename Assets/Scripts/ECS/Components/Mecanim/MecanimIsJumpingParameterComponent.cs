using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimIsJumpingParameter : IComponentData
    {
        public readonly int value;

        public MecanimIsJumpingParameter(int value)
        {
            this.value = value;
        }
    }
    
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimIsJumpingParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "IsJumping";
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimIsJumpingParameter(Animator.StringToHash(parameter)));
        }
    }
}