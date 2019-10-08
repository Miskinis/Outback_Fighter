using Unity.Entities;

namespace ECS.Components.PlayerInputs
{
    public struct InitialPlayerDepth : IComponentData
    {
        public readonly float value;

        public InitialPlayerDepth(float value)
        {
            this.value = value;
        }
    }
}