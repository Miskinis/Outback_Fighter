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
                }
            });
        }

        protected override void OnUpdate()
        {
            var entityType = GetArchetypeChunkEntityType();
            var healthBarType = GetArchetypeChunkComponentType<HealthBar>();
            
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
                        var innerEntityArray    = innerChunk.GetNativeArray(entityType);
                        var innerHealthBarArray = innerChunk.GetNativeArray(healthBarType);

                        var innerEntity = innerEntityArray[i];

                        innerHealthBarArray[i] = new HealthBar {playerEntity = outerEntity};
                        PostUpdateCommands.AddComponent<HealthBarAssigned>(outerEntity);
                        PostUpdateCommands.AddComponent<HealthBarAssigned>(innerEntity);
                    }

                    innerChunkArray.Dispose();
                }
            }
            outerChunkArray.Dispose();
            
            var healthEntityArray    = GetComponentDataFromEntity<Health>(true);
            var maxHealthEntityArray = GetComponentDataFromEntity<MaxHealth>(true);
            
            Entities.With(_healthBarQuery).ForEach((Image image, ref HealthBar healthBar) =>
            {
#if UNITY_EDITOR
                if (healthBar.playerEntity == Entity.Null)
                {
                    Debug.Log("HealthBar not assigned to player");
                }
#endif
                image.fillAmount = healthEntityArray[healthBar.playerEntity].value / maxHealthEntityArray[healthBar.playerEntity].value;
            });
        }
    }
}