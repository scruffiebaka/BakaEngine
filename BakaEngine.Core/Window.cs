using System;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using BakaEngine.Core.Helpers;
using BakaEngine.Core.Rendering;
using BakaEngine.Core;
using BakaEngine.Core.Components;
using BakaEngine.Core.Scenes;

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

        Camera camera;

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

            camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
            CursorState = CursorState.Grabbed;

            Scene demoScene = new Scene("DemoScene");
            SceneManager.SetActiveScene(demoScene);

            Texture baseTex = new Texture(Texture.LoadFromFile("Resources/Textures/texture.jpg"), TextureType.texture_diffuse);
            Material baseMat = new Material(0, 1, 32.0f);
            Gameobject cubeObject = new Gameobject("Cube");
            Mesh cubeMesh = new Mesh(BasicShapes.CUBE_VERTICES, BasicShapes.CUBE_INDICES, shader);
            MeshRenderer cubeMeshRenderer = new MeshRenderer(shader, cubeMesh, baseTex, baseMat);

            cubeObject.components.Add(typeof(MeshRenderer), cubeMeshRenderer);

            DirectionalLight dirLight = new DirectionalLight();
            dirLight.direction = new Vector3(-0.2f, -1.0f, -0.3f);

            flashlight = new Spotlight();
            flashlight.spotAngle = 30.0f;

            demoScene.lights.Add(dirLight);
            demoScene.lights.Add(flashlight);

            demoScene.entities.Add(cubeObject);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (SceneManager.currentActiveScene != null)
            {
                if (camera != null)
                {
                    shader.SetMatrix4("view", camera.GetViewMatrix());
                    shader.SetMatrix4("projection", camera.GetProjectionMatrix());
                    shader.SetVector3("viewPos", camera.Position);
                }
                else
                {
                    Debug.Error("No camera active.");
                    GL.ClearColor(new Color4(0, 0, 0, 1));
                }

                //Draw all meshes in the scene.
                foreach (Gameobject entity in SceneManager.currentActiveScene.entities)
                {
                    if (entity == null)
                    {
                        continue;
                    }
                    if (entity.TryGetComponent<MeshRenderer>(out var renderer) && entity.TryGetComponent<Transform>(out var transform))
                    {
                        renderer.Draw(transform);
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

            flashlight.position = camera.Position;
            flashlight.direction = camera.Front;

            TestCamera_Movement();
            UpdateFPS(args.Time);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }

        #region fps
        double fpsTime = 0.0;
        int fpsFrames = 0;

        void UpdateFPS(double deltaTime)
        {
            fpsTime += deltaTime;
            fpsFrames++;

            if (fpsTime >= 1.0)
            {
                double fps = fpsFrames / fpsTime;
                Console.WriteLine($"FPS: {fps:F2}");

                fpsTime = 0.0;
                fpsFrames = 0;
            }
        }
        #endregion

        #region flashlight
        Spotlight flashlight;
        private void Flashlight()
        {
            
        }
        #endregion
        private void TestCamera_Movement()
        {
            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (Input.IsKeyDown(Keys.W))
            {
                camera.Position += camera.Front * cameraSpeed * (float)updateArguements.Time; // Forward
            }

            if (Input.IsKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * cameraSpeed * (float)updateArguements.Time; // Backwards
            }
            if (Input.IsKeyDown(Keys.A))
            {
                camera.Position -= camera.Right * cameraSpeed * (float)updateArguements.Time; // Left
            }
            if (Input.IsKeyDown(Keys.D))
            {
                camera.Position += camera.Right * cameraSpeed * (float)updateArguements.Time; // Right
            }
            if (Input.IsKeyDown(Keys.Space))
            {
                camera.Position += camera.Up * cameraSpeed * (float)updateArguements.Time; // Up
            }
            if (Input.IsKeyDown(Keys.LeftShift))
            {
                camera.Position -= camera.Up * cameraSpeed * (float)updateArguements.Time; // Down
            }

            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity;
            }
        }
    }
}