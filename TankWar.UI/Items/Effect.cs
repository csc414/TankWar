using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI.Items
{
    public class Effect : GameObject
    {
        public Effect(GameController controller) : base(controller)
        {
        }

        public Bitmap[] Frames { get; init; }

        public int PerFrame { get; init; } = 1;

        public int LoopCount { get; init; } = 1;

        public Action OnFinish { get; protected set; }

        public int _frameIndex = 0;

        public int _frameCount = 0;

        public int _loopCount = 1;

        public override void Render()
        {
            Img = Frames[_frameIndex];
            _frameCount++;
            if (_frameCount % PerFrame == 0)
            {
                var index = _frameIndex + 1;
                if (index >= Frames.Length)
                {
                    if (LoopCount == -1 || _loopCount < LoopCount)
                    {
                        _frameIndex = 0;
                        _frameCount = 0;
                        _loopCount++;
                    }
                    else
                    {
                        OnFinish?.Invoke();
                    }
                }
                else
                    _frameIndex = index;
            }

            base.Render();
        }
    }
}
