using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils
{
    /// <summary>
    /// Dient zur einfachen verwaltung von Eingaben
    /// </summary>
    public class InputManager
    {
        //Position der Maus
        public static int mouseX;
        public static int mouseY;
        public static Vector mousePosition = new Vector(2);
        private List<Keybind> keys;//Liste der Keybinds

        public InputManager()
        {
            keys = new List<Keybind>();
        }

        //Ruft entsprechende Events auf
        public void OnKeyPressed(int keyChar)
        {
            Keybind key = keys.Find(x => x.KeyChar == keyChar);//findet gedrückten Key
            if (key != null)
            {
                if (key.FireOnce && !key.Pressed) //WindowsForms events so geil gemacht, dass KeyPressed tatsächlich mehrfach gecalled wird ohne key los zu lassen
                    key.OnPress?.Invoke();
                key.Pressed = true;
            }
        }

        public void OnKeyRelease(int keyChar)
        {
            Keybind key = keys.Find(x => x.KeyChar == keyChar);//findet gedrückten Key
            if (key != null)
            { 
                key.Pressed = false;
            }
        }

        //Deligiertes Event beim drücken des Keys
        public delegate void Event();

        /// <summary>
        /// Damit das Event auch jeden Ticks aufgerufen wird
        /// </summary>
        public void Update()
        {
            keys.ForEach(x => {
                if (x.Pressed && !x.FireOnce)
                    x.OnPress?.Invoke();
            });
        }

        //Hinzufügen von Keybinds
        public void Add(int keyChar, Event OnPress, bool fireOnce = false) => keys.Add(new Keybind(keyChar, OnPress, fireOnce));
        
        /// <summary>
        /// Dient zur vereinfachung der KeyBinds und dessen Events
        /// </summary>
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

            public bool FireOnce
            {
                get
                {
                    return fireOnce;
                }

                set
                {
                    fireOnce = value;
                }
            }
        }
    }
}
