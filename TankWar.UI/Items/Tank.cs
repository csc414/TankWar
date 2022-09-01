using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI.Items
{
    public class Tank : MoveObject
    {
        public Tank(GameController controller, int hp, Bitmap upImg, Bitmap downImg, Bitmap leftImg, Bitmap rightImg, MoveDirection direction, int speed, int x, int y) : base(controller, null, new Rectangle(x, y, 0, 0))
        {
            Hp = hp;
            upImg.MakeTransparent(Color.White);
            downImg.MakeTransparent(Color.White);
            leftImg.MakeTransparent(Color.White);
            rightImg.MakeTransparent(Color.White);
            UpImg = upImg;
            DownImg = downImg;
            LeftImg = leftImg;
            RightImg = rightImg;
            SetDirection(direction);
            Speed = speed;
        }

        public int Hp { get; set; }

        public bool Shooting { get; private set; }

        private Bitmap UpImg { get; }

        private Bitmap DownImg { get; }

        private Bitmap LeftImg { get; }

        private Bitmap RightImg { get; }

        private int _shootCount = 0;

        public virtual bool IsCollide(ref Rectangle rect)
        {
            return Controller.IsCollideWall(ref rect, out _);
        }

        public override int OnMoveCheck(MoveDirection direction)
        {
            var rect = Rect;
            switch (direction)
            {
                case MoveDirection.Up:
                    {
                        rect.Y -= Speed;
                        if (IsCollide(ref rect) || rect.Y <= 0)
                            return Rect.Y;

                        return rect.Y;
                    }
                case MoveDirection.Down:
                    {
                        rect.Y += Speed;
                        if (IsCollide(ref rect) || rect.Y + Img.Height >= Controller.Height)
                            return Rect.Y;

                        return rect.Y;
                    }
                case MoveDirection.Left:
                    {
                        rect.X -= Speed;
                        if (IsCollide(ref rect) || rect.X <= 0)
                            return Rect.X;

                        return rect.X;
                    }
                case MoveDirection.Right:
                    {
                        rect.X += Speed;
                        if (IsCollide(ref rect) || rect.X + Img.Width >= Controller.Width)
                            return Rect.X;

                        return rect.X;
                    }
                default:
                    throw new Exception("Unknown type of Direction");
            }
        }

        public void SetDirection(MoveDirection direction)
        {
            if (Direction != direction)
            {
                Direction = direction;
                Img = Direction switch
                {
                    MoveDirection.Up => UpImg,
                    MoveDirection.Down => DownImg,
                    MoveDirection.Left => LeftImg,
                    MoveDirection.Right => RightImg,
                    _ => throw new Exception("Unknown type of Direction"),
                };
                Rect.Width = Img.Width;
                Rect.Height = Img.Height;
            }
        }

        public void Shoot()
        {
            var bullet = new Bullet(Controller, this, Direction, Rect);
            Controller.Bullets.Add(bullet);
            Music.Fire.Play();
        }

        public void StartShooting()
        {
            if (!Shooting)
            {
                _shootCount = 0;
                Shooting = true;
            }
        }

        public void EndShooting()
        {
            if (Shooting)
                Shooting = false;
        }

        public override void Render()
        {
            if (Shooting)
            {
                _shootCount--;
                if (_shootCount <= 0)
                {
                    Shoot();
                    _shootCount = 10;
                }
            }

            base.Render();
        }
    }
}
