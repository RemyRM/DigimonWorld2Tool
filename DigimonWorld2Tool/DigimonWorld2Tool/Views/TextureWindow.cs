using DigimonWorld2Tool.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DigimonWorld2Tool.Views
{
    public partial class TextureWindow : UserControl, IHostWindow
    {
        public TextureWindow()
        {
            InitializeComponent();
        }

        public void OnWindowResizeEnded()
        {
            //throw new NotImplementedException();
        }
    }
}
