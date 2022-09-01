using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI.Items
{
    public class EnemyTank : Tank
    {
        public EnemyTank(GameController controller, int hp, Bitmap upImg, Bitmap downImg, Bitmap leftImg, Bitmap rightImg, MoveDirection direction, int speed, int x, int y) : base(controller, hp, upImg, downImg, leftImg, rightImg, direction, speed, x, y)
        {
            Moving = true;
        }

        public override bool IsCollide(ref Rectangle rect)
        {
            return base.IsCollide(ref rect) || Controller.IsCollidePlayer(ref rect);
        }

        public override int OnMoveCheck(MoveDirection direction)
        {
            var i = base.OnMoveCheck(direction);
            if (i == GetCoordinate(direction))
            {
                MoveDirection randomDirection;
                do
                {
                    randomDirection = (MoveDirection)Controller.Rd.Next(1, 5);
                }
                while (randomDirection == direction);
                SetDirection(randomDirection);
            }
            return i;
        }
    }
}
