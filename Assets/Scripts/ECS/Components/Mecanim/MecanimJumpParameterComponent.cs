using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimJumpParameter : IComponentData
    {
        public readonly int value;

        public MecanimJumpParameter(int value)
        {
            this.value = value;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimJumpParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "Jump";

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimJumpParameter(Animator.StringToHash(parameter)));
        }
    }
}