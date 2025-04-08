using System;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using BakaEngine.Core.Helpers;
using BakaEngine.Core.Rendering;
using BakaEngine.Core.ECS;
using BakaEngine.Core.ECS.Components;
using BakaEngine.Core.ECS.Scenes;

namespace BakaEngine.Core
{
    public class Window : GameWindow
    {
        public Window(int width, int height, string Title) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { Title = Title, ClientSize = new Vector2i(800, 640), MaximumClientSize = new Vector2i(800, 640) })
        {
            CenterWindow();
        }

        KeyboardState Input;
        FrameEventArgs updateArguements;

        Shader shader;

        Texture baseTex;

        Entity cameraObject;
        Entity cubeObject;

        Camera camera;
        float xRotation, yRotation;

        private bool _firstMove = true;
        private Vector2 _lastPos;

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.LoadBindings(new GLFWBindingsContext());

            GL.ClearColor(new Color4(24, 20, 28, 1));

            GL.Enable(EnableCap.DepthTest);

            shader = new Shader("./Resources/Shaders/VertexShader.glsl", "./Resources/Shaders/FragmentShader.glsl");
            shader.Use();

            TestInitial_Entities();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();


            if (SceneManager.currentActiveScene != null)
            {
                //Manage Cameras
                if (SceneManager.currentActiveScene.currentActiveCamera != null)
                {
                    SceneManager.currentActiveScene.currentActiveCamera.GetComponent<Camera>().UpdateVectors(MathHelper.DegreesToRadians(SceneManager.currentActiveScene.currentActiveCamera.transform.eulerAngles.X), MathHelper.DegreesToRadians(SceneManager.currentActiveScene.currentActiveCamera.transform.eulerAngles.Y));
                    shader.SetMatrix4("view", SceneManager.currentActiveScene.currentActiveCamera.GetComponent<Camera>().GetViewMatrix(SceneManager.currentActiveScene.currentActiveCamera.transform.Position));
                    shader.SetMatrix4("projection", SceneManager.currentActiveScene.currentActiveCamera.GetComponent<Camera>().GetProjectionMatrix());
                }
                else
                {
                    Debug.Error("No camera active.");
                    GL.ClearColor(new Color4(0, 0, 0, 1));
                }

                //Draw all meshes in the scene.
                foreach (Entity entity in SceneManager.currentActiveScene.entities)
                {
                    if (entity == null)
                    {
                        continue;
                    }
                    if (entity.TryGetComponent<MeshRenderer>(out var renderer) && entity.TryGetComponent<Transform>(out var transform))
                    {
                        foreach (Mesh mesh in renderer.Meshes)
                        {
                            renderer.Draw(shader, transform, mesh);
                        }
                    }
                }
            }
            else
            {
                Debug.Error("No scene active.");
                GL.ClearColor(new Color4(0, 0, 0, 1));
            }

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            Input = KeyboardState;
            updateArguements = args;

            if (Input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            TestCamera_Movement();
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            shader.Dispose();
        }

        #region TEST
        private void TestInitial_Entities()
        {
            CursorState = CursorState.Grabbed;
            Scene demoScene = new Scene("DemoScene");

            SceneManager.SetActiveScene(demoScene);

            cameraObject = new Entity("Camera");
            camera = cameraObject.GetComponent<Camera>();

            baseTex = new Texture(Texture.LoadFromFile("Resources/Textures/texture.png"), "texture_diffuse");

            cubeObject = new Entity("Cube");
            MeshRenderer cubeMesh = cubeObject.GetComponent<MeshRenderer>();
            cubeMesh.Meshes.Add(new Mesh(new List<Vertex> {
            // Front face
            new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Normal = new Vector3(0, 0, 1), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Normal = new Vector3(0, 0, 1), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Normal = new Vector3(0, 0, 1), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Normal = new Vector3(0, 0, 1), TexCoords = new Vector2(0, 1) },

            // Back face
            new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Normal = new Vector3(0, 0, -1), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Normal = new Vector3(0, 0, -1), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Normal = new Vector3(0, 0, -1), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Normal = new Vector3(0, 0, -1), TexCoords = new Vector2(0, 1) },

            // Left face
            new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Normal = new Vector3(-1, 0, 0), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Normal = new Vector3(-1, 0, 0), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Normal = new Vector3(-1, 0, 0), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Normal = new Vector3(-1, 0, 0), TexCoords = new Vector2(0, 1) },

            // Right face
            new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Normal = new Vector3(1, 0, 0), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Normal = new Vector3(1, 0, 0), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Normal = new Vector3(1, 0, 0), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Normal = new Vector3(1, 0, 0), TexCoords = new Vector2(0, 1) },

            // Top face
            new Vertex { Position = new Vector3(-0.5f,  0.5f, -0.5f), Normal = new Vector3(0, 1, 0), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3(-0.5f,  0.5f,  0.5f), Normal = new Vector3(0, 1, 0), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f,  0.5f), Normal = new Vector3(0, 1, 0), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3( 0.5f,  0.5f, -0.5f), Normal = new Vector3(0, 1, 0), TexCoords = new Vector2(0, 1) },

            // Bottom face
            new Vertex { Position = new Vector3(-0.5f, -0.5f, -0.5f), Normal = new Vector3(0, -1, 0), TexCoords = new Vector2(0, 0) },
            new Vertex { Position = new Vector3(-0.5f, -0.5f,  0.5f), Normal = new Vector3(0, -1, 0), TexCoords = new Vector2(1, 0) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f,  0.5f), Normal = new Vector3(0, -1, 0), TexCoords = new Vector2(1, 1) },
            new Vertex { Position = new Vector3( 0.5f, -0.5f, -0.5f), Normal = new Vector3(0, -1, 0), TexCoords = new Vector2(0, 1) },
        }, new List<uint>{
            // Front face
            0, 1, 2, 2, 3, 0,
            // Back face
            4, 5, 6, 6, 7, 4,
            // Left face
            8, 9, 10, 10, 11, 8,
            // Right face
            12, 13, 14, 14, 15, 12,
            // Top face
            16, 17, 18, 18, 19, 16,
            // Bottom face
            20, 21, 22, 22, 23, 20
        }));
            cubeMesh.Textures.Add(baseTex);

            if (camera != null) demoScene.SetActiveCamera(cameraObject);
            demoScene.entities.Add(cameraObject);
            demoScene.entities.Add(cubeObject);

            cameraObject.transform.Rotation = Quaternion.FromEulerAngles(0, -MathHelper.PiOver2, 0);
        }
        private void TestCamera_Movement()
        {
            const float cameraSpeed = 1.5f;
            const float sensitivity = 1f;

            if (Input.IsKeyDown(Keys.W))
            {
                cameraObject.transform.Position += camera.Front * cameraSpeed * (float)updateArguements.Time; // Forward
            }

            if (Input.IsKeyDown(Keys.S))
            {
                cameraObject.transform.Position -= camera.Front * cameraSpeed * (float)updateArguements.Time; // Backwards
            }
            if (Input.IsKeyDown(Keys.A))
            {
                cameraObject.transform.Position -= camera.Right * cameraSpeed * (float)updateArguements.Time; // Left
            }
            if (Input.IsKeyDown(Keys.D))
            {
                cameraObject.transform.Position += camera.Right * cameraSpeed * (float)updateArguements.Time; // Right
            }
            if (Input.IsKeyDown(Keys.Space))
            {
                cameraObject.transform.Position += camera.Up * cameraSpeed * (float)updateArguements.Time; // Up
            }
            if (Input.IsKeyDown(Keys.LeftShift))
            {
                cameraObject.transform.Position -= camera.Up * cameraSpeed * (float)updateArguements.Time; // Down
            }

            Quaternion rotation = cubeObject.transform.Rotation;

            float angle = cameraSpeed * (float)updateArguements.Time;

            // Create axis-angle quaternions
            if (Input.IsKeyDown(Keys.K)) // Rotate around local X+
                rotation = Quaternion.FromAxisAngle(Vector3.UnitX, angle) * rotation;
            if (Input.IsKeyDown(Keys.J)) // Rotate around local X-
                rotation = Quaternion.FromAxisAngle(Vector3.UnitX, -angle) * rotation;
            if (Input.IsKeyDown(Keys.H)) // Rotate around local Y+
                rotation = Quaternion.FromAxisAngle(Vector3.UnitY, angle) * rotation;
            if (Input.IsKeyDown(Keys.L)) // Rotate around local Y-
                rotation = Quaternion.FromAxisAngle(Vector3.UnitY, -angle) * rotation;

            rotation.Normalize(); // Always normalize to avoid drift
            cubeObject.transform.Rotation = rotation;

            var mouse = MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                yRotation += deltaX * sensitivity * (float)updateArguements.Time;
                xRotation -= deltaY * sensitivity * (float)updateArguements.Time;

                xRotation = Math.Clamp(xRotation, -90f, 90f);
                //TODO: Make a function called quaternion.euler() that takes in degrees for the fromeulerangles function.
                cameraObject.transform.Rotation = Quaternion.FromEulerAngles(xRotation, yRotation, 0);
            }
            Debug.Log(cameraObject.transform.eulerAngles.X.ToString() + "  " +cameraObject.transform.eulerAngles.Y.ToString() + "  " + cameraObject.transform.eulerAngles.Z.ToString());
        }

        #endregion
    }
}