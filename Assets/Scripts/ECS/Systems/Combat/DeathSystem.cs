using ECS.Components.Combat;
using ECS.Components.Mecanim;
using Unity.Entities;
using UnityEngine;

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

            _gameOverQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Transform>(),
                    ComponentType.ReadOnly<Dead>(),
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

            Entities.With(_gameOverQuery).ForEach((Entity entity, Transform transform) =>
            {
                PostUpdateCommands.DestroyEntity(entity);
                PlayerManager.instance.PlayerKilled(transform);
            });
        }
    }
}