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
    /// <summary>
    /// Checkbox zum auswählen setzen von Flags
    /// </summary>
    public class GuiCheckbox : GuiLabel
    {
        private bool state;
        private CustomAnimation<float> check;
        private float tokenWidth;
        private string token;

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
            if(e.X > this.X + Width - tokenWidth && OnHover(e))
            {
                if(check.Finished)
                    check.Reverse();
            }
        }

        private void CreateNewAnimation()
        {
            check = new CustomAnimation<float>(0.0f, 1.0f, (float current, float delta) => current + delta)
            {
                Toleranz = 1E-3f
            };//Animation für den Haken
            check.OnFinish += (object o, EventArgs args) => {
                if (!check.Increments)//Soll wieder groß werden wenn es klein geworden ist
                {
                    state = !state;
                    token = state ? ((char)10003).ToString() : ((char)10007).ToString();
                    check.Reverse();
                }
            };
            check.Fire();
        }

        public GuiCheckbox(string text, bool state)
        {
            Text = text;
            State = state;
            CreateNewAnimation();
            OnClick += GuiCheckbox_OnClick;
            token = state ? ((char)10003).ToString() : ((char)10007).ToString();
        }

        /// <summary>
        /// Zeichnen der Box
        /// </summary>
        /// <param name="partialTicks"></param>
        public override void OnRender(float partialTicks)
        {
            StateManager.SetColor(Color);
            Vector stringSize = StateManager.GetStringSize(token)/2;
            tokenWidth = stringSize.X * 2;
            StateManager.DrawString(Text, Position);
            StateManager.Push();
            StateManager.Translate(X + Width - stringSize.X, Y + stringSize.Y);//Translation auf die Mitte des Status
            StateManager.Scale(check.Value + 0.001f);//Skalierung
            StateManager.Rotate(360 * check.Value);//Rotation für die Animation
            StateManager.DrawString(token, -stringSize);//Zeichnen des Status
            StateManager.Pop();
        }
    }
}
