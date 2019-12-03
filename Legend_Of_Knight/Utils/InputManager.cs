using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils
{
    public class InputManager
    {
        public static int mouseX;
        public static int mouseY;
        public static Vector mousePosition = new Vector(2);
        private List<Keybind> keys;

        public InputManager()
        {
            keys = new List<Keybind>();
        }

        public void OnKeyPressed(int keyChar)
        {
            Keybind key = keys.Find(x => x.KeyChar == keyChar);
            if (key != null)
            {
                if (key.FireOnce && !key.Pressed) //WindowsForms events so geil gemacht, dass KeyPressed tatsächlich mehrfach gecalled wird ohne key los zu lassen
                    key.OnPress?.Invoke();
                key.Pressed = true;
            }
        }

        public void OnKeyRelease(int keyChar)
        {
            Keybind key = keys.Find(x => x.KeyChar == keyChar);
            if (key != null)
            { 
                key.Pressed = false;
            }
        }

        public delegate void Event();

        public void Update()
        {
            keys.ForEach(x => {
                if (x.Pressed && !x.FireOnce)
                    x.OnPress?.Invoke();
            });
        }

        public void Add(int keyChar, Event OnPress, bool fireOnce = false) => keys.Add(new Keybind(keyChar, OnPress, fireOnce));
        

        class Keybind
        {

            private int keyChar;
            private bool pressed;
            private bool fireOnce;
            public Event OnPress;
            public Keybind(int keyChar, Event OnPress, bool fireOnce)
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
                    pressed = value;
                }
            }

            public bool FireOnce { get => fireOnce; set => fireOnce = value; }
        }
    }
}
