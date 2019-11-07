using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend_Of_Knight
{
    public class Game : Form
    {
        public const float FPS = 60.0f, TPS = 30.0f;
        public const int WIDTH = 1280, HEIGHT = 720;
        public const string NAME = "Legend of Knight";
        private Timer renderTimer;

        public Game()
        {
            Init();
        }

        private void Init()
        {
            Name = NAME;
            Width = WIDTH;
            Height = HEIGHT;
            //FormBorderStyle = FormBorderStyle.None; //TODO: Später Header selbst schreiben
        }
    }
}
