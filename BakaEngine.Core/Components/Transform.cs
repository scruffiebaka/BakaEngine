using OpenTK.Mathematics;

namespace BakaEngine.Core.Components
{
    public class Transform : Component
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;
        public Quaternion Rotation = Quaternion.Identity;
    }
}
