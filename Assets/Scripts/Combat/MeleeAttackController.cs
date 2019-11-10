using MecanimBehaviors;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class MeleeAttackController : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    public MeleeDamageDealer meleeDamageDealer;
    public Animator animator;

    private MeleeAttackBehavior[] _meleeAttackBehaviors;
    private Collider _weaponCollider;

    private void Awake()
    {
        _weaponCollider       = meleeDamageDealer.GetComponent<Collider>();
        _meleeAttackBehaviors = animator.GetBehaviours<MeleeAttackBehavior>();
        foreach (var attackBehavior in _meleeAttackBehaviors)
        {
            attackBehavior.onStartAction = () =>
            {
                if(trailRenderer)
                    trailRenderer.enabled   = true;
                _weaponCollider.enabled = true;
            };
            attackBehavior.onEndAction = () =>
            {
                if(trailRenderer)
                    trailRenderer.enabled   = false;
                _weaponCollider.enabled = false;
            };
        }

        _weaponCollider.enabled = false;
        
        if(trailRenderer)
            trailRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        foreach (var attackBehavior in _meleeAttackBehaviors)
        {
            attackBehavior.onStartAction = null;
            attackBehavior.onEndAction   = null;
        }
    }
}