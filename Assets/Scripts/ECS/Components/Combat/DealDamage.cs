using Unity.Entities;

namespace ECS.Components
{
    public struct DealDamage : IComponentData
    {
        public ushort damage;

        public DealDamage(ushort damage)
        {
            this.damage = damage;
        }
    }
}