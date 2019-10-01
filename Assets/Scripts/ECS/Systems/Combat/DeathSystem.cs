using ECS.Components.Combat;
using ECS.Components.Mecanim;
using Unity.Entities;

namespace ECS.Systems.Combat
{
    public class DeathSystem : ComponentSystem
    {
        private EntityQuery _deathQuery;
        private EntityQuery _gameOverQuery;
        
        protected override void OnCreate()
        {
            _deathQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<MecanimDieParameter>(),
                    ComponentType.ReadWrite<MecanimTrigger>(),
                    ComponentType.ReadOnly<Health>(), 
                },
                None = new[]
                {
                    ComponentType.ReadWrite<Dead>()
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_deathQuery).ForEach((Entity entity,
                DynamicBuffer<MecanimTrigger> mecanimTriggerBuffer,
                ref MecanimDieParameter mecanimDieParameter,
                ref Health currentHealth) =>
            {
                if (currentHealth.value < 1)
                {
                    mecanimTriggerBuffer.Add(new MecanimTrigger(mecanimDieParameter.hashedParameter));
                    PostUpdateCommands.AddComponent(entity, new Dead());
                }
            });
        }
    }
}