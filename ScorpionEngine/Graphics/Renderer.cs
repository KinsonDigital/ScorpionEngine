using ScorpionCore.Plugins;
using ScorpionEngine.Physics;

namespace ScorpionEngine.Graphics
{
    public class Renderer
    {
        internal Renderer(IRenderer renderer)
        {
            InternalRenderer = renderer;
        }


        #region Props
        internal IRenderer InternalRenderer { get; set; }
        #endregion


        #region Public Methods
        public void Clear(byte red, byte green, byte blue, byte alpha)
        {
            InternalRenderer.Clear(red, green, blue, alpha);
        }


        public void Start()
        {
            InternalRenderer.Start();
        }


        public void End()
        {
            InternalRenderer.End();
        }


        public void FillCircle(Vector position, float radius, GameColor color)
        {
            InternalRenderer.FillCircle(position.X, position.Y, radius, new byte[] { color.Red, color.Green, color.Blue, color.Alpha });
        }


        public void Render(Texture texture, float x, float y)
        {
            InternalRenderer.Render(texture.InternalTexture, x, y);
        }


        //Angle is in degrees
        public void Render(Texture texture, float x, float y, float angle)
        {
            InternalRenderer.Render(texture.InternalTexture, x, y, angle);
        }


        //Angle is in degrees
        public void Render(Texture texture, float x, float y, float angle, float size, GameColor color)
        {
            InternalRenderer.Render(texture.InternalTexture, x, y, angle, size, color.Red, color.Green, color.Blue, color.Alpha);
        }


        public void Render(GameText text, float x, float y)
        {
            InternalRenderer.Render(text.InternalText, x, y);
        }


        public void Render(GameText text, float x, float y, GameColor color)
        {
            //Temporarily hold the original color of the game text
            var tempColor = text.Color;

            //Set the color to the new requested render color
            text.Color = color;

            //Render the text.
            //Internally, the InternalRenderer is going to use the color of the InternalText.
            InternalRenderer.Render(text.InternalText, x, y);

            //Reset the game text color back to its original color
            text.Color = tempColor;
        }
        #endregion
    }
}
