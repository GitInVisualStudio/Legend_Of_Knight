using Legend_Of_Knight.Utils.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend_Of_Knight.Gui
{
    public class GuiCheckbox : GuiLabel
    {
        private bool state;
        private Animation check;

        public bool State
        {
            get
            {
                return state;
            }

            set
            {
                this.state = value;
            }
        }

        public GuiCheckbox()
        {
            OnClick += GuiCheckbox_OnClick;
        }

        private void GuiCheckbox_OnClick(object sender, MouseEventArgs e)
        {
            if(e.X > this.X + Width - 5)
            {
                check.Reverse();
                state = !state;
            }
        }

        public GuiCheckbox(string text, bool state)
        {
            Text = text;
            State = state;
            check = new CustomAnimation<float>(0.0f, 1.0f, (float current, float delta) => current + delta).Fire();
        }

        public override void OnRender(float partialTicks)
        {
            base.OnRender(partialTicks);

        }
    }
}
