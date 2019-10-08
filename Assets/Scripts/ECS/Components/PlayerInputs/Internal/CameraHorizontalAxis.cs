using Unity.Entities;

namespace ECS.Components.PlayerInputs
{
    public struct CameraHorizontalAxis : IComponentData
    {
        public float value;

        public CameraHorizontalAxis(float value)
        {
            this.value = value;
        }
    }
}