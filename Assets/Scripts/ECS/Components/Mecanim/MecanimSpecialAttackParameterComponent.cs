using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimSpecialAttackParameter : IComponentData
    {
        public readonly int value;

        public MecanimSpecialAttackParameter(int value)
        {
            this.value = value;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimSpecialAttackParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "SpecialAttack";

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimSpecialAttackParameter(Animator.StringToHash(parameter)));
        }
    }
}