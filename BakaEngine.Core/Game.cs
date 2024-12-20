using System;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using BakaEngine.Core.Helpers;
using BakaEngine.Core.Rendering;
using BakaEngine.Core.Scenes;
using BakaEngine.Core.ECS;
using BakaEngine.Core.ECS.Components;

namespace BakaEngine.Core
{
    public class Game : GameWindow
	{
		public Game(int width, int height, string Title) : base(GameWindowSettings.Default,
			new NativeWindowSettings() { Title = Title, ClientSize = new Vector2i(800, 640), MaximumClientSize = new Vector2i(800, 640) })
		{
			CenterWindow();
		}

        Shader shader;

        private readonly float[] vertices = {
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
     0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
     0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
     0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
     0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
    -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
    -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
            };

        int VertexBufferObject;
		int VertexArrayObject;

		Texture baseTex;
		Matrix4 model;

        Entity cameraObject;
        Camera camera;

        private bool _firstMove = true;
        private Vector2 _lastPos;

        float speed = .02f;
		float xPos = 0;

		protected override void OnLoad()
		{
			base.OnLoad();

			GL.LoadBindings(new GLFWBindingsContext());

			GL.ClearColor(new Color4(24, 20, 28, 1));

            GL.Enable(EnableCap.DepthTest);

			baseTex = Texture.LoadFromFile("./Resources/Texture/Texture.png");
			baseTex.Use(TextureUnit.Texture0);

            VertexArrayObject = GL.GenVertexArray();
            VertexBufferObject = GL.GenBuffer();

			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, 
				BufferUsageHint.StaticDraw);

			GL.BindVertexArray(VertexArrayObject);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            shader = new Shader("./Resources/Shaders/VertexShader.glsl", "./Resources/Shaders/FragmentShader.glsl");
            shader.Use();

            Scene demoScene = new Scene("DemoScene");

            SceneManager.SetActiveScene(demoScene);

            cameraObject = new Entity("Camera");
            cameraObject.AddComponent(new Camera());
            camera = cameraObject.GetComponent<Camera>();

            SceneManager.SetActiveCamera(camera);

            cameraObject.transform.localRotation.Y = - MathHelper.PiOver2;
        }

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			base.OnRenderFrame(args);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			shader.Use();

			GL.BindVertexArray(VertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            model = Matrix4.CreateTranslation(xPos, 0.0f, 0.0f) * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));

            shader.SetMatrix4("model", model);

            if (SceneManager.GetActiveCamera() != null)
            {
                shader.SetMatrix4("view", SceneManager.GetActiveCamera().GetViewMatrix(cameraObject.transform.localPosition) * 3);
                shader.SetMatrix4("projection", SceneManager.GetActiveCamera().GetProjectionMatrix());
                SceneManager.GetActiveCamera().UpdateVectors(cameraObject.transform.localRotation.X, cameraObject.transform.localRotation.Y);
            }
            else
            {
                Debug.Error("No camera active.");
                GL.ClearColor(new Color4(0, 0, 0, 1));
            }

            SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			base.OnUpdateFrame(args);

            //Update Cameras

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
			{
				Close();
			}

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.01f;
            
            if (input.IsKeyDown(Keys.W))
            {
                cameraObject.transform.localPosition += camera.Front * cameraSpeed * (float)args.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                cameraObject.transform.localPosition -= camera.Front * cameraSpeed * (float)args.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                cameraObject.transform.localPosition -= camera.Right * cameraSpeed * (float)args.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                cameraObject.transform.localPosition += camera.Right * cameraSpeed * (float)args.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                cameraObject.transform.localPosition += camera.Up * cameraSpeed * (float)args.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                cameraObject.transform.localPosition -= camera.Up * cameraSpeed * (float)args.Time; // Down
            }

            var mouse = MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                cameraObject.transform.localRotation.Y += deltaX * sensitivity;
                cameraObject.transform.localRotation.X -= deltaY * sensitivity;
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();

			GL.DeleteVertexArray(VertexArrayObject);
			GL.DeleteBuffer(VertexBufferObject);

            shader.Dispose();
        }
    }
}
