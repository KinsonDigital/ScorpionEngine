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
        private GameText _buttonText;
        private string _text = "";
        private string _cachedText;
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
        public string Text
        {
            get => _text;
            set => _text = value;
        }
        #endregion


        #region Public Methods
        public void Initialize()
        {
        }


        public void LoadContent(ContentLoader contentLoader)
        {
            MouseOverTexture = contentLoader.LoadTexture($"MouseOverButton");
            MouseNotOverTexture = contentLoader.LoadTexture($"MouseNotOverButton");
            _buttonText = contentLoader.LoadText("Button");
            _buttonText.Text = _text;
        }


        /// <summary>
        /// Updates the <see cref="Button"/>.
        /// </summary>
        /// <param name="engineTime">The amount of time that has passed in the engine since the last frame.</param>
        public void Update(EngineTime engineTime)
        {
            if (_buttonText != null && _buttonText.Text != _text)
                _buttonText.Text = _cachedText;

            _mouse.UpdateCurrentState();

            //Update the rect's position
            _rect.X = Position.X;
            _rect.Y = Position.Y;

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

            var textPosition = new Vector()
            {
                X = Position.X - _buttonText.Width / 2f,
                Y = Position.Y - _buttonText.Height / 2f
            };

            renderer.Render(_buttonText, textPosition, new GameColor(0, 0, 0, 255));
        }
        #endregion
    }
}
