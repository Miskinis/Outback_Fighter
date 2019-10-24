using Unity.Entities;

namespace ECS.Components.Combat
{
    public struct PreviousHealth : IComponentData
    {
        public short value;

        public PreviousHealth(short value)
        {
            this.value = value;
        }
    }
}