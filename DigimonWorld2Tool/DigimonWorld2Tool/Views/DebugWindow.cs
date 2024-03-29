﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DigimonWorld2Tool.Views
{
    public partial class DebugWindow : Form
    {
        public static BindingList<string> DebugLogMessages { get; private set; } = new BindingList<string>();

        public DebugWindow()
        {
            InitializeComponent();
            this.BackColor = (Color)Settings.Settings.BackgroundColour;
            this.ForeColor = (Color)Settings.Settings.TextColour;
            Utility.ColourTheme.SetColourScheme(this.Controls);

            DebugMessageListBox.DataSource = DebugLogMessages;
        }

        /// <summary>
        /// Prevent the DebugWindow from being disposed upon closing, but hide it instead
        /// </summary>
        private void DebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
