using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Mecanim
{
    public struct MecanimVictoryParameter : IComponentData
    {
        public readonly int value;

        public MecanimVictoryParameter(int value)
        {
            this.value = value;
        }
    }

    [RequiresEntityConversion]
    [DisallowMultipleComponent]
    public class MecanimVictoryParameterComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public string parameter = "Victory";

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new MecanimVictoryParameter(Animator.StringToHash(parameter)));
        }
    }
}