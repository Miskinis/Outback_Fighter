using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimMoveDirectionParameter : IComponentData
    {
        public readonly int value;

        public MecanimMoveDirectionParameter(int value)
        {
            this.value = value;
        }
    }
    
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimMoveDirectionParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "MoveDirection";
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimMoveDirectionParameter(Animator.StringToHash(parameter)));
        }
    }
}