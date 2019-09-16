using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Components
{
    [DisallowMultipleComponent]
    public class CustomCopyTransformFromGameObject : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentObject(entity, transform);
            dstManager.AddComponentData(entity, new CopyTransformFromGameObject());

            /*this.OnDestroyAsObservable().Subscribe(_ =>
            {
                var entityManager = World.Active?.EntityManager;
                if (entityManager != null && entityManager.Exists(entity))
                    entityManager.DestroyEntity(entity);
            });*/
        }
    }
}