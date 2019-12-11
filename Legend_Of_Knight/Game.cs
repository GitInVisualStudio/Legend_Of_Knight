using Legend_Of_Knight.Entities;
using Legend_Of_Knight.Entities.Enemies;
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
using Rectangle = Legend_Of_Knight.Utils.Math.Rectangle;

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
        public static Vector SIZE => new Vector(WIDTH, HEIGHT);
        public const string NAME = "Legend of Knight";
        public const bool DEBUG = true;

        private int fps = 0;
        private int currentFrames = 0;
        private bool isIngame;
        private Timer renderTimer, tickTimer;
        private Stopwatch watch;
        private InputManager inputManager;
        private AnimationHandler animationHandler;
        public EntityPlayer thePlayer;
        private CustomAnimation<float> zoom;
        private GuiScreen currentScreen;
        private GuiIngame ingameGui;

        private Dungeon d;
        private List<Entity> entities;

        private static EntityPlayer player;
        public InputManager InputManager => inputManager;

        public static EntityPlayer Player { get => player; }

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
            isIngame = false;
            SetScreen(new GuiStartScreen());
            //FormBorderStyle = FormBorderStyle.None; //TODO: Später Header selbst schreiben
            entities = new List<Entity>();
        }

        public void LoadIngame()
        {
            d = new Dungeon(new DungeonGenArgs()
            {
                CorridorWidth = 6,
            });
            foreach (Rectangle r in d.Bounds)
            {
                r.Pos *= 15;
                r.Size *= 15;
                r.CenterPos *= 15;
            }
            thePlayer = new EntityPlayer(d.Bounds)
            {
                Position = (new CRandom(d.Args.Seed).PickElements(d.Rooms, 1)[0].CenterPos * 15).Copy()
            };
            Console.WriteLine("Seed: " + d.Args.Seed);
            thePlayer = new EntityPlayer(d.Bounds);
            thePlayer.Position = new CRandom(d.Args.Seed).PickElements(d.Rooms, 1)[0].CenterPos * 16;
            player = thePlayer;

            EnemyJens enem = new EnemyJens(d.Bounds);
            enem.Position = thePlayer.Position + new Vector(20, 20);
            entities.Add(enem);
            entities.Add(thePlayer);
            ingameGui = new GuiIngame(this);
            isIngame = true;
        }

        private void AddKeybinds()
        {
            inputManager.Add('W', () =>
            {
                if (currentScreen == null)
                    thePlayer?.AddVelocity(new Vector(0, -1) * 1f); // Problem mit In-Bounds-bleiben -> Velocity hinzufügen statt setzen?
            });
            inputManager.Add('A', () =>
            {
                if (currentScreen == null)
                    thePlayer?.AddVelocity(new Vector(-1, 0) * 1f);
            });
            inputManager.Add('S', () =>
            {
                if (currentScreen == null)
                    thePlayer?.AddVelocity(new Vector(0, 1) * 1f);
            });
            inputManager.Add('D', () =>
            {
                if(currentScreen == null)
                    thePlayer?.AddVelocity(new Vector(1, 0) * 1f);
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
            zoom.End += e.Delta / 120 / 2f;
            if (zoom.End < 0.5f)
                zoom.End = 0.5f;
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
            currentScreen?.Click(new MouseEventArgs(e.Button, e.Clicks, InputManager.mouseX, InputManager.mouseY, 0));
            if (!isIngame || currentScreen != null)
                return;
            thePlayer.Swing();
            Vector yaw = InputManager.mousePosition - SIZE / 2;
            thePlayer.Yaw = MathUtils.ToDegree((float)Math.Atan2(yaw.Y, yaw.X)) + 90;
            thePlayer.HurtTime = 30;
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
            };
        }
        public void OnRender(float partialTicks)
        {
            animationHandler.OnRender(partialTicks);
            if(isIngame)
                RenderIngame(partialTicks);
            ingameGui?.OnRender(partialTicks);
            currentScreen?.OnRender(partialTicks);
            
        }

        private void RenderIngame(float partialTicks)
        {
            StateManager.Push();
            //Translating the Player to the center
            StateManager.Scale(zoom.Value);
            StateManager.Translate(-MathUtils.Interpolate(thePlayer.PrevPosition, thePlayer.Position, partialTicks));
            StateManager.Translate(WIDTH / 2f, HEIGHT / 2f);
            RenderDungeon();
            entities.ForEach(x => x.OnRender(partialTicks));
            //TODO: Translating back to top-left and scale back to normal
            StateManager.Pop();

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

                StateManager.Push();
                StateManager.SetColor(255, 0, 0);
                //Translating the to the center
                StateManager.Scale(zoom.Value);
                StateManager.Translate(-MathUtils.Interpolate(thePlayer.PrevPosition, thePlayer.Position, partialTicks));
                StateManager.Translate(WIDTH / 2f, HEIGHT / 2f);
                //TODO: Translating back to top-left and scale back to normal
                foreach (Rectangle r in d.Bounds)
                    StateManager.DrawRect(r.Pos, r.Size.X, r.Size.Y, 2);
                StateManager.Pop();

                StateManager.SetColor(0, 0, 0);
                StateManager.DrawString("PartialTIcks: " + partialTicks, 0, 0);
                StateManager.DrawString("FPS: " + fps, 0, StateManager.GetStringHeight("PartialTicsk"));
                StateManager.Pop();

            }
            #endregion
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
                        //30 weil wegen der Interpolation die Anzeige hinterherhängt und damit vielleicht tiles im screen doch nicht gerendert werden
                        if (xx < thePlayer.X - width / 2 - 30 || xx > thePlayer.X + width / 2 || yy < thePlayer.Y - height / 2 - 30 || yy > thePlayer.Y + height / 2)
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
            entities.ForEach(x => x.OnTick());
        }
    }
}
