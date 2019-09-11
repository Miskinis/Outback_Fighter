using Unity.Entities;

namespace ECS.Components.Mecanim
{
    [InternalBufferCapacity(16)]
    public struct MecanimTrigger : IBufferElementData
    {
        public int hashedParameter;

        public MecanimTrigger(int hashedParameter)
        {
            this.hashedParameter = hashedParameter;
        }
    }
}