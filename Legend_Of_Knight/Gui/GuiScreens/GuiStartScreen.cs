﻿using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Gui.GuiScreens
{
    public class GuiStartScreen : GuiScreen
    {
        private bool loading;
        private string loadingText;

        public GuiStartScreen()
        {
        }

        public override void Init(Game game)
        {
            base.Init(game);
            loading = false;
            GuiButton start = new GuiButton("Start", Width / 2 - 50, Height / 2, 100, 20)
            {
                Background = Color.Gray
            };
            start.OnClick += Start_OnClick;
            Components.Add(start);
        }

        public override void OnRender(float partialTicks)
        {
            base.OnRender(partialTicks);
            if (TimeUtils.Check(500) && loading)
            {
                loadingText += ".";
                if(loadingText.Length > 10)
                    loadingText = "Loading";
            }
            StateManager.SetColor(0, 0, 0);
            StateManager.DrawCenteredString(loadingText, Width / 2, Height / 3);
        }

        private void Start_OnClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            new Thread(() =>
            {
                loadingText = "Loading";
                loading = true;
                game.LoadIngame();
                game.SetScreen(null);
            }).Start();
        }
    }
}