using ECS.Components.Mecanim;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems.Mecanim
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class SetMecanimParametersSystem : ComponentSystem
    {
        private EntityQuery _triggerBootstrapQuery;
        private EntityQuery _floatBootstrapQuery;
        private EntityQuery _boolBootstrapQuery;
        private EntityQuery _intBootstrapQuery;

        protected override void OnCreate()
        {
            _triggerBootstrapQuery = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadWrite<Animator>()},
                None = new[] {ComponentType.ReadWrite<MecanimTrigger>()}
            });

            _floatBootstrapQuery = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadWrite<Animator>()},
                None = new[] {ComponentType.ReadWrite<MecanimSetFloat>()}
            });

            _boolBootstrapQuery = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadWrite<Animator>()},
                None = new[] {ComponentType.ReadWrite<MecanimSetBool>()}
            });

            _intBootstrapQuery = GetEntityQuery(new EntityQueryDesc
            {
                All  = new[] {ComponentType.ReadWrite<Animator>()},
                None = new[] {ComponentType.ReadWrite<MecanimSetInteger>()}
            });
        }

        protected override void OnUpdate()
        {
            //Bootstraps
            Entities.With(_triggerBootstrapQuery).ForEach(entity => { PostUpdateCommands.AddBuffer<MecanimTrigger>(entity); });
            Entities.With(_floatBootstrapQuery).ForEach(entity => { PostUpdateCommands.AddBuffer<MecanimSetFloat>(entity); });
            Entities.With(_boolBootstrapQuery).ForEach(entity => { PostUpdateCommands.AddBuffer<MecanimSetBool>(entity); });
            Entities.With(_intBootstrapQuery).ForEach(entity => { PostUpdateCommands.AddBuffer<MecanimSetInteger>(entity); });

            //Universal Trigger
            Entities.ForEach((Entity entity, DynamicBuffer<MecanimTrigger> dataBuffer, Animator animator) =>
            {
                for (int i = 0; i < dataBuffer.Length; i++)
                {
                    animator.SetTrigger(dataBuffer[i].hashedParameter);
                }

                dataBuffer.Clear();
            });

            //Universal float
            Entities.ForEach((Entity entity, DynamicBuffer<MecanimSetFloat> dataBuffer, Animator animator) =>
            {
                for (int i = 0; i < dataBuffer.Length; i++)
                {
                    var dataInstance = dataBuffer[i];
                    animator.SetFloat(dataInstance.hashedParameter, dataInstance.value);
                }

                dataBuffer.Clear();
            });

            //Universal bool
            Entities.ForEach((Entity entity, DynamicBuffer<MecanimSetBool> dataBuffer, Animator animator) =>
            {
                for (int i = 0; i < dataBuffer.Length; i++)
                {
                    var dataInstance = dataBuffer[i];
                    animator.SetBool(dataInstance.hashedParameter, dataInstance.value);
                }

                dataBuffer.Clear();
            });

            //Universal int
            Entities.ForEach((Entity entity, DynamicBuffer<MecanimSetInteger> dataBuffer, Animator animator) =>
            {
                for (int i = 0; i < dataBuffer.Length; i++)
                {
                    var dataInstance = dataBuffer[i];
                    animator.SetInteger(dataInstance.hashedParameter, dataInstance.value);
                }

                dataBuffer.Clear();
            });
        }
    }
}