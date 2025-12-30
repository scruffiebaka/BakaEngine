using System;

using OpenTK.Graphics.OpenGL4;

namespace BakaEngine.Core.Rendering
{
    public class Material
    {
        public int diffuse, specular;
        public float shininess;
        public Material(int diffuse, int specular, float shininess)
        {
            this.diffuse = diffuse;
            this.specular = specular;
            this.shininess = shininess;
        }
    }
}