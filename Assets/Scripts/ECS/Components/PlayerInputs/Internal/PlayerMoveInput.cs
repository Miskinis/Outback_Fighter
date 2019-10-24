using Unity.Entities;

namespace ECS.Components.PlayerInputs
{
    public struct PlayerMoveInput : IComponentData
    {
        public float value;
    }
}