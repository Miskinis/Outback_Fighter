using Unity.Entities;

namespace ECS.Components.Mecanim
{
    [InternalBufferCapacity(16)]
    public struct MecanimSetInteger : IBufferElementData
    {
        public readonly int hashedParameter;
        public readonly int value;

        public MecanimSetInteger(int hashedParameter, int value)
        {
            this.hashedParameter = hashedParameter;
            this.value           = value;
        }
    }
}