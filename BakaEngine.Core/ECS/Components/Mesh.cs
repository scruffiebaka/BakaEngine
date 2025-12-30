using System;
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

using BakaEngine.Core.Rendering;
using BakaEngine.Core.Helpers;

namespace BakaEngine.Core.ECS.Components
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoords;
    }

    public class Mesh : Component
    {
        public List<Vertex> Vertices { get; private set; }
        public List<uint> Indices { get; private set; }
        public Shader shader;

        public int VertexArrayObject, VertexBufferObject, ElementBufferObject;

        public Mesh(List<Vertex> vertices, List<uint> indices, Shader shader)
        {
            this.Vertices = vertices;
            this.Indices = indices;

            this.shader = shader;

            SetupMesh();
        }

        private void SetupMesh()
        {
            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();
            ElementBufferObject = GL.GenBuffer();

            GL.BindVertexArray(VertexArrayObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            //TODO: i got 32 by calculating the size myself. this is very stupid. please fix this.
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * 32, Vertices.ToArray(), BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Count * sizeof(uint), Indices.ToArray(), BufferUsageHint.StaticDraw);

            //Position attribute
            var positionLocation = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var normalLocation = shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            var texCoordLocation = shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            
            GL.BindVertexArray(0);
        }
    }
}
