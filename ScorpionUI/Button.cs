using ScorpionCore;
using ScorpionCore.Content;
using ScorpionCore.Graphics;
using ScorpionCore.Input;

namespace ScorpionUI
{
    public class Button : IControl
    {
        #region Fields
        private Mouse _mouse;
        private Rect _rect;
        #endregion


        #region Constructors
        public Button()
        {
            _mouse = new Mouse();
            _rect = new Rect()
            {
                X = Position.X,
                Y = Position.Y,
                Width = Width,
                Height = Height
            };
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets the position of the <see cref="Button"/> on the screen.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets or sets the width of the <see cref="Button"/>.
        /// </summary>
        public int Width
        {
            get
            {
                if (MouseOverTexture == null || MouseNotOverTexture == null)
                    return 0;

                return MouseOverTexture.Width > MouseNotOverTexture.Width ?
                    MouseOverTexture.Width :
                    MouseNotOverTexture.Width;
            }
        }

        /// <summary>
        /// Gets or sets the height of the <see cref="Button"/>.
        /// </summary>
        public int Height
        {
            get
            {
                if (MouseOverTexture == null || MouseNotOverTexture == null)
                    return 0;

                return MouseOverTexture.Height > MouseNotOverTexture.Height ?
                    MouseOverTexture.Height :
                    MouseNotOverTexture.Height;
            }
        }

        /// <summary>
        /// Gets or sets the texture of the when the mouse is over the <see cref="Button"/>.
        /// </summary>
        public Texture MouseOverTexture { get; set; }

        /// <summary>
        /// Gets or sets the texture of the when the mouse is not over the <see cref="Button"/>.
        /// </summary>
        public Texture MouseNotOverTexture { get; set; }

        /// <summary>
        /// Gets a value indicating if the mouse is over the button.
        /// </summary>
        public bool IsMouseOver { get; private set; }

        /// <summary>
        /// Gets or sets the text of the button.
        /// </summary>
        public GameText Text { get; set; }
        #endregion


        #region Public Methods
        public void Initialize()
        {
        }


        public void LoadContent(ContentLoader contentLoader)
        {
            MouseOverTexture = contentLoader.LoadTexture($"MouseOverButton");
            MouseNotOverTexture = contentLoader.LoadTexture($"MouseNotOverButton");
            Text = contentLoader.LoadText("Button");
            Text.Text = "Hello";
        }


        /// <summary>
        /// Updates the <see cref="Button"/>.
        /// </summary>
        /// <param name="engineTime">The amount of time that has passed in the engine since the last frame.</param>
        public void Update(EngineTime engineTime)
        {
            //Update the rect's position
            _rect.X = Position.X;
            _rect.Y = Position.Y;

            _mouse.UpdateCurrentState();

            IsMouseOver = _rect.Contains(_mouse.X, _mouse.Y);

            _mouse.UpdatePreviousState();
        }


        /// <summary>
        /// Renders the <see cref="Button"/> to the screen.
        /// </summary>
        /// <param name="renderer">Renders the <see cref="Button"/>.</param>
        public void Render(Renderer renderer)
        {
            if (IsMouseOver && MouseOverTexture != null)
            {
                renderer.Render(MouseOverTexture, Position.X, Position.Y);
            }
            else
            {
                if (MouseNotOverTexture != null)
                    renderer.Render(MouseNotOverTexture, Position.X, Position.Y, 0);
            }
        }
        #endregion
    }
}
