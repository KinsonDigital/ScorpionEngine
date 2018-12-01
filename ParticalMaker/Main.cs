using ScorpionCore;
using ScorpionCore.Content;
using ScorpionCore.Graphics;
using ScorpionEngine;
using ScorpionUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParticalMaker
{
    public class Main : Engine
    {
        private Button _button;


        public Main() : base(false)
        {
        }


        public override void Init()
        {
            _button = new Button();

            base.Init();
        }


        public override void LoadContent(ContentLoader contentLoader)
        {
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
