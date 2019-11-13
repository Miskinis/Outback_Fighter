using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Combat
{
    public struct Dash : IComponentData
    {
        public readonly float speed;

        public Dash(float speed)
        {
            this.speed = speed;
        }
    }

    public class DashComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Tooltip("How far will the character dash over animation time")]
        public float speed = 1.5f;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new Dash(speed));
        }
    }
}