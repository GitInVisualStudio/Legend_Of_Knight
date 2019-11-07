using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils
{
    public class InputManager
    {

        private List<Keybind> keys;

        public InputManager()
        {
            keys = new List<Keybind>();
        }

        public void OnKeyPressed(int keyChar)
        {
            Keybind key = keys.Find(x => x.KeyChar == keyChar);
            if (key != null)
                key.Pressed = true;
        }

        public void OnKeyRelease(int keyChar)
        {
            Keybind key = keys.Find(x => x.KeyChar == keyChar);
            if (key != null)
                key.Pressed = false;
        }

        public delegate void Event();

        public void Add(int keyChar, Event OnPress)
        {
            keys.Add(new Keybind(keyChar, OnPress));
        }

        public void Add(int keyChar)
        {
            keys.Add(new Keybind(keyChar, null));
        }

        class Keybind
        {

            private int keyChar;
            private bool pressed;
            public Event OnPress;
            public Keybind(int keyChar, Event OnPress)
            {
                KeyChar = keyChar;
                this.OnPress = OnPress;
                Pressed = false;
            }

            public int KeyChar
            {
                get
                {
                    return keyChar;
                }

                set
                {
                    keyChar = value;
                }
            }

            public bool Pressed
            {
                get
                {
                    return pressed;
                }

                set
                {
                    if (value)
                        OnPress?.Invoke();
                    pressed = value;
                }
            }
        }
    }
}
