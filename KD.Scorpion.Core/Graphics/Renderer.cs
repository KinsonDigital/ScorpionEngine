using KDScorpionCore.Plugins;

namespace KDScorpionCore.Graphics
{
    public class Renderer
    {
        #region Constructors
        public Renderer(IRenderer renderer)
        {
            InternalRenderer = renderer;
        }
        #endregion


        #region Props
        public IRenderer InternalRenderer { get; set; }
        #endregion


        #region Public Methods
        public void Clear(byte red, byte green, byte blue, byte alpha) => InternalRenderer.Clear(new GameColor(alpha, red, green, blue));


        public void Start() => InternalRenderer.Start();


        public void End() => InternalRenderer.End();


        public void Render(Texture texture, float x, float y) => InternalRenderer.Render(texture.InternalTexture, x, y);


        public void Render(Texture texture, Vector position) => Render(texture, position.X, position.Y);


        //Angle is in degrees
        public void Render(Texture texture, float x, float y, float angle) => 
            InternalRenderer.Render(texture.InternalTexture, x, y, angle);


        //Angle is in degrees
        public void Render(Texture texture, float x, float y, float angle, float size, GameColor color) => 
            InternalRenderer.Render(texture.InternalTexture, x, y, angle, size, color);


        public void RenderTextureArea(Texture texture, Rect area, Vector position) => 
            InternalRenderer.RenderTextureArea(texture.InternalTexture, area, position.X, position.Y);


        public void Render(GameText text, float x, float y) =>
            InternalRenderer.Render(text.InternalText, x, y);


        /// <summary>
        /// Renders the given text at the given <paramref name="x"/> and <paramref name="y"/>
        /// location and using the given <paramref name="color"/>.
        /// </summary>
        /// <param name="text">The text to render.</param>
        /// <param name="x">The X coordinate location of where to render the text.</param>
        /// <param name="y">The Y coordinate location of where to render the text.</param>
        /// <param name="color">The color to render the text.</param>
        public void Render(GameText text, float x, float y, GameColor color) => InternalRenderer.Render(text.InternalText, x, y, color);


        public void Render(GameText text, Vector position) => Render(text, position.X, position.Y);


        public void Render(GameText text, Vector position, GameColor color) => Render(text, position.X, position.Y, color);


        public void FillCircle(Vector position, float radius, GameColor color) => 
            InternalRenderer.FillCircle(position.X, position.Y, radius, color);


        public void FillRect(Rect rect, GameColor color) => InternalRenderer.FillRect(rect, color);


        public void Line(Vector start, Vector end, GameColor color) => InternalRenderer.RenderLine(start.X, start.Y, end.X, end.Y, color);
        #endregion
    }
}
