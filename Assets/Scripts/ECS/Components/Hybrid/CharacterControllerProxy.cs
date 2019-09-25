using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Hybrid
{
    [RequireComponent(typeof(CharacterController))]
    [RequiresEntityConversion]
    public class CharacterControllerProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, GetComponent<CharacterController>());
        }
    }
}