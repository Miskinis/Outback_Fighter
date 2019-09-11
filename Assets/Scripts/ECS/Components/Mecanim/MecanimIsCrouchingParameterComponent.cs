using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimIsCrouchingParameter : IComponentData
    {
        public readonly int value;

        public MecanimIsCrouchingParameter(int value)
        {
            this.value = value;
        }
    }
    
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimIsCrouchingParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "IsCrouching";
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimIsCrouchingParameter(Animator.StringToHash(parameter)));
        }
    }
}