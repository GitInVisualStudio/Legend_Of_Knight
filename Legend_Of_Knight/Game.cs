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
        /// <summary>
        /// frames per Second, ticks per Second and time per tick
        /// </summary>
        public const float FPS = 120.0f, TPS = 30.0f, TPT = (1000.0f / TPS);
        private static int A_WIDTH = 1280, A_HEIGHT = 720; //Absolute Dimension der Form
        public static float WIDTH => (A_WIDTH * 1f / StateManager.ScaleX); //Relative Width zur Skalierung, notwendig für die Translation des Spielers in die Mitte
        public static float HEIGHT => (A_HEIGHT * 1f / StateManager.ScaleY);
        public static Vector SIZE => new Vector(WIDTH, HEIGHT);
        public const string NAME = "Legend of Knight";

        private bool isIngame;
        private Timer renderTimer, tickTimer;//Timer für das Berechnen des Spiels
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

        /// <summary>
        /// Initialisiert die Form und die dafür notwendingen Komponenten für unser Framework
        /// </summary>
        private void Init()
        {
            Text = NAME;
            Width = (int)WIDTH;
            Height = (int)HEIGHT;
            DoubleBuffered = true; //Verhindert Flackern

            //Erstellen der Timer für aktualisierung der Form
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

            //Hinzufügen der Events für interaktionen
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
            SetScreen(new GuiStartScreen());//StartScreen setzen
            entities = new List<Entity>();
            enemyTypes = new Type[] { typeof(EnemyJens) };
        }

        /// <summary>
        /// Läd das Ingame-Spiel
        /// </summary>
        public void LoadIngame()
        {
            //Erstellen des Dungeons
            d = new Dungeon(new DungeonGenArgs()
            {
                CorridorWidth = 6,
                Rooms = 10,
                LeaveConnectionPercentage = 0.25f,
                EnemiesPerRoom = 3
            });
            //Resize der Rectangle für Performanz
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

        /// <summary>
        /// Hinzufügen der Events für die Tastatur-Interaktion
        /// Steuert den Spieler
        /// </summary>
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
            inputManager.Add(27/*Escape*/, () =>
            {
                if (currentScreen == null)
                    SetScreen(new GuiOptions());
            }, fireOnce: true); //Sonst wird der Screen solange gesetzt bis der key released wird
        }

        /// <summary>
        /// Methoden für die Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #region events
        private void Game_MouseEvent(object sender, MouseEventArgs e)
        {
            //Anpassung auf die Skalierung
            InputManager.mouseX = (int)(e.X / StateManager.ScaleX);
            InputManager.mouseY = (int)(e.Y / StateManager.ScaleY);
            InputManager.mousePosition = new Vector(InputManager.mouseX, InputManager.mouseY);
            if (currentScreen != null)
            {
                currentScreen.Move(new MouseEventArgs(e.Button, e.Clicks, InputManager.mouseX, InputManager.mouseY, 0));
                return;
            }
            //Zoom
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
            //Spieler Interaktion Ingame oder im Screen
            currentScreen?.Click(new MouseEventArgs(e.Button, e.Clicks, InputManager.mouseX, InputManager.mouseY, 0));
            if (!isIngame || currentScreen != null || thePlayer.IsDead)
                return;
            //Schlagen und Zielen
            thePlayer.Swing();
            Vector yaw = InputManager.mousePosition - SIZE / 2;
            thePlayer.Yaw = MathUtils.ToDegree((float)Math.Atan2(yaw.Y, yaw.X)) + 90;
        }

        /// <summary>
        /// Ruft Tick auf für berechnung von Position und Kollision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TickTimer_Tick(object sender, EventArgs e)
        {
            //TODO: Call Tick, reset the StopWatch
            watch.Reset();
            watch.Start();
            OnTick();
        }

        /// <summary>
        /// Refreshed das Form damit ein Paint-Event erzeugt werden kann um dies neu zu Zeichnen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// Zeichnen der Form
        /// Setzen der Graphics-Instanz für den StateManager und ausrechung von PartialTicks(zeit bis zum nächsten tick für Interpoaltion 0-1)
        /// Ruft OnRender für das Zeichnen des Spiels auf
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// Setzt den momentanen Screen, schließt vorherigen und öffnet neuen
        /// </summary>
        /// <param name="screen"></param>
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

        /// <summary>
        /// Zeichnet das Spiel
        /// </summary>
        /// <param name="partialTicks"></param>
        public void OnRender(float partialTicks)
        {
            animationHandler.OnRender(partialTicks); //AnimationHandler als erstes für flüssige Animationen
            if(isIngame)
                RenderIngame(partialTicks);
            ingameGui?.OnRender(partialTicks);
            currentScreen?.OnRender(partialTicks);
        }

        /// <summary>
        /// Zeichnet das IngameSpiel
        /// </summary>
        /// <param name="partialTicks"></param>
        private void RenderIngame(float partialTicks)
        {
            StateManager.Push();
            //Skalierung um den Zoom
            StateManager.Scale(zoom.Value);
            //Translation des Spielers in FensterMitte
            Vector player = -MathUtils.Interpolate(thePlayer.PrevPosition, thePlayer.Position, partialTicks);
            StateManager.Translate(player);
            StateManager.Translate(WIDTH / 2f, HEIGHT / 2f);
            //Shaken des Screens wenn Spieler getroffen wurde
            float shake = MathUtils.Sin(thePlayer.HurtTime / (float)thePlayer.MaxHurtTime * 360 * 2) * 5;
            StateManager.Translate(shake, -shake);
            //Zeichnet den Dungeon
            RenderDungeon();
            StateManager.Pop();
            //Zeichnet alle Entities
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

        /// <summary>
        /// Zeichnet alle TileMaps des Dungeons
        /// </summary>
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
                        //Falls TileMap nicht sichtbar ist, wird diese nicht gezeichnet
                        if (xx < thePlayer.X - width / 2 - 30 || xx > thePlayer.X + width / 2 || yy < thePlayer.Y - height / 2 - 30 || yy > thePlayer.Y + height / 2)
                            continue;
                        StateManager.DrawImage(d.Fields[x, y].Anim.Image, xx, yy);
                    }
                }
            }
        }

        /// <summary>
        /// Berechnung von allen notwendingen Werten
        /// </summary>
        public void OnTick()
        {
            animationHandler.Update();
            inputManager.Update();//Updaten des InputManages für Key-Events
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].OnTick();
                if (((EntityLivingBase)Entities[i]).IsDead)
                    Entities.RemoveAt(i);
            }
            //Wenn Spiel zu ende ist, wird der Spieler wird in ein Menü geworfen
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
