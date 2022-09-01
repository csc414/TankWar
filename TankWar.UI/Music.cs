using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace TankWar.UI
{
    public class Music
    {
        private readonly WaveOutEvent _waveOut = new WaveOutEvent();

        private readonly WaveFileReader _reader;

        public Music(Stream stream)
        {
            _reader = new WaveFileReader(stream);
            _waveOut.Init(_reader);
        }

        public void Play()
        {
            _reader.Position = 0;
            _waveOut.Play();
        }

        public static Music Add = new Music(Resources.add);

        public static Music Start = new Music(Resources.start);

        public static SoundPlayer Fire = new SoundPlayer(Resources.fire);

        public static Music Hit = new Music(Resources.hit);

        public static Music Blast = new Music(Resources.blast);
    }
}
