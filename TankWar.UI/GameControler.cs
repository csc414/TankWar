using System;
using System.ComponentModel;
using System.Numerics;
using TankWar.UI.Items;

namespace TankWar.UI
{
    public class GameControler
    {
        private readonly Bitmap _canvas;

        private readonly Graphics _g;

        private Tank _player;

        private HashSet<Keys> _playerKeys = new HashSet<Keys>();

        private readonly List<GameObject> _walls = new List<GameObject>();

        private readonly List<Point> _bornPoints = new List<Point>
        {
            new Point(0 * 15, 0 * 15),
            new Point(19 * 15, 0 * 15),
            new Point(38 * 15, 0 * 15)
        };

        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        private readonly List<Tank> _enemies = new List<Tank>();

        public GameControler(int width, int height)
        {
            _canvas = new Bitmap(width, height);
            _g = Graphics.FromImage(_canvas);
        }

        public Bitmap Canvas => _canvas;

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
                            _walls.Add(new Wall(_g, x, y));
                            break;
                        case '2':
                            _walls.Add(new Steel(_g, x, y));
                            break;
                        case '3':
                            _walls.Add(new Boss(_g, x, y));
                            break;
                    }
                }
            }
        }

        public void CreateTanks()
        {
            _player = new Tank(_g, Resources.MyTankUp, Resources.MyTankDown, Resources.MyTankLeft, Resources.MyTankRight, TankDirection.Up, 2, 16 * 15, 38 * 15);
            _player.OnMoveCheck += OnMoveCheck;

            for (int i = 0; i < 50; i++)
                CreateEnemy();
        }

        public void CreateEnemy()
        {
            var point = _bornPoints[_random.Next(0, _bornPoints.Count)];
            var tankType = _random.Next(0, 4);
            Tank tank;
            switch (tankType)
            {
                case 0:
                    tank = new Tank(_g, Resources.GrayUp, Resources.GrayDown, Resources.GrayLeft, Resources.GrayRight, TankDirection.Down, 2, point.X, point.Y);
                    break;
                case 1:
                    tank = new Tank(_g, Resources.GreenUp, Resources.GreenDown, Resources.GreenLeft, Resources.GreenRight, TankDirection.Down, 2, point.X, point.Y);
                    break;
                case 2:
                    tank = new Tank(_g, Resources.QuickUp, Resources.QuickDown, Resources.QuickLeft, Resources.QuickRight, TankDirection.Down, 3, point.X, point.Y);
                    break;
                case 3:
                    tank = new Tank(_g, Resources.SlowUp, Resources.SlowDown, Resources.SlowLeft, Resources.SlowRight, TankDirection.Down, 1, point.X, point.Y);
                    break;
                default:
                    throw new Exception("overflow");
            }
            tank.Moving = true;
            tank.OnMoveCheck += OnEnemyMoveCheck;
            _enemies.Add(tank);
        }

        private int OnEnemyMoveCheck(Tank tank, TankDirection direction)
        {
            var i = OnMoveCheck(tank, direction);
            if (i == -1)
            {
                TankDirection randomDirection;
                do
                {
                    randomDirection = (TankDirection)_random.Next(1, 5);
                }
                while (randomDirection == direction);
                tank.SetDirection(randomDirection);
            }
            return i;
        }

        private int OnMoveCheck(Tank tank, TankDirection direction)
        {
            var rect = tank.GetRectangle();
            var speed = tank.Speed;
            switch (direction)
            {
                case TankDirection.Up:
                    {
                        rect.Y -= speed;
                        if (IsCollide(ref rect, tank) || rect.Y <= 0)
                            return -1;

                        return rect.Y;
                    }
                case TankDirection.Down:
                    {
                        rect.Y += speed;
                        if (IsCollide(ref rect, tank) || rect.Y + tank.Img.Height >= _canvas.Height)
                            return -1;

                        return rect.Y;
                    }
                case TankDirection.Left:
                    {
                        rect.X -= speed;
                        if (IsCollide(ref rect, tank) || rect.X <= 0)
                            return -1;

                        return rect.X;
                    }
                case TankDirection.Right:
                    {
                        rect.X += speed;
                        if (IsCollide(ref rect, tank) || rect.X + tank.Img.Width >= _canvas.Width)
                            return -1;

                        return rect.X;
                    }
            }

            return 0;
        }

        bool IsCollide(ref Rectangle rect, GameObject except)
        {
            foreach (var wall in _walls)
            {
                ref var wallRect = ref wall.GetRectangle();
                if (rect.IntersectsWith(wallRect))
                    return true;
            }

            if(except == _player)
            {
                foreach (var enemy in _enemies)
                {
                    if (except == enemy)
                        continue;

                    ref var enemyRect = ref enemy.GetRectangle();
                    if (rect.IntersectsWith(enemyRect))
                        return true;
                }
            }
            else
            {
                if (except != _player)
                {
                    ref var playerRect = ref _player.GetRectangle();
                    if (rect.IntersectsWith(playerRect))
                        return true;
                }
            }

            return false;
        }

        public void Initialize()
        {
            CreateMap();
            CreateTanks();
        }

        public void Render()
        {
            _g.Clear(Color.Black);
            //砖头墙 渲染
            for (int i = 0; i < _walls.Count; i++)
                _walls[i].Render();

            //敌人 渲染
            for (int i = 0; i < _enemies.Count; i++)
                _enemies[i].Render();

            //玩家 渲染
            _player.Render();
        }

        void Move(Tank tank, Keys key)
        {
            switch (key)
            {
                case Keys.W:
                    tank.SetDirection(TankDirection.Up);
                    break;
                case Keys.S:
                    tank.SetDirection(TankDirection.Down);
                    break;
                case Keys.A:
                    tank.SetDirection(TankDirection.Left);
                    break;
                case Keys.D:
                    tank.SetDirection(TankDirection.Right);
                    break;
            }

            tank.Moving = true;
        }

        public void KeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    _playerKeys.Add(Keys.W);
                    Move(_player, e.KeyCode);
                    break;
                case Keys.S:
                    _playerKeys.Add(Keys.S);
                    Move(_player, e.KeyCode);
                    break;
                case Keys.A:
                    _playerKeys.Add(Keys.A);
                    Move(_player, e.KeyCode);
                    break;
                case Keys.D:
                    _playerKeys.Add(Keys.D);
                    Move(_player, e.KeyCode);
                    break;
            }
        }

        public void KeyUp(KeyEventArgs e)
        {
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
            }

            if (_playerKeys.Count > 0)
                Move(_player, _playerKeys.Last());
            else
                _player.Moving = false;
        }
    }
}
