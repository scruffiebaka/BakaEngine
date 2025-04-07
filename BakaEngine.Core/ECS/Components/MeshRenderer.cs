using System.Collections.Generic;
using BakaEngine.Core.Helpers;
using BakaEngine.Core.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace BakaEngine.Core.ECS.Components
{
    public class MeshRenderer : Component
    {
        public List<Mesh> Meshes = new List<Mesh>();
        public List<Texture> Textures = new List<Texture>();

        public void Draw(Shader shader, Transform transform, Mesh mesh)
        {
            shader.SetMatrix4("mesh", 
                Matrix4.CreateTranslation(transform.Position)
                * Matrix4.CreateScale(transform.Scale)
                * Matrix4.CreateFromQuaternion(transform.QuaternionRotation));

            if (Textures.Count > 0)
            {
                int diffuseN = 1;
                int specularN = 1;
                for (int i = 0; i < Textures.Count(); i++)
                {
                    GL.ActiveTexture(TextureUnit.Texture0 + i);
                    string number;
                    string name = Textures[i].Type;
                    if(name == "texture_diffuse")
                    {
                        number = diffuseN++.ToString();
                    }
                    else if(name == "texture_specular")
                    {
                        number = specularN++.ToString();
                    }
                    else
                    {
                        Debug.Error("Texture name not formatted.");
                        return;
                    }

                    shader.SetInt(name + "" + number, Textures[i].Handle);
                    GL.BindTexture(TextureTarget.Texture2D, Textures[i].Handle);
                }
                GL.ActiveTexture(TextureUnit.Texture0);
            }

            GL.BindVertexArray(mesh.VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, mesh.Indices.Count, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }

    }
}