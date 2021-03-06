using System.Collections;
using ECS;
using ECS.Components.Combat;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Combat
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class RangedDamageDealer : MonoBehaviour
    {
        public short damage = 10;
        public float impulseForce = 10f;
        public float selfDestructDelay = 5f;
        public GameObject spawnEffect;
        public GameObject hitEffect;

        private Entity _rootEntity;
        private EntityManager _entityManager;
        private Rigidbody _rigidbody;
        private ConvertHierarchyToEntities _characterRootGameObject;

        private void Awake()
        {
            _characterRootGameObject = GetComponentInParent<ConvertHierarchyToEntities>();
            _rootEntity              = _characterRootGameObject.HierarchyRootEntity;
            _entityManager           = World.Active.EntityManager;
            _rigidbody               = GetComponent<Rigidbody>();
            _rigidbody.isKinematic   = false;

            var thisTransform = transform;
            thisTransform.parent = null;
            _rigidbody.AddForce(thisTransform.forward * impulseForce, ForceMode.Impulse);
            
            if(spawnEffect)
            {
                Instantiate(spawnEffect, thisTransform.position, quaternion.identity);
            }

            foreach (var childColliders in _characterRootGameObject.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), childColliders);
            }
            
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
            if (_entityManager == null || _entityManager.Exists(_rootEntity) == false)
            {
                Destroy(this);
                return;
            }
            
            if(hitEffect)
            {
                Instantiate(hitEffect, other.contacts[0].point, Quaternion.identity);
            }

            var otherGameObject = other.gameObject;

            if (otherGameObject.GetComponent<RangedDamageDealer>())
            {
                Destroy(otherGameObject);
                Destroy(gameObject);
                return;
            }
            
            var otherEntityObject = otherGameObject.GetComponent<ConvertHierarchyToEntities>();
            if (otherEntityObject != null && otherGameObject != _characterRootGameObject.gameObject && otherGameObject.GetComponent<HealthComponent>() != null)
            {
                var otherEntity = otherEntityObject.HierarchyRootEntity;
                _entityManager.AddComponentData(otherEntity, new DealDamage(damage));
            }
            
            Destroy(gameObject);
        }
    }
}