using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI.Items
{
    public class Star : Effect
    {
        public Star(GameController controller, Point location) : base(controller)
        {
            var exp1 = Resources.Star1;
            var exp2 = Resources.Star2;
            var exp3 = Resources.Star3;
            exp1.MakeTransparent(Color.Black);
            exp2.MakeTransparent(Color.Black);
            exp3.MakeTransparent(Color.Black);

            Music.Add.Play();
            Frames = new[] { exp1, exp2, exp3 };
            LoopCount = 3;
            PerFrame = 3;
            OnFinish = () =>
            {
                controller.CreateEnemy(location);
                controller.Effects.Remove(this);
            };
            Rect = new Rectangle(location, new Size(exp1.Width, exp1.Height));
        }
    }
}
