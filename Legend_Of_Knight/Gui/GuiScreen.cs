using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend_Of_Knight.Gui
{
    //Dient zur verwaltung von GUI
    public class GuiScreen : Gui
    {
        //Komponenten auf dem Screen
        private List<GuiLabel> components;
        private bool onClose;
        private GuiScreen prevScreen;//vorheriger screen, falls diese verschachtelt sind
        protected Game game;
        public bool IsClosed => Animation.Finished && onClose;

        public List<GuiLabel> Components
        {
            get
            {
                return components;
            }

            set
            {
                components = value;
            }
        }

        public GuiScreen()
        {
            //Hinzufügen der Eingabe-Events für alle Komponenten
            OnClick += (object sender, MouseEventArgs e) =>
            {
                foreach (Gui gui in Components)
                    gui.Click(e);
            };
            OnRelease += (object sender, MouseEventArgs e) =>
            {
                foreach (Gui gui in Components)
                    gui.Release(e);
            };
            OnMove += (object sender, MouseEventArgs e) =>
            {
                foreach (Gui gui in Components)
                    gui.Move(e);
            };
            OnKeyPressed += (object sender, KeyEventArgs e) =>
            {
                if(e.KeyValue == 27)
                {
                    game.SetScreen(prevScreen);
                    return;
                }
                foreach (Gui gui in Components)
                    gui.KeyPressed(e);
            };
        }

        /// <summary>
        /// Erstellt alle Komponenten
        /// </summary>
        /// <param name="game"></param>
        public virtual void Init(Game game)
        {
            this.game = game;
            components = new List<GuiLabel>();
            Resize();
        }

        /// <summary>
        /// Falls das Form Resized wird
        /// </summary>
        public void Resize()
        {
            Vector deltaSize = new Vector(Game.WIDTH, Game.HEIGHT) - Size;
            foreach (Gui g in components)
                g.Position += deltaSize / 2;
            Size = new Vector(Game.WIDTH, Game.HEIGHT);
        }

        /// <summary>
        /// Öffnet den Screen mit einer Animation
        /// </summary>
        /// <param name="prevScreen"></param>
        /// <returns></returns>
        public virtual GuiScreen Open(GuiScreen prevScreen)
        {
            this.prevScreen = prevScreen;
            Animation = new CustomAnimation<float>(-Game.HEIGHT, 0.0f, (float delta, float current) => current + delta)
            {
                Toleranz = 1E-3f
            };
            Animation.Fire();
            return this;
        }

        /// <summary>
        /// Schließt den Screen
        /// </summary>
        public virtual void Close()
        {
            if (onClose)
                return;
            onClose = true;
            Animation.Reverse();
        }

        /// <summary>
        /// Rendert alle Komponenten des Screens
        /// </summary>
        /// <param name="partialTicks"></param>
        public override void OnRender(float partialTicks)
        {
            StateManager.Push();
            StateManager.Translate(0, GetAnimation<float>());//Translation für Start- und CloseAnimation
            for(int i = Components.Count - 1; i >= 0; i--) //Falls Components im Rendern entfernt werden
            {
                Components[i].OnRender(partialTicks);
            }
            StateManager.Pop();
        }
    }
}
