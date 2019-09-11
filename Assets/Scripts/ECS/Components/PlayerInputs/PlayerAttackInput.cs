using Unity.Entities;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerAttackInput : IComponentData
    {
        public bool value;
    }
}