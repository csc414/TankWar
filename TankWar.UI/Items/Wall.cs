namespace TankWar.UI.Items
{
    /// <summary>
    /// 砖头
    /// </summary>
    public class Wall : GameObject
    {
        public Wall(GameController controller, int x, int y) : base(controller, Resources.wall, x, y, 15, 15)
        {
        }
    }
}
