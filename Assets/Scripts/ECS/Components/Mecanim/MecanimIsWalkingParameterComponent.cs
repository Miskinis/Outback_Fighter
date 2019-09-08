using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimIsWalkingParameter : IComponentData
    {
        public readonly int value;

        public MecanimIsWalkingParameter(int value)
        {
            this.value = value;
        }
    }
    
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimIsWalkingParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "IsWalking";
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimIsWalkingParameter(Animator.StringToHash(parameter)));
        }
    }
}