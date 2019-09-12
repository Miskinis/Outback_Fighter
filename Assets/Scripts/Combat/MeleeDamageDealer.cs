using ECS;
using ECS.Components;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class MeleeDamageDealer : MonoBehaviour
{
    public ConvertHierarchyToEntities rootEntityObject;
    public ushort damage = 1;

    private Entity _entity;
    private EntityManager _entityManager;

    private void Awake()
    {
        _entity        = rootEntityObject.HierarchyRootEntity;
        _entityManager = World.Active.EntityManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_entityManager == null || _entityManager.Exists(_entity) == false)
        {
            Destroy(this);
            return;
        }

        var otherEntityObject = other.GetComponent<ConvertHierarchyToEntities>();
        if (otherEntityObject == null || other.GetComponent<HealthComponent>() == null) return;

        var otherEntity = otherEntityObject.HierarchyRootEntity;

        _entityManager.AddComponentData(otherEntity, new DealDamage(damage));
    }
}