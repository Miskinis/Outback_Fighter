using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerMoveDirection : IComponentData
    {
        public float3 value;
    }
}