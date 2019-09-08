using Unity.Entities;

namespace ECS.Components.Mecanim
{
    [InternalBufferCapacity(16)]
    public struct MecanimSetBool : IBufferElementData
    {
        public readonly int hashedParameter;
        public readonly bool value;

        public MecanimSetBool(int hashedParameter, bool value)
        {
            this.hashedParameter = hashedParameter;
            this.value = value;
        }
    }
}