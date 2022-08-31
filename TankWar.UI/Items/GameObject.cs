namespace TankWar.UI.Items
{
    public abstract class GameObject
    {
        protected GameObject(Graphics g)
        {
            G = g;
        }

        public GameObject(Graphics g, Image img, int x, int y, int width, int height) : this(g, img, new Rectangle(x, y, width, height)) { }

        public GameObject(Graphics g, Image img, Rectangle rectangle) : this(g)
        {
            Img = img;
            Rect = rectangle;
        }

        protected Graphics G { get; }

        public virtual Image Img { get; protected set; }

        protected Rectangle Rect;

        public ref Rectangle GetRectangle()
        {
            return ref Rect;
        }

        public virtual void Render()
        {
            G.DrawImage(Img, Rect);
        }
    }
}
