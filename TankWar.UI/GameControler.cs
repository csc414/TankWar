using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Numerics;
using TankWar.UI.Items;

namespace TankWar.UI
{
    public class GameController
    {
        private readonly HashSet<Keys> _playerKeys = new HashSet<Keys>();

        private readonly List<Point> _bornPoints = new List<Point>
        {
            new Point(0 * 15, 0),
            new Point(19 * 15, 0),
            new Point(38 * 15, 0)
        };

        public GameController(int width, int height)
        {
            Canvas = new Bitmap(width, height);
            G = Graphics.FromImage(Canvas);
        }

        public int Width => Canvas.Width;

        public int Height => Canvas.Height;

        public Random Rd { get; } = new Random(Guid.NewGuid().GetHashCode());

        public Bitmap Canvas { get; }

        public Graphics G { get; }

        public HashSet<GameObject> Walls { get; private set; } = new HashSet<GameObject>();

        public HashSet<Tank> Enemies { get; private set; } = new HashSet<Tank>();

        public HashSet<Bullet> Bullets { get; private set; } = new HashSet<Bullet>();

        public HashSet<Effect> Effects { get; private set; } = new HashSet<Effect>();

        public Tank Player { get; private set; }


        public void Initialize()
        {
            Walls.Clear();
            Enemies.Clear();
            Bullets.Clear();
            Effects.Clear();
            CreateMap();
            CreateTanks();
            Music.Start.Play();
        }

        public void CreateMap()
        {
            var lines = File.ReadAllLines("map.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++)
                {
                    var x = j * 15;
                    var y = i * 15;
                    switch (line[j])
                    {
                        case '1':
                            Walls.Add(new Wall(this, x, y));
                            break;
                        case '2':
                            Walls.Add(new Steel(this, x, y));
                            break;
                        case '3':
                            Walls.Add(new Boss(this, x, y));
                            break;
                    }
                }
            }
        }

        public void CreateTanks()
        {
            Player = new PlayerTank(this, 5, Resources.MyTankUp, Resources.MyTankDown, Resources.MyTankLeft, Resources.MyTankRight, MoveDirection.Up, 4, 16 * 15, 38 * 15);
            for (int i = 0; i < 100; i++)
                CreateStar();
        }

        public void CreateStar()
        {
            var point = _bornPoints[Rd.Next(0, _bornPoints.Count)];
            Effects.Add(new Star(this, point));
        }

        public void CreateEnemy(Point point)
        {
            var tankType = Rd.Next(0, 4);
            Tank tank;
            switch (tankType)
            {
                case 0:
                    tank = new EnemyTank(this, 2, Resources.GrayUp, Resources.GrayDown, Resources.GrayLeft, Resources.GrayRight, MoveDirection.Down, 2, point.X, point.Y);
                    break;
                case 1:
                    tank = new EnemyTank(this, 4, Resources.GreenUp, Resources.GreenDown, Resources.GreenLeft, Resources.GreenRight, MoveDirection.Down, 2, point.X, point.Y);
                    break;
                case 2:
                    tank = new EnemyTank(this, 1, Resources.QuickUp, Resources.QuickDown, Resources.QuickLeft, Resources.QuickRight, MoveDirection.Down, 3, point.X, point.Y);
                    break;
                case 3:
                    tank = new EnemyTank(this, 1, Resources.SlowUp, Resources.SlowDown, Resources.SlowLeft, Resources.SlowRight, MoveDirection.Down, 1, point.X, point.Y);
                    break;
                default:
                    throw new Exception("overflow");
            }
            Enemies.Add(tank);
        }

        public bool IsCollideWall(ref Rectangle rect, out GameObject wall)
        {
            foreach (var item in Walls)
            {
                ref var wallRect = ref item.GetRectangle();
                if (rect.IntersectsWith(wallRect))
                {
                    wall = item;
                    return true;
                }
            }

            wall = null;
            return false;
        }

        public bool IsCollideEnemy(ref Rectangle rect, out Tank tank)
        {
            foreach (var enemy in Enemies)
            {
                ref var enemyRect = ref enemy.GetRectangle();
                if (rect.IntersectsWith(enemyRect))
                {
                    tank = enemy;
                    return true;
                }
            }

            tank = null;
            return false;
        }

        public bool IsCollidePlayer(ref Rectangle rect)
        {
            ref var playerRect = ref Player.GetRectangle();
            if (rect.IntersectsWith(playerRect))
                return true;

            return false;
        }

        public void Render()
        {
            G.Clear(Color.Black);

            //砖头墙 渲染
            foreach (var wall in Walls)
                wall.Render();

            //敌人 渲染
            foreach (var enemy in Enemies)
                enemy.Render();

            //玩家 渲染
            Player.Render();

            //子弹 渲染
            foreach (var bullet in Bullets)
                bullet.Render();

            //效果 渲染
            foreach (var effect in Effects)
                effect.Render();

            G.DrawString($"HP:{Player?.Hp ?? 0}", new Font("宋体", 20, FontStyle.Bold), new SolidBrush(Color.Red), new PointF(5, 5));
        }

        void Move(Tank tank, Keys key)
        {
            switch (key)
            {
                case Keys.W:
                    tank.SetDirection(MoveDirection.Up);
                    break;
                case Keys.S:
                    tank.SetDirection(MoveDirection.Down);
                    break;
                case Keys.A:
                    tank.SetDirection(MoveDirection.Left);
                    break;
                case Keys.D:
                    tank.SetDirection(MoveDirection.Right);
                    break;
            }

            tank.Moving = true;
        }

        public void KeyDown(KeyEventArgs e)
        {
            if (Player == null)
                return;

            switch (e.KeyCode)
            {
                case Keys.W:
                    _playerKeys.Add(Keys.W);
                    Move(Player, e.KeyCode);
                    break;
                case Keys.S:
                    _playerKeys.Add(Keys.S);
                    Move(Player, e.KeyCode);
                    break;
                case Keys.A:
                    _playerKeys.Add(Keys.A);
                    Move(Player, e.KeyCode);
                    break;
                case Keys.D:
                    _playerKeys.Add(Keys.D);
                    Move(Player, e.KeyCode);
                    break;
                case Keys.J:
                    Debug.Print("Hello");
                    Player.StartShooting();
                    break;
            }
        }

        public void KeyUp(KeyEventArgs e)
        {
            if (Player == null)
                return;

            switch (e.KeyCode)
            {
                case Keys.W:
                    _playerKeys.Remove(Keys.W);
                    break;
                case Keys.S:
                    _playerKeys.Remove(Keys.S);
                    break;
                case Keys.A:
                    _playerKeys.Remove(Keys.A);
                    break;
                case Keys.D:
                    _playerKeys.Remove(Keys.D);
                    break;
                case Keys.J:
                    Player.EndShooting();
                    break;
            }

            if (_playerKeys.Count > 0)
                Move(Player, _playerKeys.Last());
            else
                Player.Moving = false;
        }
    }
}
