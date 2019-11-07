using Legend_Of_Knight.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend_Of_Knight
{
    public class Game : Form
    {
        /// <summary>
        /// Frames per Second and Ticks per Second
        /// </summary>
        public const float FPS = 240.0f, TPS = 30.0f; 
        public const int WIDTH = 1280, HEIGHT = 720;
        public const string NAME = "Legend of Knight";
        private Timer renderTimer, tickTimer;
        private Stopwatch watch;

        public Game()
        {
            Init();
        }

        private void Init()
        {
            Name = NAME;
            Width = WIDTH;
            Height = HEIGHT;
            DoubleBuffered = true; //Verhindert Flackern

            renderTimer = new Timer()
            {
                Interval = (int)(1000.0f / FPS)
            };
            renderTimer.Tick += RenderTimer_Tick;

            tickTimer = new Timer() {
                Interval = (int)(1000.0f / TPS),
            };
            tickTimer.Tick += TickTimer_Tick;

            watch = new Stopwatch();
            watch.Start();

            renderTimer.Start();
            tickTimer.Start();
            //FormBorderStyle = FormBorderStyle.None; //TODO: Später Header selbst schreiben
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            //TODO: Call Tick, reset the StopWatch
            watch.Reset();
            watch.Start();
            onTick();
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //TODO: calculate the partialTicks, set new Graphics instance
            float partialTicks = (float)((1000.0f / TPS) - watch.Elapsed.TotalMilliseconds);
            Console.WriteLine(partialTicks);
            StateManager.Update(e.Graphics);
            onRender(partialTicks);
        }

        public void onRender(float partialTicks)
        {
        }

        public void onTick()
        {

        }
    }
}
