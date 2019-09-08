using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Systems
{
    [UpdateInGroup(typeof(TransformSystemGroup))]
    [UpdateAfter(typeof(CopyTransformFromGameObjectSystem))]
    [UpdateBefore(typeof(EndFrameTRSToLocalToWorldSystem))]
    public class CustomCopyTransformFromGameObjectSystem : JobComponentSystem
    {
        private EndSimulationEntityCommandBufferSystem _entityCommandBufferSystem;
        private EntityQuery _localToWorldGroup;
        private EntityQuery _translationQuery;
        private EntityQuery _rotationQuery;

        protected override void OnCreate()
        {
            _entityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            
            _localToWorldGroup = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadOnly<CopyTransformFromGameObject>()},
                Any  = new[] {ComponentType.ReadOnly<Translation>(), ComponentType.ReadOnly<Rotation>()},
                None = new[] {ComponentType.ReadOnly<LocalToWorld>()}
            });

            _translationQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<CopyTransformFromGameObject>(),
                    ComponentType.ReadOnly<LocalToWorld>(),
                    ComponentType.ReadWrite<Translation>()
                }
            });
            
            _rotationQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadOnly<CopyTransformFromGameObject>(),
                    ComponentType.ReadOnly<LocalToWorld>(),
                    ComponentType.ReadWrite<Rotation>()
                }
            });
        }

        private struct LocalToWorldJob : IJobChunk
        {
            [WriteOnly] public EntityCommandBuffer.Concurrent commandBuffer;
            [ReadOnly] public ArchetypeChunkEntityType entityType;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                var instanceCount = chunk.Count;
                var entities      = chunk.GetNativeArray(entityType);
                for (var i = 0; i < instanceCount; i++)
                {
                    commandBuffer.AddComponent(chunkIndex, entities[i], new LocalToWorld {Value = float4x4.identity});
                }
            }
        }
        
        [BurstCompile]
        private struct TranslationJob : IJobForEach<LocalToWorld, Translation>
        {
            public void Execute([ReadOnly] ref LocalToWorld transformMatrix, [WriteOnly] ref Translation translation)
            {
                translation.Value = transformMatrix.Position;
            }
        }
        
        [BurstCompile]
        private struct RotationJob : IJobForEach<LocalToWorld, Rotation>
        {
            public void Execute([ReadOnly] ref LocalToWorld transformMatrix, [WriteOnly] ref Rotation rotation)
            {
                rotation.Value = quaternion.LookRotation(transformMatrix.Forward, transformMatrix.Up);
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var localToWorldJob = new LocalToWorldJob
            {
                commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent(),
                entityType = GetArchetypeChunkEntityType()
            }.Schedule(_localToWorldGroup);
            var translationJob = new TranslationJob().Schedule(_translationQuery, inputDeps);
            var rotationJob = new RotationJob().Schedule(_rotationQuery, inputDeps);
            
            _entityCommandBufferSystem.AddJobHandleForProducer(localToWorldJob);
            
            return JobHandle.CombineDependencies(localToWorldJob, translationJob, rotationJob);
        }
    }
}