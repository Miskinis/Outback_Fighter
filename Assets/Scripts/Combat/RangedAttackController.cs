using ECS;
using MecanimBehaviors;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class RangedAttackController : MonoBehaviour
{
    public Transform spawnPoint;
    public ConvertHierarchyToEntities rootEntityObject;
    public Animator animator;
    public float maxHitDistance = 100f;
    public ushort damage;
    public GameObject projectilePrefab;

    private RangedAttackBehavior[] _rangedAttackBehaviors;

    private Entity _entity;
    private EntityManager _entityManager;

    private void Awake()
    {
        _entity        = rootEntityObject.HierarchyRootEntity;
        _entityManager = World.Active.EntityManager;

        _rangedAttackBehaviors = animator.GetBehaviours<RangedAttackBehavior>();
        foreach (var attackBehavior in _rangedAttackBehaviors)
            attackBehavior.onFrameAction = () =>
            {
                if (_entityManager == null || _entityManager.Exists(_entity) == false)
                {
                    Destroy(this);
                    return;
                }

                Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation, rootEntityObject.transform);
            };
    }

    private void OnDestroy()
    {
        foreach (var attackBehavior in _rangedAttackBehaviors) attackBehavior.onFrameAction = null;
    }
}