using Unity.Entities;
using Unity.Mathematics;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerFeetPoint : IComponentData
    {
        public float3 value;

        public PlayerFeetPoint(float3 value)
        {
            this.value = value;
        }
    }
}