using Legend_Of_Knight.Entities;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Math.Triangulation;
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
        public const float FPS = 240.0f, TPS = 0.5f; 
        public const int WIDTH = 1280, HEIGHT = 720;
        public const string NAME = "Legend of Knight";
        private Timer renderTimer, tickTimer;
        private Stopwatch watch;
        private InputManager inputManager;
        private AnimationHandler animationHandler;
        private EntityPlayer thePlayer;

        private DelaunayTriangulation triang;
        private MinimumSpanningTree mst;
        private bool mstDisplayed;

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

            //thePlayer = new EntityPlayer();

            StateManager.Color(0, 0, 0);

            CRandom rnd = new CRandom(22102016);
            Vector[] points = new Vector[10];
            for (int i = 0; i < points.Length; i++)
                points[i] = new Vector(rnd.NextFloat() * 200, rnd.NextFloat() * 100);
            triang = new DelaunayTriangulation(new Vector(200, 100), points);
            mst = new MinimumSpanningTree(triang);
            mstDisplayed = false;
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
            OnRender(partialTicks);
        }

        public void OnRender(float partialTicks)
        {
            StateManager.Scale(4);
            //thePlayer.OnRender(partialTicks);
            foreach (Vector point in triang.Points)
                StateManager.DrawRect(point.X - 1, point.Y - 1, 2, 2, 2);

            foreach (Edge edge in mstDisplayed ? triang.Edges : mst.Edges)
                StateManager.DrawLine(edge.A, edge.B, 1);
        }

        public void onTick()
        {
            animationHandler.Update();
            inputManager.Update();

            //thePlayer.OnTick();

            if (!mstDisplayed)
            {
                CRandom rnd = new CRandom(22102016);
                Vector[] points = new Vector[10];
                for (int i = 0; i < points.Length; i++)
                    points[i] = new Vector(rnd.NextFloat() * 200, rnd.NextFloat() * 100);
                triang = new DelaunayTriangulation(new Vector(200, 100), points);
                mst = new MinimumSpanningTree(triang);
            }
            mstDisplayed = !mstDisplayed;
        }
    }
}
