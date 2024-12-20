using System;

using OpenTK.Mathematics;

using BakaEngine.Core.Helpers;

namespace BakaEngine.Core.ECS.Components
{
    public class Camera : Component
    {
        private Vector3 _front = -Vector3.UnitZ;

        private Vector3 _up = Vector3.UnitY;

        private Vector3 _right = Vector3.UnitX;

        private float _fov = MathHelper.PiOver2;

        public float AspectRatio { private get; set; }

        public Vector3 Front => _front;

        public Vector3 Up => _up;

        public Vector3 Right => _right;

        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 160f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        public Matrix4 GetViewMatrix(Vector3 Position)
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            //TODO: fix this!!
            //return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
            return Matrix4.CreatePerspectiveFieldOfView(_fov, 1.2017673f, 0.01f, 100f);
        }

        public void UpdateVectors(float pitch, float yaw)
        {
            _front.X = MathF.Cos(pitch) * MathF.Cos(yaw);
            _front.Y = MathF.Sin(pitch);
            _front.Z = MathF.Cos(pitch) * MathF.Sin(yaw);

            _front = Vector3.Normalize(_front);

            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }
    }
}
