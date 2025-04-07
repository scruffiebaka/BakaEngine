using OpenTK.Mathematics;

namespace BakaEngine.Core.ECS.Components
{
    public class Transform : Component
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        public Vector3 Rotation = Vector3.Zero;
        public Quaternion QuaternionRotation { get; private set; } = Quaternion.Identity;

        public void Update() {
                QuaternionRotation = Quaternion.FromEulerAngles(
                    new Vector3(MathHelper.DegreesToRadians(Rotation.X),
                    MathHelper.DegreesToRadians(Rotation.Y),
                    MathHelper.DegreesToRadians(Rotation.Z)));
        }
    }
}
