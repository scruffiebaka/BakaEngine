using System;

using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace BakaEngine.Core.Rendering
{
    public class Texture
    {
        public readonly int Handle;
        public string Type;

        public Texture(int handle, string type)
        {
            Handle = handle;
            Type = type;
        }

        public static int LoadFromFile(string TexturePath)
        {
            int textureID = GL.GenTexture();

            StbImage.stbi_set_flip_vertically_on_load(1);

            using (Stream stream = File.OpenRead(TexturePath))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                GL.BindTexture(TextureTarget.Texture2D, textureID);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }
            
            //Set Filtering
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
            //Set Wrapping
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (float)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (float)TextureWrapMode.Repeat);
            
            return textureID;
        }
    }
}
