using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Hybrid
{
    [RequireComponent(typeof(Animator))]
    [RequiresEntityConversion]
    public class AnimatorProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, GetComponent<Animator>());
        }
    }
}