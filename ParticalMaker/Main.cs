using ScorpionCore;
using ScorpionCore.Content;
using ScorpionCore.Graphics;
using ScorpionCore.Input;
using ScorpionEngine;
using ScorpionUI;

namespace ParticalMaker
{
    public class Main : Engine
    {
        private Button _button;
        private Mouse _mouse;
        

        public Main() : base(false)
        {
        }


        public override void Init()
        {
            _button = new Button()
            {
                Position = new Vector(300, 300)
            };
            _button.Click += _button_Click;

            _mouse = new Mouse();

            base.Init();
        }

        private void _button_Click(object sender, System.EventArgs e)
        {
            _button.ButtonText.Text = "Clicked!";
        }

        public override void LoadContent(ContentLoader contentLoader)
        {
            _button.LoadContent(contentLoader);

            _button.MouseOverTexture = contentLoader.LoadTexture($"MouseOverButton");
            _button.MouseNotOverTexture = contentLoader.LoadTexture($"MouseNotOverButton");
            _button.MouseDownTexture = contentLoader.LoadTexture("MouseDownButton");
            _button.ButtonText = contentLoader.LoadText("Button");
            _button.ButtonText.Text = "Button";

            base.LoadContent(contentLoader);
        }


        public override void Update(EngineTime engineTime)
        {
            _button.Update(engineTime);

            base.Update(engineTime);
        }


        public override void Render(Renderer renderer)
        {
            _button.Render(renderer);

            base.Render(renderer);
        }
    }
}
