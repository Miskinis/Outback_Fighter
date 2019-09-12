using Unity.Entities;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerJumpInput : IComponentData
    {
        public bool value;
    }
}