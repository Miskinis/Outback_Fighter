using System;
using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Combat
{
    public struct OnKillEffects : ISharedComponentData, IEquatable<OnKillEffects>
    {
        public readonly GameObject visualEffect;
        public readonly Transform effectSpawnPoint;

        public OnKillEffects(GameObject visualEffect, Transform effectSpawnPoint)
        {
            this.visualEffect = visualEffect;
            this.effectSpawnPoint = effectSpawnPoint;
        }

        public bool Equals(OnKillEffects other)
        {
            return Equals(visualEffect, other.visualEffect) && Equals(effectSpawnPoint, other.effectSpawnPoint);
        }

        public override bool Equals(object obj)
        {
            return obj is OnKillEffects other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((visualEffect != null ? visualEffect.GetHashCode() : 0) * 397) ^ (effectSpawnPoint != null ? effectSpawnPoint.GetHashCode() : 0);
            }
        }
    }

    public class OnKillEffectsComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject visualEffect;
        public Transform effectSpawnPoint;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddSharedComponentData(entity, new OnKillEffects(visualEffect, effectSpawnPoint));
        }
    }
}