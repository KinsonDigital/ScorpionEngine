﻿using KDScorpionCore;
using KDScorpionCore.Content;
using KDScorpionCore.Graphics;
using KDScorpionCore.Input;
using System;

namespace KDScorpionUI
{
    public class Button : IControl
    {
        #region Events
        public event EventHandler<EventArgs> Click;
        #endregion


        #region Fields
        private readonly Mouse _mouse;
        private Rect _rect = new Rect();
        private bool _isMouseDown;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="Button"/>.
        /// </summary>
        public Button()
        {
            _mouse = new Mouse();
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
        /// Gets or sets the texture when the left mouse button is
        /// in the down position over the button.
        /// </summary>
        public Texture MouseDownTexture { get; set; }

        /// <summary>
        /// Gets a value indicating if the mouse is over the button.
        /// </summary>
        public bool IsMouseOver { get; private set; }

        /// <summary>
        /// Gets or sets the text of the button.
        /// </summary>
        public GameText ButtonText { get; set; }
        #endregion


        #region Public Methods
        public void Initialize()
        {
        }


        public void LoadContent(ContentLoader contentLoader)
        {
        }


        /// <summary>
        /// Updates the <see cref="Button"/>.
        /// </summary>
        /// <param name="engineTime">The amount of time that has passed in the engine since the last frame.</param>
        public void Update(EngineTime engineTime)
        {
            ProcessMouse();

            _rect.X = Position.X - Width / 2f;
            _rect.Y = Position.Y - Height / 2f;
            _rect.Width = Width;
            _rect.Height = Height;
        }


        /// <summary>
        /// Renders the <see cref="Button"/> to the screen.
        /// </summary>
        /// <param name="renderer">Renders the <see cref="Button"/>.</param>
        public void Render(Renderer renderer)
        {
            if (_isMouseDown && MouseDownTexture != null)
            {
                renderer.Render(MouseDownTexture, Position.X, Position.Y);
            }
            else
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

            var textPosition = new Vector()
            {
                X = Position.X - ButtonText.Width / 2f,
                Y = Position.Y - ButtonText.Height / 2f
            };

            if(ButtonText != null)
                renderer.Render(ButtonText, textPosition, new GameColor(255, 0, 0, 0));
        }
        #endregion


        #region Private Methods
        private void ProcessMouse()
        {
            _mouse.UpdateCurrentState();

            IsMouseOver = _rect.Contains(_mouse.X, _mouse.Y);

            _isMouseDown = IsMouseOver && _mouse.IsButtonDown(InputButton.LeftButton);

            if (_isMouseDown)
                Click?.Invoke(this, new EventArgs());

            _mouse.UpdatePreviousState();
        }
        #endregion
    }
}
