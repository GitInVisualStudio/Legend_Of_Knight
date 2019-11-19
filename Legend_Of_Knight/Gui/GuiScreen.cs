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
        private List<Gui> components;
        private bool onClose;
        public List<Gui> Components { get => components; set => components = value; }
        public bool IsClosed => Animation.Finished && onClose;

        public GuiScreen()
        {
            OnClick += CallEvents;
            OnMove += CallEvents;
            OnRelease += CallEvents;
        }

        private void CallEvents(object sender, MouseEventArgs e)
        {
            foreach (Gui gui in Components)
                gui.Click(e);
        }

        public virtual void Open()
        {
            Animation = new CustomAnimation<float>(0.0f, 1.0f, (float delta, float current) => current + delta);
            Animation.Fire();
        }

        public virtual void Close()
        {
            onClose = true;
            Animation.Reset();
            Animation.Fire();
        }

        public override void OnRender(float partialTicks)
        {
            StateManager.Push();
            StateManager.Translate(0, Game.HEIGHT - GetAnimation<float>() * Game.HEIGHT);
            for(int i = components.Count - 1; i >= 0; i--) //Falls Components im Rendern entfernt werden
            {
                components[i].OnRender(partialTicks);
            }
            StateManager.Pop();
        }
    }
}
