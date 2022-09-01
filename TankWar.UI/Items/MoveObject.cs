using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI.Items
{
    public abstract class MoveObject : GameObject
    {
        protected MoveObject(GameController controller) : base(controller)
        {
        }

        protected MoveObject(GameController controller, Image img, Rectangle rectangle) : base(controller, img, rectangle)
        {
        }

        protected MoveObject(GameController controller, Image img, int x, int y, int width, int height) : base(controller, img, x, y, width, height)
        {
        }

        public MoveDirection Direction { get; protected set; }

        public int Speed { get; set; }

        public bool Moving { get; set; }

        public virtual int OnMoveCheck(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Up:
                    return Rect.Y - Speed;
                case MoveDirection.Down:
                    return Rect.Y + Speed;
                case MoveDirection.Left:
                    return Rect.X - Speed;
                case MoveDirection.Right:
                    return Rect.X + Speed;
            }

            return -1;
        }

        public virtual void Move()
        {
            if (Moving)
            {
                switch (Direction)
                {
                    case MoveDirection.Up:
                    case MoveDirection.Down:
                        Rect.Y = OnMoveCheck(Direction);
                        break;
                    case MoveDirection.Left:
                    case MoveDirection.Right:
                        Rect.X = OnMoveCheck(Direction);
                        break;
                }
            }
        }

        public int GetCoordinate(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Up:
                case MoveDirection.Down:
                    return Rect.Y;
                case MoveDirection.Left:
                case MoveDirection.Right:
                    return Rect.X;
            }

            return -1;
        }

        public override void Render()
        {
            Move();

            base.Render();
        }
    }
}
