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
using Legend_Of_Knight.World;

namespace Legend_Of_Knight
{
    public class Game : Form
    {
        /// <summary>
        /// frames per Second and ticks per Second
        /// </summary>
        public const float FPS = 240.0f, TPS = 0.05f; 
        public const int WIDTH = 1280, HEIGHT = 720;
        public const string NAME = "Legend of Knight";
        private Timer renderTimer, tickTimer;
        private Stopwatch watch;
        private InputManager inputManager;
        private AnimationHandler animationHandler;
        private EntityPlayer thePlayer;
        private Dungeon d;

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
            d = new Dungeon(new DungeonGenArgs()
            {
                Size = new Vector(Width / 2, Height / 2),
                Rooms = 50,
                RoomSize = new Vector(40, 40)
            });   
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Game
            // 
            this.ClientSize = new System.Drawing.Size(242, 252);
            this.Name = "Game";
            this.ResumeLayout(false);

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
            //thePlayer.OnRender(partialTicks);
            StateManager.Scale(2);
            for (int x = 0; x < d.Fields.GetLength(0); x++)
            {
                for (int y = 0; y < d.Fields.GetLength(1); y++)
                {
                    if (d.Fields[x, y].Area is Corridor)
                        StateManager.Color(255, 0, 0);
                    else if (d.Fields[x, y].Area is Room)
                        StateManager.Color(0, 0, 255);
                    else
                        StateManager.Color(255, 255, 255);
                    StateManager.DrawRect(x, y, 1, 1);
                }
            }
            //StateManager.Color(0, 255, 0);
            //foreach (Edge e in d.Mst.Edges)
            //    StateManager.DrawLine(e.A, e.B);
            StateManager.Color(0, 0, 0);
            //foreach (Room r in d.Rooms)
            //    StateManager.DrawRect(r.CenterPos.X, r.CenterPos.Y, 1, 1, 1);
        }

        public void onTick()
        {
            animationHandler.Update();
            inputManager.Update();

            //thePlayer.OnTick();
        }
    }
}
