using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI.Items
{
    public class PlayerTank : Tank
    {
        public PlayerTank(GameController controller, int hp, Bitmap upImg, Bitmap downImg, Bitmap leftImg, Bitmap rightImg, MoveDirection direction, int speed, int x, int y) : base(controller, hp, upImg, downImg, leftImg, rightImg, direction, speed, x, y)
        {
        }

        public override bool IsCollide(ref Rectangle rect)
        {
            return base.IsCollide(ref rect) || Controller.IsCollideEnemy(ref rect, out _);
        }
    }
}
