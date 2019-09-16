using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ECS.Components.Hybrid
{
    [RequireComponent(typeof(PlayerInput))]
    [RequiresEntityConversion]
    public class PlayerInputProxy : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, GetComponent<PlayerInput>());
        }
    }
}
