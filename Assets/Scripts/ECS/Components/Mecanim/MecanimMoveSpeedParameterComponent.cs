using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimMoveSpeedParameter : IComponentData
    {
        public readonly int value;

        public MecanimMoveSpeedParameter(int value)
        {
            this.value = value;
        }
    }
    
    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimMoveSpeedParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "MoveSpeed";
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimMoveSpeedParameter(Animator.StringToHash(parameter)));
        }
    }
}