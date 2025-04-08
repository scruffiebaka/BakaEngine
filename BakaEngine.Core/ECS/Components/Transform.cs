using OpenTK.Mathematics;

namespace BakaEngine.Core.ECS.Components
{
    public class Transform : Component
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        public Quaternion Rotation = Quaternion.Identity;
        public Vector3 eulerAngles
        {
            get
            {
                Vector3 eulerAngles_Radians;
                Quaternion.ToEulerAngles(Rotation, out eulerAngles_Radians);
                return new Vector3(MathHelper.RadiansToDegrees(eulerAngles_Radians.X),
                MathHelper.RadiansToDegrees(eulerAngles_Radians.Y),
                MathHelper.RadiansToDegrees(eulerAngles_Radians.Z));
            }
            set
            {
                Rotation = Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(value.X),
                MathHelper.DegreesToRadians(value.Y),
                MathHelper.DegreesToRadians(value.Z));
            }
        }
    }
}
