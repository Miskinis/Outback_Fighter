using System;
using Unity.Entities;
using UnityEngine;

namespace ECS.Components.Combat
{
    public struct TimeFadeValue : ISharedComponentData, IEquatable<TimeFadeValue>
    {
        public readonly AnimationCurve value;

        public TimeFadeValue(AnimationCurve value)
        {
            this.value = value;
        }

        public bool Equals(TimeFadeValue other)
        {
            return Equals(value, other.value);
        }

        public override bool Equals(object obj)
        {
            return obj is TimeFadeValue other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (value != null ? value.GetHashCode() : 0);
        }
    }

    public struct TimeFadeClock : IComponentData
    {
        public float time;

        public TimeFadeClock(float time)
        {
            this.time = time;
        }
    }
    
    public class TimeFadeValueComponent : MonoBehaviour, IConvertGameObjectToEntity
    {
        public AnimationCurve valueCurve;
        
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddSharedComponentData(entity, new TimeFadeValue(valueCurve));
            dstManager.AddComponentData(entity, new TimeFadeClock(valueCurve[valueCurve.length - 1].time));
        }
    }
}