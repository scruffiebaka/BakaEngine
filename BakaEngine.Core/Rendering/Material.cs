using System;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace BakaEngine.Core.Rendering
{
    public class Material
    {
        public int diffuse_textureunit, specular_textureunit;
        public Vector3 diffusecolor, specularcolor;
        public float shininess;

        public Material(Vector3 diffusecolor, Vector3 specularcolor, int diffuse_textureunit, int specular_textureunit, float shininess)
        {
            this.diffuse_textureunit = diffuse_textureunit;
            this.specular_textureunit = specular_textureunit;
            this.diffusecolor = diffusecolor;
            this.specularcolor = specularcolor;
            this.shininess = shininess;
        }
    }
}