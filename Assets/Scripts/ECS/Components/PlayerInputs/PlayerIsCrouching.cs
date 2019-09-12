using Unity.Entities;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerCrouchInput : IComponentData
    {
        public bool value;
    }
}