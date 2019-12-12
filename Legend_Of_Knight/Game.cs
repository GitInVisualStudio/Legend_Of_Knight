using Legend_Of_Knight.Entities;
using Legend_Of_Knight.Entities.Enemies;
using Legend_Of_Knight.Entities.Items;
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
        // TODO: Kommentare
        // Entity teilweise
        // EntityItem
        // EntityLivingBase teilweise
        // der gesamte GUI-Namespace lol
        // alles in Utils.Animations
        // TimeUtils
        // alles in Utils.Render

        /// <summary>
        /// frames per Second, ticks per Second and time per tick
        /// </summary>
        public const float FPS = 120.0f, TPS = 30.0f, TPT = (1000.0f / TPS);
        private static int A_WIDTH = 1280, A_HEIGHT = 720; //Absolut
        public static float WIDTH => (A_WIDTH * 1f / StateManager.ScaleX); //Relativ
        public static float HEIGHT => (A_HEIGHT * 1f / StateManager.ScaleY);
        public static Vector SIZE => new Vector(WIDTH, HEIGHT);
        public const string NAME = "Legend of Knight";

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
        private static List<Entity> entities;
        private CRandom rnd;
        private Type[] enemyTypes;

        private static EntityPlayer player;
        public InputManager InputManager => inputManager;

        public static EntityPlayer Player { get => player; }
        public static List<Entity> Entities { get => entities; }

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
                A_WIDTH = Width;
                A_HEIGHT = Height;
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
            enemyTypes = new Type[] { typeof(EnemyJens) };
        }

        public void LoadIngame()
        {
            d = new Dungeon(new DungeonGenArgs()
            {
                CorridorWidth = 6,
                Rooms = 10,
                LeaveConnectionPercentage = 0.25f,
                EnemiesPerRoom = 3
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
            rnd = new CRandom(d.Args.Seed);
            thePlayer = new EntityPlayer(d.Bounds);
            thePlayer.Position = rnd.PickElements(d.Rooms, 1)[0].CenterPos * 16;
            player = thePlayer;
            Entities.Add(thePlayer);

            SpawnEnemies();
            
            ingameGui = new GuiIngame(this);
            isIngame = true;
        }

        /// <summary>
        /// Erstellt anhand der in den DungeonGenArgs angegebenen Zahl pro Raum Gegner und platziert sie zufällig
        /// </summary>
        private void SpawnEnemies()
        {
            foreach (Room r in d.Rooms)
            {
                int enemyCount = (int)Math.Round((rnd.NextFloatGaussian(0.5f) + 0.5) * d.Args.EnemiesPerRoom);
                for (int i = 0; i < enemyCount; i++)
                {
                    EntityEnemy enem = (EntityEnemy)rnd.PickElements(enemyTypes, 1)[0].GetConstructors()[0].Invoke(new object[] { d.Bounds }); // sucht einen Gegnertyp zufällig aus und erstellt ein Objekt dessen
                    enem.Position = (new Vector(r.X, r.Y) + new Vector(r.SizeX * rnd.NextFloatGaussian(), r.SizeY * rnd.NextFloatGaussian()) / 2) * 16;
                    entities.Add(enem);
                }
            }
        }

        private void AddKeybinds()
        {
            inputManager.Add('W', () =>
            {
                if (currentScreen == null)
                    thePlayer?.AddVelocity(new Vector(0, -1) * 1f);
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
            if (!isIngame || currentScreen != null || thePlayer.IsDead)
                return;
            thePlayer.Swing();
            Vector yaw = InputManager.mousePosition - SIZE / 2;
            thePlayer.Yaw = MathUtils.ToDegree((float)Math.Atan2(yaw.Y, yaw.X)) + 90;
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
            e.Graphics.Clear(Color.FromArgb(20, 3, 7));
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
            screen.Init(this);// wirft alle jubeljahre eine null reference exception ?????
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
            StateManager.Scale(zoom.Value);
            Vector player = -MathUtils.Interpolate(thePlayer.PrevPosition, thePlayer.Position, partialTicks);
            StateManager.Translate(player);
            StateManager.Translate(WIDTH / 2f, HEIGHT / 2f);
            float shake = MathUtils.Sin(thePlayer.HurtTime / (float)thePlayer.MaxHurtTime * 360 * 2) * 5;
            StateManager.Translate(shake, -shake);
            RenderDungeon();
            StateManager.Pop();
            for (int k = 0; k < entities.Count; k++)
            {
                StateManager.Push();
                StateManager.Scale(zoom.Value);
                StateManager.Translate(player);
                StateManager.Translate(WIDTH / 2f, HEIGHT / 2f);
                StateManager.Translate(shake, -shake);
                entities[k].OnRender(partialTicks);
                StateManager.Pop();
            }
        }

        /// <summary>
        /// Gibt alle Items der Gegner einer bestimmten Entity zurück.
        /// </summary>
        /// <param name="friendly">Falls true, wird nach den Entities auf der Seite des Protagonisten gesucht und ihre Waffen zurück. Falls nein werden die Items der Antagonisten zurückgegeben.</param>
        public static List<EntityItem> GetEnemyItems(bool friendly)
        {
            List<EntityItem> res = new List<EntityItem>();
            if (friendly)
                res.Add(Player.EntityItem);
            else
                for (int i = 0; i < entities.Count; i++)
                {
                    Entity e = entities[i];
                    if (e is EntityLivingBase && !(e is EntityPlayer))
                        res.Add(((EntityLivingBase)e).EntityItem);
                }
            return res;
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
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].OnTick();
                if (((EntityLivingBase)Entities[i]).IsDead)
                    Entities.RemoveAt(i);
            }
            if (entities.Count == 1)
            {
                isIngame = false;
                SetScreen(new GuiStartScreen());
                entities.Clear();
                ingameGui = null;
            }
            else if (isIngame && thePlayer.IsDead)
            {
                isIngame = false;
                SetScreen(new GuiDeathScreen());
                entities.Clear();
                ingameGui = null;
            }

        }
    }
}
