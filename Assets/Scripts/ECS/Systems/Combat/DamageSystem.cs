using ECS.Components.Combat;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems.Combat
{
    public class DamageSystem : ComponentSystem
    {
        private EntityQuery _dealDamageQuery;
        
        protected override void OnCreate()
        {
            _dealDamageQuery = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadOnly<DealDamage>(), ComponentType.ReadWrite<Health>()},
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_dealDamageQuery).ForEach((Entity entity, ref DealDamage dealDamage, ref Health health) =>
            {
                health.value -= dealDamage.damage;
                PostUpdateCommands.RemoveComponent<DealDamage>(entity);
                
                if (HasSingleton<AllowTimeFade>() == false)
                {
                    PostUpdateCommands.CreateEntity(EntityManager.CreateArchetype(ComponentType.ReadWrite<AllowTimeFade>()));
                }
            });

            if (HasSingleton<AllowTimeFade>())
            {
                Entities.WithAll<TimeFadeValue, TimeFadeClock>().ForEach((TimeFadeValue timeFadeValue, ref TimeFadeClock timeFadeClock) =>
                {
                    var curve = timeFadeValue.value;
                    UnityEngine.Time.timeScale = curve.Evaluate(timeFadeClock.time);
                    if (timeFadeClock.time > curve[curve.length - 1].time)
                    {
                        timeFadeClock.time = curve[0].time;
                        PostUpdateCommands.DestroyEntity(GetSingletonEntity<TimeFadeValue>());
                    }

                    timeFadeClock.time += UnityEngine.Time.unscaledDeltaTime;
                });
            }
        }
    }
}