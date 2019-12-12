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
    public class GuiScreen : Gui
    {
        private List<GuiLabel> components;
        private bool onClose;
        private GuiScreen prevScreen;
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

        public virtual void Init(Game game)
        {
            this.game = game;
            components = new List<GuiLabel>();
            Resize();
        }

        public void Resize()
        {
            Vector deltaSize = new Vector(Game.WIDTH, Game.HEIGHT) - Size;
            foreach (Gui g in components)
                g.Position += deltaSize / 2;
            Size = new Vector(Game.WIDTH, Game.HEIGHT);
        }

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

        public virtual void Close()
        {
            if (onClose)
                return;
            onClose = true;
            Animation.Reverse();
        }

        public override void OnRender(float partialTicks)
        {
            StateManager.Push();
            StateManager.Translate(0, GetAnimation<float>());
            for(int i = Components.Count - 1; i >= 0; i--) //Falls Components im Rendern entfernt werden
            {
                Components[i].OnRender(partialTicks);
            }
            StateManager.Pop();
        }
    }
}
