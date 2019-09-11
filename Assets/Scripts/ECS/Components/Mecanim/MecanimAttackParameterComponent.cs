using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimAttackParameter : IComponentData
    {
        public readonly int value;

        public MecanimAttackParameter(int value)
        {
            this.value = value;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimAttackParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "Attack";

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimAttackParameter(Animator.StringToHash(parameter)));
        }
    }
}