namespace TankWar.UI
{
    public partial class MainForm : Form
    {
        private readonly CancellationTokenSource _tokenSource;

        public MainForm()
        {
            InitializeComponent();
            Width = Width + (Width - ClientRectangle.Width);
            Height = Height + (Height - ClientRectangle.Height);
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(Render, CreateGraphics(), _tokenSource.Token);
        }

        public void Render(object state)
        {
            var controller = new GameControler(ClientRectangle.Width, ClientRectangle.Height);
            KeyDown += (sender, arg) => controller.KeyDown(arg);
            KeyUp += (sender, arg) => controller.KeyUp(arg);
            controller.Initialize();

            var g = (Graphics)state!;
            var delay = 1000 / 60;
            while(!_tokenSource.IsCancellationRequested)
            {
                var start = DateTimeOffset.Now;
                controller.Render();
                g.DrawImage(controller.Canvas, 0, 0);
                var d = delay - (int)(DateTimeOffset.Now - start).TotalMilliseconds;
                if(d > 0)
                    Thread.Sleep(d);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _tokenSource.Cancel();
        }
    }
}