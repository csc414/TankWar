namespace TankWar.UI.Items
{
    public abstract class GameObject
    {
        protected GameObject(GameController controller)
        {
            Controller = controller;
        }

        public GameObject(GameController controller, Image img, int x, int y, int width, int height) : this(controller, img, new Rectangle(x, y, width, height)) { }

        public GameObject(GameController controller, Image img, Rectangle rectangle) : this(controller)
        {
            Img = img;
            Rect = rectangle;
        }

        protected GameController Controller { get; }

        public virtual Image Img { get; protected set; }

        protected Rectangle Rect;

        public ref Rectangle GetRectangle()
        {
            return ref Rect;
        }

        public virtual void Render()
        {
            Controller.G.DrawImage(Img, Rect);
        }
    }
}
