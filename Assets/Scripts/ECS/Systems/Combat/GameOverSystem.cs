using ECS.Components.Combat;
using ECS.Components.Mecanim;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems.Combat
{
    public class GameOverSystem : ComponentSystem
    {
        private EntityQuery _deathQuery;
        private EntityQuery _postDeathQuery;
        private EntityQuery _victoryQuery;
        private EntityQuery _postVictoryQuery;
        
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
                    ComponentType.ReadWrite<Dead>(),
                    ComponentType.ReadWrite<GameOver>(), 
                }
            });

            _postDeathQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Transform>(),
                    ComponentType.ReadWrite<Dead>(),
                    ComponentType.ReadWrite<GameOver>(), 
                }
            });
            
            _victoryQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Victory>(),
                    ComponentType.ReadWrite<MecanimTrigger>(),
                    ComponentType.ReadOnly<MecanimVictoryParameter>(),
                },
                None = new []
                {
                    ComponentType.ReadWrite<GameOver>(), 
                }
            });

            _postVictoryQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Victory>(),
                    ComponentType.ReadWrite<GameOver>(),
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
                    PostUpdateCommands.AddComponent(entity, new GameOver());
                }
            });

            Entities.With(_postDeathQuery).ForEach((Entity entity, Transform transform) =>
            {
                PostUpdateCommands.DestroyEntity(entity);
                PlayerManager.instance.PlayerKilled(transform);
            });
            
            Entities.With(_victoryQuery)
                .ForEach((DynamicBuffer<MecanimTrigger> mecanimTriggerBuffer, ref MecanimVictoryParameter mecanimVictoryParameter) =>
                {
                    mecanimTriggerBuffer.Add(new MecanimTrigger(mecanimVictoryParameter.value));
                });
            
            Entities.With(_postVictoryQuery).ForEach((Entity entity) =>
            {
                PostUpdateCommands.DestroyEntity(entity);
            });
        }
    }
}