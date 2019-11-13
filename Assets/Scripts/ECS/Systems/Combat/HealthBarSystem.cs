using ECS.Components.Combat;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace ECS.Systems.Combat
{
    public class HealthBarSystem : ComponentSystem
    {
        private struct HealthBarAssigned : IComponentData
        {
        }

        private EntityQuery _bootstrapOuterQuery;
        private EntityQuery _bootstrapInnerQuery;
        private EntityQuery _healthBarQuery;

        protected override void OnCreate()
        {
            _bootstrapOuterQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Health>(),
                    ComponentType.ReadWrite<MaxHealth>(), 
                },
                None = new[]
                {
                    ComponentType.ReadWrite<HealthBarAssigned>(),
                }
            });
            _bootstrapInnerQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<HealthBar>(),
                    ComponentType.ReadWrite<Image>(), 
                },
                None = new[]
                {
                    ComponentType.ReadWrite<HealthBarAssigned>(),
                }
            });
            _healthBarQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<Image>(), 
                    ComponentType.ReadOnly<HealthBar>(),
                    ComponentType.ReadWrite<HealthBarAssigned>(), 
                }
            });
        }

        protected override void OnUpdate()
        {
            var entityType = GetArchetypeChunkEntityType();

            var outerChunkArray = _bootstrapOuterQuery.CreateArchetypeChunkArray(Allocator.TempJob);
            foreach (var outerChunk in outerChunkArray)
            {
                int outerInstanceCount = outerChunk.Count;
                var outerEntityArray = outerChunk.GetNativeArray(entityType);
                for (int i = 0; i < outerInstanceCount; i++)
                {
                    var outerEntity = outerEntityArray[i];
                    
                    var innerChunkArray = _bootstrapInnerQuery.CreateArchetypeChunkArray(Allocator.TempJob);
                    foreach (var innerChunk in innerChunkArray)
                    {
                        var innerEntityArray = innerChunk.GetNativeArray(entityType);

#if UNITY_EDITOR
                        if (innerChunk.Count < outerInstanceCount)
                        {
                            Debug.LogError($"There are {outerInstanceCount} players, but only {innerChunk.Count} health bars");
                        }
#endif                        
                        var innerEntity = innerEntityArray[i];
                        PostUpdateCommands.SetComponent(innerEntity, new HealthBar {playerEntity = outerEntity});
                        PostUpdateCommands.AddComponent<HealthBarAssigned>(innerEntity);
                        
                        PostUpdateCommands.AddComponent<HealthBarAssigned>(outerEntity);
                    }
                    innerChunkArray.Dispose();
                }
            }
            outerChunkArray.Dispose();
            
            var healthEntityArray    = GetComponentDataFromEntity<Health>(true);
            var maxHealthEntityArray = GetComponentDataFromEntity<MaxHealth>(true);
            
            Entities.With(_healthBarQuery).ForEach((Entity entity, Image image, ref HealthBar healthBar) =>
            {
                if (healthEntityArray.Exists(healthBar.playerEntity) == false)
                {
                    PostUpdateCommands.RemoveComponent<HealthBarAssigned>(healthBar.playerEntity);
                    PostUpdateCommands.RemoveComponent<HealthBarAssigned>(entity);
                    healthBar.playerEntity = Entity.Null;
                    return;
                }
                image.fillAmount = healthEntityArray[healthBar.playerEntity].value / maxHealthEntityArray[healthBar.playerEntity].value;
            });
        }
    }
}