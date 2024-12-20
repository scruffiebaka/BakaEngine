using System;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace BakaEngine.Core.ECS.Components
{
    public class Transform : Component
    {
        public Vector3 localPosition = Vector3.Zero;
        public Vector3 localScale = Vector3.Zero;
        public Vector3 localRotation = Vector3.Zero;

        private Vector3 worldPosition = Vector3.Zero;
        private Vector3 worldScale = Vector3.Zero;
        private Vector3 worldRotation = Vector3.Zero;
    }
}
