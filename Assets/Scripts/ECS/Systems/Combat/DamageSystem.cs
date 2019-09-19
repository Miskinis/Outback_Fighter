using ECS.Components.Combat;
using Unity.Entities;

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
            });
        }
    }
}