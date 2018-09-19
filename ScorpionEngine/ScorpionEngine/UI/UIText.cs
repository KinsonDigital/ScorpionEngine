using ScorpionEngine.Content;
using ScorpionEngine.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.UI
{
    /// <summary>
    /// Represents a single piece of text rendered to a screen.
    /// </summary>
    public class UIText
    {
        #region Private Vars
        private IText _labelFont;//The font for the label section of the text item
        private IText _valueFont;//The font for the value section of the text item.  This is the dynamic text that gets updated.
        private int _elapsedTime;//The amount of time that has elapsed since the last frame in miliseconds.
        private bool _updateText = true;//Indicates if the text can be updated.  Only updated if the UpdateFrequency value is >= to the elapsed time
        private string _labelText;//The label section of the text item.
        private string _valueText;//The value section of the text item.
        private int _labelWidth;//The width of the label section
        private int _labelHeight;//The height of the label section
        private int _valueWidth;//The width of the value section
        private int _valueHeight;//The height of the value section
        #endregion


        #region Constructors
        /// <summary>
        /// Creates a new instance of an <see cref="UIText"/> item.
        /// </summary>
        /// <param name="labelFont">The font used for the label section of the text item.</param>
        /// <param name="valueFont">The font used for the value section of the text item.</param>
        public UIText(IText labelFont, IText valueFont)
        {
            _labelFont = labelFont;
            _valueFont = valueFont;
            LabelText = "";
            ValueText = "";
            Position = Vector.Zero;
        }


        /// <summary>
        /// Creates a new instance of an <see cref="UIText"/> item.
        /// </summary>
        /// <param name="labelFont">The font used for the label section of the text item.</param>
        /// <param name="valueFont">The font used for the value section of the text item.</param>
        /// <param name="labelText">The text of the label section of the text item.</param>
        /// <param name="valueText">The text of the value section of the text item.</param>
        public UIText(IText labelFont, IText valueFont, string label = "", string value = "")
        {
            _labelFont = labelFont;
            _valueFont = valueFont;
            LabelText = label;
            ValueText = value;
            Position = Vector.Zero;
        }


        /// <summary>
        /// Creates a new instance of an <see cref="UIText"/> item.
        /// </summary>
        /// <param name="labelFont">The font used for the label section of the text item.</param>
        /// <param name="valueFont">The font used for the value section of the text item.</param>
        /// <param name="position">The position to to render the text item.</param>
        /// <param name="labelText">The text of the label section of the text item.</param>
        /// <param name="valueText">The text of the value section of the text item.</param>
        public UIText(IText labelFont, IText valueFont, Vector position, string labelText = "", string valueText = "")
        {
            _labelFont = labelFont;
            _valueFont = valueFont;
            LabelText = labelText;
            ValueText = valueText;
            Position = position;
        }


        /// <summary>
        /// Creates a new instance of an <see cref="UIText"/> item.
        /// </summary>
        /// <param name="labelFont">The font used for the label section of the text item.</param>
        /// <param name="valueFont">The font used for the value section of the text item.</param>
        /// <param name="x">The X location of the text item.</param>
        /// <param name="y">The Y location of the text item.</param>
        /// <param name="labelText">The text of the label section of the text item.</param>
        /// <param name="valueText">The text of the value section of the text item.</param>
        public UIText(IText labelFont, IText valueFont, int x = 0, int y = 0, string labelText = "", string valueText = "")
        {
            _labelFont = labelFont;
            _valueFont = valueFont;
            LabelText = labelText;
            ValueText = valueText;
            Position = new Vector(x, y);
        }
        #endregion


        #region Props
        /// <summary>
        /// Gets or sets a value indicating if the update frequency should be ignored.
        /// </summary>
        public bool IgnoreUpdateFrequency { get; set; } = true;

        /// <summary>
        /// Gets or sets the selected color of the text item.
        /// </summary>
        public Color SelectedColor { get; set; } = Color.FromArgb(255, 255, 0, 255);

        /// <summary>
        /// Gets or sets the name of the text item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the label section of the text item.
        /// </summary>
        public string LabelText
        {
            get => _labelText;
            set
            {
                if (_updateText || UpdateFrequency == 0 || IgnoreUpdateFrequency)
                {
                    _labelText = value;
                    _updateText = false;

                    //TODO: Refactor name of _labelFont to _labelText
                    //Do the same with _valueFont
                    _labelWidth = _labelFont.Width;
                    _labelHeight = _labelFont.Height;
                }
            }
        }

        /// <summary>
        /// Gets or sets the information of the stat to display.
        /// </summary>
        public string ValueText
        {
            get => _valueText;
            set
            {
                if (_updateText || UpdateFrequency == 0 || IgnoreUpdateFrequency)
                {
                    _valueText = value;
                    _updateText = false;

                    _valueWidth = _valueFont.Width;
                    _valueHeight = _valueFont.Height;
                }
            }
        }

        /// <summary>
        /// Gets or sets the position of the text item.
        /// </summary>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the text will render as selected.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets the frequency in milliseconds that the text will get updated.
        /// </summary>
        public int UpdateFrequency { get; set; } = 62;

        /// <summary>
        /// Gets or sets the size of the text item. <see cref="Vector.X"/> is for the width and <see cref="Vector.Y"/> is for the height.
        /// </summary>
        public Vector TextItemSize
        {
            get
            {
                return new Vector(Width, Height);
            }
        }

        /// <summary>
        /// Gets the width of the entire text item.
        /// </summary>
        public int Width
        {
            get
            {
                return _labelWidth + SectionSpacing + _valueWidth;
            }
        }

        /// <summary>
        /// Gets the height of the entire text item.
        /// </summary>
        public int Height
        {
            get
            {
                //Return the greatest height
                return _labelHeight > _valueHeight ? _labelHeight : _valueHeight;
            }
        }

        /// <summary>
        /// Gets the location of the right side of the <see cref="UIText"/> item.
        /// </summary>
        public int Right
        {
            get
            {
                return (int)Position.X + Width;
            }
        }

        /// <summary>
        /// Gets the location of the bottom of the <see cref="UIText"/> item.
        /// </summary>
        public int Bottom
        {
            get
            {
                return (int)Position.Y + _labelHeight;
            }
        }

        /// <summary>
        /// Adds an additional amont to the vertical position of the label text section.
        /// </summary>
        public int VerticalLabelOffset { get; set; } = 0;

        /// <summary>
        /// Adds an additional amont to the vertical position of the value text section.
        /// </summary>
        public int VerticalValueOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the spacing between the label and value sections.
        /// </summary>
        public int SectionSpacing { get; set; } = 5;

        /// <summary>
        /// Gets or sets the color of the label section of the text item.
        /// </summary>
        public Color LabelColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets the color of the value section of the text item.
        /// </summary>
        public Color ValueColor { get; set; } = Color.Black;

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="UIText"/> item will render in the
        /// regular color or disabled color.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the forecolor of the <see cref="UIText"/> item when disabled.
        /// </summary>
        public Color DisabledForecolor { get; set; } = Color.FromArgb(100, 100, 100);
        #endregion


        #region Public Methods
        /// <summary>
        /// Updates the text item. This helps keep the update frequency up to date.
        /// </summary>
        /// <param name="gameTime">The frame time information of the last frame.</param>
        public void Update(IEngineTiming gameTime)
        {
            _elapsedTime += gameTime.ElapsedEngineTime.Milliseconds;

            if (_elapsedTime >= UpdateFrequency)
            {
                _elapsedTime = 0;
                _updateText = true;
            }
        }


        /// <summary>
        /// Render the text item to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use to render.</param>
        public void Render(IRenderer spriteBatch)
        {
            //If the item is selected, render the background color
            //if (Selected)
            //    spriteBatch.FillRectangle(new Rectangle((int)Position.X, (int)Position.Y, Width, Height), SelectedColor);

            //var yPosition = Position.Y + (_labelHeight / 2) - (_valueHeight / 2);

            //spriteBatch.DrawString(_labelFont, $"{LabelText}:", new Vector(Position.X, Position.Y + VerticalLabelOffset), Enabled ? LabelColor : DisabledForecolor);
            //spriteBatch.DrawString(_valueFont, ValueText, new Vector(Position.X + _labelWidth + SectionSpacing, Position.Y + VerticalValueOffset), Enabled ? ValueColor : DisabledForecolor);
        }
        #endregion
    }
}
