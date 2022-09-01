namespace TankWar.UI.Items
{
    public class Boss : GameObject
    {
        public Boss(GameController controller, int x, int y) : base(controller, Resources.Boss, x, y, 30, 30)
        {
        }
    }
}
