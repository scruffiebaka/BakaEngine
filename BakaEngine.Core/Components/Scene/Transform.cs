using OpenTK.Mathematics;

namespace BakaEngine.Core.Components
{
    public class Transform : Component
    {
        public Vector3 LocalPosition;
        public Quaternion LocalRotation = Quaternion.Identity;
        public Vector3 LocalScale = Vector3.One;

        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public Vector3 Scale { get; private set; }

        public Matrix4 WorldMatrix;
        public Matrix4 LocalMatrix =>
                Matrix4.CreateScale(LocalScale)
                * Matrix4.CreateFromQuaternion(LocalRotation)
                * Matrix4.CreateTranslation(LocalPosition);

        public override void Update()
        {
            base.Update();

            if (Gameobject.Parent == null)
            {
                WorldMatrix = LocalMatrix;
                Position = LocalPosition;
                Rotation = LocalRotation;
                Scale = LocalScale;
            }
            else
            {
                WorldMatrix = Gameobject.Parent.transform.WorldMatrix * LocalMatrix;
                Position = WorldMatrix.ExtractTranslation();
                Rotation = Gameobject.Parent.transform.Rotation * LocalRotation;
                Scale = Gameobject.Parent.transform.Scale * LocalScale;
            }
        }
    }
}
