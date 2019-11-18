using Legend_Of_Knight.Entities;
using Legend_Of_Knight.Properties;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend_Of_Knight
{
    public class Game : Form
    {
        /// <summary>
        /// frames per Second, ticks per Second and time per tick
        /// </summary>
        public const float FPS = 120.0f, TPS = 30.0f, TPT = (1000.0f / TPS); 
        public const int WIDTH = 1280, HEIGHT = 720;
        public const string NAME = "Legend of Knight";
        public const bool DEBUG = true;

        private int fps = 0;
        private int currentFrames = 0;
        private Timer renderTimer, tickTimer;
        private Stopwatch watch;
        private InputManager inputManager;
        private AnimationHandler animationHandler;
        private EntityPlayer thePlayer;
        private CustomAnimation<float> zoom;
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
                Interval = (int)TPT,
            };
            tickTimer.Tick += TickTimer_Tick;

            watch = new Stopwatch();
            watch.Start();

            MouseClick += Game_MouseClick;
            MouseMove += Game_MouseEvent;
            KeyDown += Game_KeyDown;
            KeyUp += Game_KeyUp;
            MouseWheel += Game_MouseEvent;

            animationHandler = new AnimationHandler();
            zoom = new CustomAnimation<float>(5, 5, (float current, float delta) =>
            {
                return current + delta;
            });
            zoom.Fire();
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
                thePlayer.SetVelocity(thePlayer.Velocity.X, thePlayer.Velocity.Y - 1);
            });
            inputManager.Add('A', () =>
            {
                thePlayer.SetVelocity(thePlayer.Velocity.X - 1, thePlayer.Velocity.Y);
            });
            inputManager.Add('S', () =>
            {
                thePlayer.SetVelocity(thePlayer.Velocity.X, thePlayer.Velocity.Y + 1);
            });
            inputManager.Add('D', () =>
            {
                thePlayer.SetVelocity(thePlayer.Velocity.X + 1, thePlayer.Velocity.Y);
            });

        }

        private void Game_MouseEvent(object sender, MouseEventArgs e)
        {
            InputManager.mouseX = (int)(e.X / StateManager.ScaleX);
            InputManager.mouseY = (int)(e.Y / StateManager.ScaleY);
            InputManager.mousePosition = new Vector(InputManager.mouseX, InputManager.mouseY);
            zoom.End += e.Delta / 120;
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
            OnTick();
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //TODO: calculate the partialTicks, set new Graphics instance
            float partialTicks = (float)(TPT - watch.Elapsed.TotalMilliseconds) / TPT;
            StateManager.Update(e.Graphics);
            OnRender(partialTicks);
        }

        public void OnRender(float partialTicks)
        {
            animationHandler.OnRender(partialTicks);
            StateManager.Push();
            StateManager.Scale(zoom.Value);
            #region DEBUG
            if (DEBUG)
            {
                currentFrames++;
                if (TimeUtils.Check(1000))
                {
                    fps = currentFrames;
                    currentFrames = 0;
                }
                StateManager.Push();
                StateManager.Scale(1/zoom.Value);
                StateManager.SetColor(0, 0, 0);
                StateManager.DrawString("PartialTIcks: " + partialTicks, 0, 0);
                StateManager.DrawString("FPS: " + fps, 0, StateManager.GetStringHeight("PartialTicsk"));
                StateManager.Pop();
            }
            #endregion
            thePlayer.OnRender(partialTicks);
        }

        public void OnTick()
        {
            animationHandler.Update();
            inputManager.Update();
            thePlayer.OnTick();
        }
    }
}
