using System;
using System.Collections;
using ECS;
using ECS.Components;
using ECS.Components.Combat;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class RangedDamageDealer : MonoBehaviour
{
    public ushort damage = 10;
    public float impulseForce = 10f;
    public float selfDestructDelay = 5f;
    public GameObject spawnEffect;
    public GameObject hitEffect;

    private Entity _entity;
    private EntityManager _entityManager;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _entity                = GetComponentInParent<ConvertHierarchyToEntities>().HierarchyRootEntity;
        _entityManager         = World.Active.EntityManager;
        _rigidbody             = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = false;
        
        var thisTransform = transform;
        thisTransform.parent = null;
        _rigidbody.AddForce(thisTransform.forward * impulseForce, ForceMode.Impulse);
        Instantiate(spawnEffect, thisTransform.position, quaternion.identity);
        
        StartCoroutine(SelfDestruct(selfDestructDelay));
    }

    private IEnumerator SelfDestruct(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (enabled == false) return;
        if (_entityManager == null || _entityManager.Exists(_entity) == false)
        {
            Destroy(this);
            return;
        }
        var otherGameObject = other.gameObject;
        var otherEntityObject = otherGameObject.GetComponent<ConvertHierarchyToEntities>();
        if (otherEntityObject == null || otherGameObject.GetComponent<HealthComponent>() == null) return;

        var otherEntity = otherEntityObject.HierarchyRootEntity;
        _entityManager.AddComponentData(otherEntity, new DealDamage(damage));
        Instantiate(hitEffect, other.contacts[0].point, Quaternion.identity);
        StopCoroutine(nameof(SelfDestruct));
        enabled = false;
        Destroy(gameObject);
    }
}