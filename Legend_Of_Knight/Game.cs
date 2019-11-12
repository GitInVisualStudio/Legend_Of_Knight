using Legend_Of_Knight.Entities;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Render;
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
        /// frames per Second and ticks per Second
        /// </summary>
        public const float FPS = 240.0f, TPS = 30.0f; 
        public const int WIDTH = 1280, HEIGHT = 720;
        public const string NAME = "Legend of Knight";
        private Timer renderTimer, tickTimer;
        private Stopwatch watch;
        private InputManager inputManager;
        private AnimationHandler animationHandler;
        private EntityPlayer thePlayer;

        public InputManager InputManager => inputManager;

        public Game()
        {
            Init();
        }

        private void Init()
        {
            Text = NAME;
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

            MouseClick += Game_MouseClick;
            MouseMove += Game_MouseMove;
            KeyDown += Game_KeyDown;
            KeyUp += Game_KeyUp;

            animationHandler = new AnimationHandler();
            inputManager = new InputManager();
            AddKeybinds();
            renderTimer.Start();
            tickTimer.Start();
            //FormBorderStyle = FormBorderStyle.None; //TODO: Später Header selbst schreiben

            thePlayer = new EntityPlayer();
        }

        private void AddKeybinds()
        {
            inputManager.Add('W', () =>
            {
                thePlayer.SetVelocity(thePlayer.Velocity.X, -1);
            });
            inputManager.Add('A', () =>
            {
                thePlayer.SetVelocity(-1, thePlayer.Velocity.Y);
            });
            inputManager.Add('S', () =>
            {
                thePlayer.SetVelocity(thePlayer.Velocity.X, 1);
            });
            inputManager.Add('D', () =>
            {
                thePlayer.SetVelocity(1, thePlayer.Velocity.Y);
            });

        }

        private void Game_MouseMove(object sender, MouseEventArgs e)
        {
            InputManager.mouseX = e.X;
            InputManager.mouseY = e.Y;
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            inputManager.OnKeyRelease(e.KeyValue);
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            inputManager.OnKeyPressed(e.KeyValue);
        }

        private void Game_MouseClick(object sender, MouseEventArgs e)
        {
            //TODO: Handle events in GuiScreen & PlayerInteraction -> PlayerController?
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
            float partialTicks = (float)((1000.0f / TPS) - watch.Elapsed.TotalMilliseconds) / (1000.0f / TPS);
            StateManager.Update(e.Graphics);
            onRender(partialTicks);
        }

        public void onRender(float partialTicks)
        {
            thePlayer.OnRender(partialTicks);
        }

        public void onTick()
        {
            animationHandler.Update();
            inputManager.Update();
            thePlayer.OnTick();
        }
    }
}
