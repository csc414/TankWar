namespace TankWar.UI.Items
{
    /// <summary>
    /// 砖头
    /// </summary>
    public class Wall : GameObject
    {
        public Wall(Graphics g, int x, int y) : base(g, Resources.wall, x, y, 15, 15)
        {
        }
    }
}
