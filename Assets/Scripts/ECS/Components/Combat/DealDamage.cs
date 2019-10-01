using Unity.Entities;

namespace ECS.Components.Combat
{
    public struct DealDamage : IComponentData
    {
        public short damage;

        public DealDamage(short damage)
        {
            this.damage = damage;
        }
    }
}