using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI.Items
{
    public enum TankDirection
    {
        Unknow,
        Up,
        Down,
        Left,
        Right
    }

    public class Tank : GameObject
    {
        public Tank(Graphics g, Bitmap upImg, Bitmap downImg, Bitmap leftImg, Bitmap rightImg, TankDirection direction, int speed, int x, int y) : base(g, null, new Rectangle(x, y, 0, 0))
        {
            Speed = speed;
            upImg.MakeTransparent(Color.White);
            downImg.MakeTransparent(Color.White);
            leftImg.MakeTransparent(Color.White);
            rightImg.MakeTransparent(Color.White);
            UpImg = upImg;
            DownImg = downImg;
            LeftImg = leftImg;
            RightImg = rightImg;
            SetDirection(direction);
        }

        private Bitmap UpImg { get; }

        private Bitmap DownImg { get; }

        private Bitmap LeftImg { get; }

        private Bitmap RightImg { get; }

        public TankDirection Direction { get; protected set; }

        public void SetDirection(TankDirection direction)
        {
            if (Direction != direction)
            {
                Direction = direction;
                Img = Direction switch
                {
                    TankDirection.Up => UpImg,
                    TankDirection.Down => DownImg,
                    TankDirection.Left => LeftImg,
                    TankDirection.Right => RightImg,
                    _ => throw new ArgumentException("Unknown type of Direction", nameof(Direction)),
                };
                Rect.Width = Img.Width;
                Rect.Height = Img.Height;
            }
        }

        public int Speed { get; set; }

        public bool Moving { get; set; }

        public event Func<Tank, TankDirection, int> OnMoveCheck;

        public override void Render()
        {
            if (Moving)
            {
                switch (Direction)
                {
                    case TankDirection.Up:
                    case TankDirection.Down:
                        var y = OnMoveCheck(this, Direction);
                        if (y > 0)
                            Rect.Y = y;
                        break;
                    case TankDirection.Left:
                    case TankDirection.Right:
                        var x = OnMoveCheck(this, Direction);
                        if (x > 0)
                            Rect.X = x;
                        break;
                }
            }

            base.Render();
        }
    }
}
