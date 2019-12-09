using Legend_Of_Knight.Entities;
using Legend_Of_Knight.Gui;
using Legend_Of_Knight.Gui.GuiScreens;
using Legend_Of_Knight.Properties;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;
using Legend_Of_Knight.World;
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
        private const int A_WIDTH = 1280, A_HEIGHT = 720; //Absolut
        public static float WIDTH => (A_WIDTH * 1f / StateManager.ScaleX); //Relativ
        public static float HEIGHT => (A_HEIGHT * 1f / StateManager.ScaleY);
        public const string NAME = "Legend of Knight";
        public const bool DEBUG = false;

        private int fps = 0;
        private int currentFrames = 0;
        private Timer renderTimer, tickTimer;
        private Stopwatch watch;
        private InputManager inputManager;
        private AnimationHandler animationHandler;
        private EntityPlayer thePlayer;
        private CustomAnimation<float> zoom;
        private GuiScreen currentScreen;

        private Dungeon d;
        public InputManager InputManager => inputManager;

        public Game()
        {
            Init();
        }

        private void Init()
        {
            Text = NAME;
            Width = (int)WIDTH;
            Height = (int)HEIGHT;
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
            Resize += (object sender, EventArgs e) =>
            {
                currentScreen?.Resize();
            };

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

            d = new Dungeon(new DungeonGenArgs()
            {
                CorridorWidth = 6
            });
            thePlayer = new EntityPlayer(d.Bounds);
            thePlayer.Position = new CRandom(d.Args.Seed).PickElements(d.Rooms, 1)[0].CenterPos * 16;
        }

        private void AddKeybinds()
        {
            inputManager.Add('W', () =>
            {
                if (currentScreen == null)
                    thePlayer.AddVelocity(new Vector(0, -1) * 1f); // Problem mit In-Bounds-bleiben -> Velocity hinzufügen statt setzen?
            });
            inputManager.Add('A', () =>
            {
                if (currentScreen == null)
                    thePlayer.AddVelocity(new Vector(-1, 0) * 1f);
            });
            inputManager.Add('S', () =>
            {
                if (currentScreen == null)
                    thePlayer.AddVelocity(new Vector(0, 1) * 1f);
            });
            inputManager.Add('D', () =>
            {
                if(currentScreen == null)
                    thePlayer.AddVelocity(new Vector(1, 0) * 1f);
            });
            inputManager.Add(27, () =>
            {
                if (currentScreen == null)
                    SetScreen(new GuiOptions());
            }, fireOnce: true); //Sonst wird der Screen solange gesetzt bis der key released wird
        }

        #region events
        private void Game_MouseEvent(object sender, MouseEventArgs e)
        {
            InputManager.mouseX = (int)(e.X / StateManager.ScaleX);
            InputManager.mouseY = (int)(e.Y / StateManager.ScaleY);
            InputManager.mousePosition = new Vector(InputManager.mouseX, InputManager.mouseY);
            if (currentScreen != null)
            {
                currentScreen.Move(new MouseEventArgs(e.Button, e.Clicks, InputManager.mouseX, InputManager.mouseY, 0));
                return;
            }
            zoom.End += e.Delta / 120;
            if (zoom.Finished)
                zoom.Fire();
        }

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            inputManager.OnKeyRelease(e.KeyValue);
        }

        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            currentScreen?.KeyPressed(e);
            inputManager.OnKeyPressed(e.KeyValue);
        }

        private void Game_MouseClick(object sender, MouseEventArgs e)
        {
            //TODO: Handle events in GuiScreen & PlayerInteraction -> PlayerController?
            thePlayer.Swing();
            Vector yaw = InputManager.mousePosition - thePlayer.Position;
            thePlayer.Yaw = MathUtils.ToDegree((float)Math.Atan2(yaw.Y, yaw.X)) + 90;
            currentScreen?.Click(new MouseEventArgs(e.Button, e.Clicks, InputManager.mouseX, InputManager.mouseY, 0));
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
        #endregion

        public void SetScreen(GuiScreen screen)
        {
            if(screen == null && currentScreen != null)
            {
                currentScreen.Close();
                currentScreen.Animation.OnFinish += (object sender, EventArgs args) =>
                {
                    currentScreen = null;
                }; //CheckBox, Label, Textbox, Slider
                return;
            }
            screen.Init(this);
            if (currentScreen == null)
            {
                currentScreen = screen.Open(currentScreen);
                return;
            }
            currentScreen.Close();
            currentScreen.Animation.OnFinish += (object sender, EventArgs args) =>
            {
                currentScreen = screen.Open(currentScreen);
            }; //CheckBox, Label, Textbox, Slider
        }
        public void OnRender(float partialTicks)
        {
            animationHandler.OnRender(partialTicks);
            StateManager.Push();
            //Translating the Player to the center
            StateManager.Scale(zoom.Value);
            StateManager.Translate(-thePlayer.Position);
            StateManager.Translate(WIDTH / 2f, HEIGHT / 2f);
            RenderDungeon();
            currentScreen?.OnRender(partialTicks);
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
                //StateManager.Scale(0.5f);
                StateManager.SetColor(255, 0, 0);
                foreach (Utils.Math.Rectangle r in d.Bounds)
                    StateManager.DrawRect(r.Pos * 16, r.Size.X * 16, r.Size.Y * 16, 5);
                StateManager.SetColor(0, 0, 0);
                StateManager.DrawString("PartialTIcks: " + partialTicks, 0, 0);
                StateManager.DrawString("FPS: " + fps, 0, StateManager.GetStringHeight("PartialTicsk"));
                StateManager.Pop();
                
            }
            #endregion
            thePlayer.OnRender(partialTicks);
        }

        public void RenderDungeon()
        {
            float width = WIDTH;
            float height = HEIGHT;
            for (int x = 0; x < d.Fields.GetLength(0); x++)
            { 
                for (int y = 0; y < d.Fields.GetLength(1); y++)
                {
                    if (d.Fields[x, y].Anim != null)
                    {
                        int xx = x * 15, yy = y * 15;
                        if (xx < thePlayer.X - width / 2 - 15 || xx > thePlayer.X + width / 2 || yy < thePlayer.Y - height / 2 - 15 || yy > thePlayer.Y + height / 2)
                            continue;
                        StateManager.DrawImage(d.Fields[x, y].Anim.Image, xx, yy);
                    }
                }
            }
        }

        public void OnTick()
        {
            animationHandler.Update();
            inputManager.Update();
            thePlayer.OnTick();
        }
    }
}
