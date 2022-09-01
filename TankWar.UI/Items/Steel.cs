namespace TankWar.UI.Items
{
    /// <summary>
    /// 砖头
    /// </summary>
    public class Steel : GameObject
    {
        public Steel(GameController controller, int x, int y) : base(controller, Resources.steel, x, y, 15, 15)
        {
        }
    }
}
