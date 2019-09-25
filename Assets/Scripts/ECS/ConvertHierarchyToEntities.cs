using Unity.Entities;
using UnityEngine;

namespace ECS
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-1000)]
    public class ConvertHierarchyToEntities : MonoBehaviour, IConvertGameObjectToEntity
    {
        private Entity _entity;

        public Entity HierarchyRootEntity => _entity;

        private void OnDisable()
        {
            var entityManager = World.Active?.EntityManager;
            if (entityManager != null && entityManager.Exists(_entity)) entityManager.DestroyEntity(_entity);
        }

        private void OnEnable()
        {
            if (World.Active.EntityManager.Exists(_entity) == false) _entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObject, World.Active);

#if UNITY_EDITOR
            World.Active.EntityManager.SetName(HierarchyRootEntity, $"{name} {HierarchyRootEntity.ToString()}");
#endif
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            if (dstManager.Exists(_entity) == false) _entity = entity;
        }
    }
}