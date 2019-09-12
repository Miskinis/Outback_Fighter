using Unity.Entities;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerSpecialAttackInput : IComponentData
    {
        public bool value;
    }
}