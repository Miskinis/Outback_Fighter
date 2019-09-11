using Unity.Entities;

namespace ECS.Components.Mecanim
{
    [InternalBufferCapacity(32)]
    public struct MecanimSetFloat : IBufferElementData
    {
        public readonly int hashedParameter;
        public readonly float value;

        public MecanimSetFloat(int hashedParameter, float value)
        {
            this.hashedParameter = hashedParameter;
            this.value = value;
        }
    }
}