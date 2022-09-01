using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI.Items
{
    public class Blast : Effect
    {
        public Blast(GameController controller, Rectangle rect) : base(controller)
        {
            var exp1 = Resources.EXP1;
            var exp2 = Resources.EXP2;
            var exp3 = Resources.EXP3;
            var exp4 = Resources.EXP4;
            var exp5 = Resources.EXP5;
            exp1.MakeTransparent(Color.Black);
            exp2.MakeTransparent(Color.Black);
            exp3.MakeTransparent(Color.Black);
            exp4.MakeTransparent(Color.Black);
            exp5.MakeTransparent(Color.Black);

            Music.Blast.Play();
            Frames = new[] { exp1, exp2, exp3, exp4, exp5 };
            OnFinish = () => controller.Effects.Remove(this);
            Rect = rect;
        }
    }
}
