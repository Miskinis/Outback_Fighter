using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Hybrid
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class TransformProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, transform);
        }
    }
}