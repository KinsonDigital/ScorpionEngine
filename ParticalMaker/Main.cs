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

            _mouse = new Mouse();

            base.Init();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
            _button.LoadContent(contentLoader);

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
