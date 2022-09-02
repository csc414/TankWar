using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI.Items
{
    public class Bullet : MoveObject
    {
        public Bullet(GameController controller, GameObject shooter, MoveDirection direction, Rectangle rect, int speed = 6) : base(controller)
        {
            switch (direction)
            {
                case MoveDirection.Up:
                    var upImg = Resources.BulletUp;
                    upImg.MakeTransparent(Color.Black);
                    Img = upImg;
                    Rect = new Rectangle(rect.Right - rect.Width / 2 - Img.Width / 2, rect.Top - Img.Height / 2, Img.Width, Img.Height);
                    break;
                case MoveDirection.Down:
                    var downImg = Resources.BulletDown;
                    downImg.MakeTransparent(Color.Black);
                    Img = downImg;
                    Rect = new Rectangle(rect.Right - rect.Width / 2 - Img.Width / 2, rect.Bottom - Img.Height / 2, Img.Width, Img.Height);
                    break;
                case MoveDirection.Left:
                    var leftImg = Resources.BulletLeft;
                    leftImg.MakeTransparent(Color.Black);
                    Img = leftImg;
                    Rect = new Rectangle(rect.Left - Img.Width / 2, rect.Bottom - rect.Height / 2 - Img.Height / 2, Img.Width, Img.Height);
                    break;
                case MoveDirection.Right:
                    var rightImg = Resources.BulletRight;
                    rightImg.MakeTransparent(Color.Black);
                    Img = rightImg;
                    Rect = new Rectangle(rect.Right - Img.Width / 2, rect.Bottom - rect.Height / 2 - Img.Height / 2, Img.Width, Img.Height);
                    break;
            }
            Direction = direction;
            Shooter = shooter;
            Speed = speed;
            Moving = true;
        }

        public GameObject Shooter { get; }

        public override void Render()
        {
            if (Rect.X < -Rect.Width || Rect.Y < -Rect.Height || Rect.X > Controller.Width + Rect.Width || Rect.Y > Controller.Height + Rect.Height)
            {
                Controller.Bullets.Remove(this);
            }
            else if (Controller.IsCollideWall(ref Rect, out var wall))
            {
                if (wall is Wall)
                    Controller.Walls.Remove(wall);

                Controller.Bullets.Remove(this);
            }
            else if (Shooter is PlayerTank)
            {
                if (Controller.IsCollideEnemy(ref Rect, out var tank))
                {
                    tank.Hp--;
                    if (tank.Hp == 0)
                    {
                        Controller.Effects.Add(new Blast(Controller, tank.GetRectangle()));
                        Controller.Enemies.Remove(tank);
                        if(Controller.Enemies.Count == 0)
                        {
                            MessageBox.Show("你赢了~");
                            Controller.Initialize();
                            return;
                        }
                    }
                    else
                        Music.Hit.Play();
                    Controller.Bullets.Remove(this);
                }
            }
            else if(Controller.IsCollidePlayer(ref Rect))
            {
                Controller.Player.Hp--;
                if (Controller.Player.Hp == 0)
                {
                    MessageBox.Show("完蛋了~");
                    Controller.Initialize();
                    return;
                }
                else
                {
                    Controller.Effects.Add(new Blast(Controller, Controller.Player.GetRectangle()));
                    Music.Hit.Play();
                }
                Controller.Bullets.Remove(this);
            }

            base.Render();
        }
    }
}
