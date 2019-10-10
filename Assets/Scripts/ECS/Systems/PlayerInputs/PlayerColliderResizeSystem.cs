using ECS.Components.PlayerInputs;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ECS.Systems.PlayerInputs
{
    [UpdateBefore(typeof(PlayerInputSystem))]
    public class PlayerColliderResizeSystem : ComponentSystem
    {
        private EntityQuery _resizeQuery;

        protected override void OnCreate()
        {
            _resizeQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlayerSizeComponent>(),
                    ComponentType.ReadWrite<PlayerSize>(),
                }
            });
        }

        protected override void OnUpdate()
        {
            Entities.With(_resizeQuery)
                .ForEach((PlayerSizeComponent playerSizeComponent, ref PlayerSize playerSize) =>
                {
                    float3 foreheadPosition = playerSizeComponent.forehead.position;
                    float3 feetPosition     = playerSizeComponent.feet.position;
                    float3 torsoPosition    = playerSizeComponent.torso.position;

                    float headToFeetDistance  = math.distance(new float3(0f, foreheadPosition.y, 0f), new float3(0f, feetPosition.y, 0f));
                    //float headToTorsoDistance = math.distance(foreheadPosition, torsoPosition);
                    //float feetToTorsoDistance = math.distance(feetPosition, torsoPosition);

                    playerSize.height = headToFeetDistance;
                    playerSize.center = (foreheadPosition + feetPosition + torsoPosition) / 3f;

                    Debug.DrawLine(feetPosition, foreheadPosition, Color.red);
                    Debug.DrawLine(feetPosition, torsoPosition, Color.red);
                    Debug.DrawLine(torsoPosition, foreheadPosition, Color.red);
                    /*playerSize.radius = headToFeetDistance * headToTorsoDistance * feetToTorsoDistance /
                                        2 * (headToTorsoDistance * feetToTorsoDistance * math.sin(Vector3.Angle(feetPosition - torsoPosition, foreheadPosition - torsoPosition)));*/
                });
        }
    }
}