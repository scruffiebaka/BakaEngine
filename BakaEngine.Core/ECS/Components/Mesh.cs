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

        public int VertexArrayObject, VertexBufferObject, ElementBufferObject;

        public Mesh(List<Vertex> vertices, List<uint> indices)
        {
            this.Vertices = vertices;
            this.Indices = indices;

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
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * 32, Vertices.ToArray(),
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Count * sizeof(uint), Indices.ToArray(), BufferUsageHint.StaticDraw);

            //Position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 32, 0);
            GL.EnableVertexAttribArray(0);

            //Normal attribute
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 32, Vector3.SizeInBytes);
            GL.EnableVertexAttribArray(1);

            //TextureCoordinate attribute
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 32, Vector3.SizeInBytes * 2);
            GL.EnableVertexAttribArray(2);

            GL.BindVertexArray(0);
        }
    }
}
