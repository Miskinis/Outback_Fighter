using Unity.Entities;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerIsGrounded : IComponentData
    {
        public bool value;
    }
}