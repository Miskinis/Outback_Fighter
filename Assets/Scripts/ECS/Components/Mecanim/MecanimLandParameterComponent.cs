using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimLandParameter : IComponentData
    {
        public readonly int value;

        public MecanimLandParameter(int value)
        {
            this.value = value;
        }
    }
    
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimLandParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "Land";
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimLandParameter(Animator.StringToHash(parameter)));
        }
    }
}